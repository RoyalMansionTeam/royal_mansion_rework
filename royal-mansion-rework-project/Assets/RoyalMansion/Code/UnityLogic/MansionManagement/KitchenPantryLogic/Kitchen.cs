using RoyalMansion.Code.UI.WorldspaceUI;
using RoyalMansion.Code.UnityLogic.CameraLogic;
using RoyalMansion.Code.UnityLogic.NPC;
using RoyalMasion.Code.Editor;
using RoyalMasion.Code.Infrastructure.Data;
using RoyalMasion.Code.Infrastructure.Saving;
using RoyalMasion.Code.Infrastructure.Services.AssetProvider;
using RoyalMasion.Code.Infrastructure.Services.ProjectData;
using RoyalMasion.Code.Infrastructure.Services.SaveLoadService;
using RoyalMasion.Code.Infrastructure.Services.SceneContext;
using RoyalMasion.Code.Infrastructure.Services.StaticData;
using RoyalMasion.Code.Infrastructure.Services.UIFactory;
using RoyalMasion.Code.UnityLogic.MasionManagement.ApartmentLogic;
using RoyalMasion.Code.UnityLogic.MasionManagement.GardenLogic;
using RoyalMasion.Code.UnityLogic.MasionManagement.MansionStateMachine;
using RoyalMasion.Code.UnityLogic.MasionManagement.MansionStateMachine.States;
using RoyalMasion.Code.UnityLogic.MasionManagement.StaffRecruitmentLogic;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VContainer;

namespace RoyalMasion.Code.UnityLogic.MasionManagement.KitchenGardenLogic
{
    public class Kitchen : MonoBehaviour, ISaveReader
    {
        [SerializeField] private ObjectClickHandler _touchHandler;
        [SerializeField] private Transform _waiterSpawnPoint;
        [SerializeField] private Transform _cookSpawnPoint;

        private List<GuestNPC> _guestsToOrder = new();
        private List<WaiterNPC> _waiterNPCs = new();
        private List<CookNPC> _cookNPCs = new();
        private bool _isAbleToCook;

        private GuestNPC _currentGuest;
        private INpcFactory _npcFactory;
        private IUIFactory _uiFactory;
        private ISceneContextService _sceneContext;
        private IAssetProvider _assetProvider;
        private IMansionFactory _mansionFactory;
        private IStaticDataService _staticDataService;
        private IEconomyDataService _economyData;
        
        private WaiterNPC _targetWaiter;
        private CookNPC _targetCook;
        private int _orderResourceCost;
        private RecruitmentMessagesConfig _recruitmentConfig;
        private MansionUnitUIHandler _unitUIHandler;
        private RecruitmentMessageData _cookMessageData;

        public Transform CookSpawnPoint => _cookSpawnPoint;

        [Inject]
        public void Construct(INpcFactory npcFactory, IStaticDataService staticDataService,
            IEconomyDataService economyData, IUIFactory uiFactory, ISceneContextService sceneContext,
            IAssetProvider assetProvider, IMansionFactory mansionFactory)
        {
            _npcFactory = npcFactory;
            _staticDataService = staticDataService;
            _economyData = economyData;
            _uiFactory = uiFactory;
            _sceneContext = sceneContext;
            _assetProvider = assetProvider;
            _mansionFactory = mansionFactory;

            InitData();
            RegisterSaveableEntity();
        }

        private void InitData() =>
            _orderResourceCost = _staticDataService.GameData.
            PlaytestStaticData.TestOrderData.OrderResourceCost;

        public void LoadProgress(GameProgress progress)
        {
            SpawnStaff(progress);
            CheckCookAmount();
        }
        private void Start()
        {
            _isAbleToCook = false;
            Subscribe();          
        }


        private void Subscribe()
        {
            _touchHandler.ClickHandled += HandleTouch;
        }

        private void Unsubscribe()
        {
            _touchHandler.ClickHandled -= HandleTouch;
        }


        private void HandleTouch()
        {
            if (_cookNPCs.Count == 0)
            {
                _sceneContext.StaffRecruitmentService.SpawnRecruitmentWindow(_cookMessageData);
                _sceneContext.StaffRecruitmentService.StaffRecruited += TryDespawnKitchenUI;
                Debug.Log("OpenWindow");
                return;
            }
            HandleCollectable();
            HandleOrder();
        }
        private async void CheckCookAmount()
        {
            if (_cookNPCs.Count > 0)
                return;
            _recruitmentConfig = await _assetProvider.Load<RecruitmentMessagesConfig>(AssetAddress.RECRUITMENT_MESSAGES_CONFIG);
            foreach(RecruitmentMessageData message in _recruitmentConfig.MessageDatas)
            {
                if (message.Type != NpcType.Cook)
                    continue;
                _cookMessageData = message;
                break;
            }
            _unitUIHandler = _uiFactory.CreateUnitUIHandler();
            _unitUIHandler.SetHandlerData(_cookSpawnPoint.position, _sceneContext.CinemachineHandler.MainCamera.GetComponent<DraggableCamera>());
            await _unitUIHandler.SetUIMessenge(InternalUnitStates.Warning);
        }
        private void TryDespawnKitchenUI(NpcType actionTartetType)
        {
            if (actionTartetType != NpcType.Cook)
                return;
            _sceneContext.StaffRecruitmentService.StaffRecruited -= TryDespawnKitchenUI;
            _unitUIHandler.gameObject.SetActive(false);
            OnOrderConditionsChanged();
        }

        public void AddCook(CookNPC npc)
        {
            npc.AssignedUnitID = "Kitchen";
            _cookNPCs.Add(npc);
        }

        public void SendWaiterBack(NpcBase npc) 
        {
            if (npc.AssignedUnitID != "Waiter")
                return;
            npc.gameObject.GetComponent<WaiterNPC>().SetTarget(_waiterSpawnPoint);
            npc.gameObject.GetComponent<WaiterNPC>().EnterWaiterBehaviour(NpcState.Resting);
        }

        public void AddToOrderList(GuestNPC guest)
        {
            if (!_guestsToOrder.Contains(guest))
                _guestsToOrder.Add(guest);
            OnOrderConditionsChanged();
        }

        private void SpawnStaff(GameProgress progress)
        {
            for (int i = 0; i < _staticDataService.GameData.PlaytestStaticData.WaiterNumber; i++)
            {
                WaiterNPC npc = _npcFactory.SpawnNpc<WaiterNPC>(_waiterSpawnPoint);
                npc.SetNPCData(_uiFactory, 
                    _sceneContext.CinemachineHandler.MainCamera.GetComponent<DraggableCamera>());
                _waiterNPCs.Add(npc);
            }
            if (progress.MansionProgress.NpcSaveData == null)
                return;
            foreach(NpcSaveData npcData in progress.MansionProgress.NpcSaveData)
            {
                if (npcData.AssignedUnitID != "Kitchen")
                    continue;
                CookNPC npc = _npcFactory.SpawnNpc<CookNPC>(_cookSpawnPoint);
                npc.SetNPCData(_uiFactory,
                    _sceneContext.CinemachineHandler.MainCamera.GetComponent<DraggableCamera>());
                AddCook(npc);
            }

        }

        private void OnOrderConditionsChanged() => 
            SetKitchenAvailability
            (ResourcesAvailable() & OrdersAvailable() & StaffAvailable());

        private bool ResourcesAvailable() => 
            _economyData.GetEconomyData(ResourceType.Fruit) >= _orderResourceCost;
        private bool OrdersAvailable()
        {
            return _guestsToOrder.Count != 0;
        }

        private bool StaffAvailable()
        {
            bool availableWaiter = false;
            foreach (WaiterNPC waiter in _waiterNPCs)
            {
                if (waiter.CurrentBehavior.State == NpcState.Resting)
                {
                    availableWaiter = true;
                    _targetWaiter = waiter;
                    break;
                }
            }
            bool availableCook = false;
            foreach (CookNPC cook in _cookNPCs)
            {
                if (cook.CurrentBehavior.State == NpcState.Resting)
                {
                    availableCook = true;
                    _targetCook = cook;
                    break;
                }
            }
            return availableCook & availableWaiter;
        }

        private void SetKitchenAvailability(bool state)
        {
            _isAbleToCook = state;
            if (state)
                _targetCook.PlaceUI(InternalUnitStates.KitchenReadyToOrder);
            
        }


        private void HandleOrder()
        {
            if (!_isAbleToCook)
                return;
            _currentGuest = _guestsToOrder[0];
            _targetCook.SetTaskBehaviour(_targetWaiter,
                _staticDataService.GameData.PlaytestStaticData.TestOrderData);
            _targetWaiter.SetTarget(_currentGuest.transform);
            RemoveFromOrderList(_currentGuest);
            _economyData.SetEconomyData(ResourceType.Fruit, -1 * _orderResourceCost);
        }
        private void HandleCollectable()
        {
            foreach(CookNPC cookNPC in _cookNPCs)
            {
                if (!cookNPC.AvailableToCollect)
                    continue;
                _economyData.SetEconomyData(ResourceType.SoftVallue, cookNPC.OrderReward);
                cookNPC.Clear();
            }
        }
        public void RemoveFromOrderList(GuestNPC guest)
        {
            if (_guestsToOrder.Contains(guest))
                _guestsToOrder.Remove(guest);
            OnOrderConditionsChanged();
        }
        private void RegisterSaveableEntity() => 
            _mansionFactory.RegisterSaveableEntity(this);

        private void OnDestroy()
        {
            Unsubscribe();
        }

    }
}
