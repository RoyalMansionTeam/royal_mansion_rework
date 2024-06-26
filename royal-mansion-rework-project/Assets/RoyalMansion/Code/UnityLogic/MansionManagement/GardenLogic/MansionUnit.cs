using UnityEngine;
using UnityEngine.UI;
using RoyalMasion.Code.Infrastructure.Data;
using System.Collections.Generic;
using VContainer;
using RoyalMasion.Code.Infrastructure.Services.ProjectData;
using RoyalMasion.Code.Infrastructure.Services.StaticData;
using System;
using RoyalMasion.Code.UnityLogic.MasionManagement.MansionStateMachine;
using RoyalMansion.Code.Extensions.Utils;
using RoyalMasion.Code.UnityLogic.MasionManagement.ApartmentLogic;
using RoyalMansion.Code.UnityLogic.NPC;
using RoyalMasion.Code.Infrastructure.Services.SceneContext;
using RoyalMasion.Code.Infrastructure.Services.UIFactory;
using RoyalMasion.Code.Infrastructure.Services.SaveLoadService;
using RoyalMasion.Code.Infrastructure.Saving;
using RoyalMasion.Code.Extensions;
using RoyalMansion.Code.UnityLogic.Catalog;
using System.Threading.Tasks;
using RoyalMasion.Code.UnityLogic.MasionManagement.MansionStateMachine.States;
using RoyalMansion.Code.UI.WorldspaceUI;

namespace RoyalMasion.Code.UnityLogic.MasionManagement.GardenLogic
{
    public class MansionUnit : MonoBehaviour, ISaveReader, ISaveWriter
    {
        public string SaveableID { get; set; }

        protected IMansionStateMachine StateMashine;
        protected INpcFactory _npcFactory;
        protected UnitState StartState;
        protected UnitStaticData UnitData;
        protected Action<CatalogSection> ItemBoughtEvent;
        protected MansionStateMachineData _stateMashineData;
        protected bool BasicUnitRequirementsMet;

        [SerializeField] private MansionUnitUIHandler _uiHandler;
        [SerializeField] private Transform _newItemSpawnPoint;
        [SerializeField] private Transform _itemsParent;
        [SerializeField] private Transform _navmeshTarget;

        private ISceneContextService _sceneContext;
        private IUIFactory _uiFactory;
        private IEconomyDataService _econommyDataService;
        private IMansionFactory _mansionFactory;
        private NpcSaveData _npcSaveData;

        [Inject]
        public void Construct(IEconomyDataService dataService,
            IMansionFactory mansionFactory, INpcFactory npcFactory,
            ISceneContextService sceneContext, IUIFactory uiFactory)
        {
            _econommyDataService = dataService;
            _mansionFactory = mansionFactory;
            _npcFactory = npcFactory;
            _sceneContext = sceneContext;
            _uiFactory = uiFactory;

            RegisterSaveableEntity();
        }


        protected void EnterFirstState()
        {
            switch (StartState)
            {
                case UnitState.Locked:
                    StateMashine.Enter<LockedState>();
                    break;
                case UnitState.Empty:
                    StateMashine.Enter<EmptyState>();
                    break;
                case UnitState.InUse:
                    StateMashine.Enter<InUseState>();
                    break;
                case UnitState.Collectable:
                    StateMashine.Enter<CollectableState, UnitTaskData>(
                        UnitData.GetTaskData(StateMashine.LastTask));
                    break;
                case UnitState.Dirty:
                    StateMashine.Enter<DirtyState>();
                    break;
            }
        }

        protected void InitUnitData(UnitStaticData unitData)
        {
            UnitData = unitData;
            SaveableID = UnitData.UnitID;
        }

        public void NpcEnteredUnitEvent()
        {
            StateMashine.NPC.OnUnitAchieved();
        }

        private void InitStateMachine()
        {
            _stateMashineData = new MansionStateMachineData
                (
                    economyDataService: _econommyDataService,
                    npcFactory: _npcFactory,
                    unitStaticData: UnitData,
                    sceneContext: _sceneContext,
                    uiFactory: _uiFactory,
                    itemSpawnPoint: _newItemSpawnPoint,
                    navMeshTarget: _navmeshTarget,
                    unitUIHandler: _uiHandler,
                    itemBoughtEvent: ItemBoughtEvent,
                    npcSaveData: _npcSaveData
                );
            StateMashine = new MansionStateMachine.MansionStateMachine(_stateMashineData);
        }
        public void LoadProgress(GameProgress progress)
        {
            if (progress.MansionProgress.MansionUnitsSave == null)
            {
                InitStateMachine();
                EnterFirstState();
                return;
            }
            foreach (CatalogItemSaveData catalogItem in progress.MansionProgress.CatalogItemsSave)
            {
                if (catalogItem.AssignedUnitID != UnitData.UnitID)
                    continue;
                LoadMansionItem(catalogItem);
            }
            foreach (NpcSaveData npcSaveData in progress.MansionProgress.NpcSaveData)
            {
                if (npcSaveData.AssignedUnitID != UnitData.UnitID)
                    continue;
                Debug.Log("Load");
                _npcSaveData = npcSaveData;
            }
            foreach (MansionUnitSaveData unitData in progress.MansionProgress.MansionUnitsSave)
            {
                if (unitData.UniqueSaveID != SaveableID)
                    continue;
                LoadUnitState(unitData);
                break;
            }
        }


        private void LoadUnitState(MansionUnitSaveData unitData)
        {
            InitStateMachine();
            StartState = (UnitState)unitData.UnitStateID;
            StateMashine.LastTask = (UnitState)unitData.TaskUnitID;
            EnterFirstState();
        }

        private async void LoadMansionItem(CatalogItemSaveData catalogItem)
        {
            GameObject instance = await _mansionFactory.CreateUnitObject(
                                reference: catalogItem.AssetGUID,
                                at: catalogItem.Position.AsUnityVector(),
                                parent: _itemsParent);
            instance.transform.localScale = Vector3.one;
            instance.GetComponent<UnitItem>().SaveableID = catalogItem.UniqueSaveID;
            instance.GetComponent<UnitItem>().SetItemData(catalogItem.AssignedUnitID, catalogItem.AssetGUID);
            Destroy(instance.GetComponent<DragAndDrop>());
        }

        private void RegisterSaveableEntity()
        {
            _mansionFactory.RegisterSaveableEntity(this);
        }

        public void SaveProgress(GameProgress progress)
        {
            progress.MansionProgress.TryAddUnit(
                new MansionUnitSaveData(
                    uSID: SaveableID,
                    unitState: (int)StateMashine.GetUnitStateEnum(StateMashine.ActiveState.GetType()),
                    taskUnit: (int)StateMashine.LastTask
                ));
        }

    }

}
