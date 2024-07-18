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
using RoyalMansion.Code.UnityLogic.CameraLogic;
using RoyalMasion.Code.Infrastructure.Services.AssetProvider;

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
        protected Action StateMachineInitiated;
        protected MansionStateMachineData _stateMashineData;
        protected bool BasicUnitRequirementsMet;
        protected List<ApartmentMaterialParents> MaterialsData;

        [SerializeField] private Transform _newItemSpawnPoint;
        [SerializeField] private Transform _navmeshTarget;
        [SerializeField] private Transform _uiAnchorPoint;
        [SerializeField] private UnitVirtualCamerasData _unitCameras;

        private MansionUnitUIHandler _uiHandler;
        private ISceneContextService _sceneContext;
        private IUIFactory _uiFactory;
        private IEconomyDataService _econommyDataService;
        private IMansionFactory _mansionFactory;
        private IAssetProvider _assetProvider;
        private NpcSaveData _npcSaveData;

        [Inject]
        public void Construct(IEconomyDataService dataService,
            IMansionFactory mansionFactory, INpcFactory npcFactory,
            ISceneContextService sceneContext, IUIFactory uiFactory,
            IAssetProvider assetProvider)
        {
            _econommyDataService = dataService;
            _mansionFactory = mansionFactory;
            _npcFactory = npcFactory;
            _sceneContext = sceneContext;
            _uiFactory = uiFactory;
            _assetProvider = assetProvider;
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

        protected void InitUnitUI()
        {
            _uiHandler = _uiFactory.CreateUnitUIHandler();
            _uiHandler.SetHandlerData(_uiAnchorPoint.position,
                _sceneContext.CinemachineHandler.MainCamera.GetComponent<DraggableCamera>());
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
                    npcSaveData: _npcSaveData,
                    basicRequirementsMetState: BasicUnitRequirementsMet,
                    virtialCameraData: _unitCameras.VirtialCameras
                ) ;
            StateMashine = new MansionStateMachine.MansionStateMachine(_stateMashineData);
            StateMachineInitiated?.Invoke();
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

            if (MaterialsData == null)
                return;
            LoadApartmentMaterials(unitData);
        }

        private void LoadApartmentMaterials(MansionUnitSaveData unitData)
        {
            ApplyMaterial(unitData.BathroomWallID, MaterialsData[0].WallsHandler);
            ApplyMaterial(unitData.BathroomFloorID, MaterialsData[0].FloorsHandler);
            ApplyMaterial(unitData.BedroomWallID, MaterialsData[1].WallsHandler);
            ApplyMaterial(unitData.BedroomFloorID, MaterialsData[1].FloorsHandler);
        }

        private async void ApplyMaterial(string materialID, ApartmentMaterialHandler materialsHandler)
        {
            if (materialID == "")
                return;
            Material wallMaterial = await _assetProvider.Load<Material>(materialID);
            materialsHandler.TryMaterial(wallMaterial, materialID);
            materialsHandler.ConfirmMaterial();
        }

        private async void LoadMansionItem(CatalogItemSaveData catalogItem)
        {
            GameObject instance = await _mansionFactory.CreateUnitObject(
                                reference: catalogItem.AssetGUID,
                                at: catalogItem.Position.AsUnityVector(),
                                parent: _newItemSpawnPoint);
            CatalogSection instanceSection = (CatalogSection)catalogItem.CatalogSectionID;
            instance.transform.localScale = Vector3.one;
            instance.transform.rotation *= Quaternion.Euler(catalogItem.Rotation.AsUnityVector());
            instance.GetComponent<UnitItem>().SaveableID = catalogItem.UniqueSaveID;
            instance.GetComponent<UnitItem>().SetItemData(
                catalogItem.AssignedUnitID, 
                catalogItem.AssetGUID,
                instanceSection);
            ItemBoughtEvent?.Invoke(instanceSection);
            Destroy(instance.GetComponent<DragAndDrop>());
        }

        private void RegisterSaveableEntity()
        {
            _mansionFactory.RegisterSaveableEntity((ISaveReader)this);
        }

        public void SaveProgress(GameProgress progress)
        {
            progress.MansionProgress.TryAddUnit(
                new MansionUnitSaveData(
                    uSID: SaveableID,
                    unitState: (int)StateMashine.GetUnitStateEnum(StateMashine.ActiveState.GetType()),
                    taskUnit: (int)StateMashine.LastTask,
                    bathroomWallID: MaterialsData[0].WallsHandler.AddressableMaterialID,
                    bedroomWallID: MaterialsData[1].WallsHandler.AddressableMaterialID,
                    bathroomFloorID: MaterialsData[0].FloorsHandler.AddressableMaterialID,
                    bedroomFloorID: MaterialsData[1].FloorsHandler.AddressableMaterialID
                ));
        }

    }

}
