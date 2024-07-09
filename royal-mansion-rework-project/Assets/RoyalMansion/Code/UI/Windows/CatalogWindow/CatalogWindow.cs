using RoyalMansion.Code.UnityLogic.CameraLogic;
using RoyalMansion.Code.UnityLogic.Catalog;
using RoyalMasion.Code.Infrastructure.Data;
using RoyalMasion.Code.Infrastructure.Services.AssetProvider;
using RoyalMasion.Code.Infrastructure.Services.ProjectData;
using RoyalMasion.Code.Infrastructure.Services.SceneContext;
using RoyalMasion.Code.UI.Windows;
using RoyalMasion.Code.UnityLogic.MasionManagement;
using RoyalMasion.Code.UnityLogic.MasionManagement.ApartmentLogic;
using RoyalMasion.Code.UnityLogic.MasionManagement.GardenLogic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UI;
using VContainer;

namespace RoyalMansion.Code.UI.Windows.Catalog
{
    public class CatalogWindow : WindowBase
    {
        [SerializeField] private Transform _sectionsLayoutGroup;
        [SerializeField] private Transform _itemsLayoutGroup;

        [SerializeField] private AssetReference _sectionReference;
        [SerializeField] private AssetReference _itemReference;

        [SerializeField] private GameObject _catalogUI;
        [SerializeField] private GameObject _placementUI;

        [SerializeField] private Button _placementCancel;
        [SerializeField] private Button _placementApply;
        [SerializeField] private Button _placementRotate;

        [SerializeField] private List<CatalogAreaUI> _areaSwitchers;

        private IAssetProvider _assetProvider;
        private IMansionFactory _mansionFactory;
        private IEconomyDataService _economyService;
        private ISceneContextService _sceneContext;

        private Dictionary<CatalogSection, List<CatalogItemStaticData>> _catalogData;
        private Dictionary<CatalogSection, List<CatalogItemStaticData>> _mainCatalogData;
        private Dictionary<CatalogSection, List<CatalogItemStaticData>> _extraCatalogData;
        private GameObject _sectionPrefab;
        private GameObject _itemPrefab;
        private ApartmentAreaType _currentApartmentArea;
        private List<ApartmentMaterialParents> _apartmentMaterialsData;
        private CatalogSection _currentSection;
        private Transform _newItemSpawnPoint;
        private GameObject _objectInPlacing;
        private CatalogItemStaticData _objectInPlacingData;
        private string _unitID;
        private Action<CatalogSection> _unitOnBuyEvent;
        private List<UnitVirtualCamera> _virtualCameras;

        [Inject]
        public void Construct(IAssetProvider assetProvider, IMansionFactory mansionFactory,
            IEconomyDataService economyService, ISceneContextService sceneContext)
        {
            _assetProvider = assetProvider;
            _mansionFactory = mansionFactory;
            _economyService = economyService;
            _sceneContext = sceneContext;
        }

        private void Start()
        {
            Subscribe();
            _placementUI.SetActive(false);
        }

        private void Subscribe()
        {
            _placementCancel.onClick.AddListener(CancelPlacement);
            _placementApply.onClick.AddListener(ApplylPlacement);
            _placementRotate.onClick.AddListener(RotatePlacement);
            if (_areaSwitchers.Count == 0)
                return;
            foreach (CatalogAreaUI handler in _areaSwitchers)
                handler.PickArea += SwitchArea;
        }


        private void Unsubscribe()
        {
            _placementCancel.onClick.RemoveListener(CancelPlacement);
            _placementApply.onClick.RemoveListener(ApplylPlacement);
            if (_areaSwitchers.Count == 0)
                return;
            foreach (CatalogAreaUI handler in _areaSwitchers)
                handler.PickArea -= SwitchArea;
        }

        private async Task LoadAssets()
        {
            _sectionPrefab = await _assetProvider.Load<GameObject>(_sectionReference);
            _itemPrefab = await _assetProvider.Load<GameObject>(_itemReference);
        }

        public async void SetUnitType(UnitType targetType, Transform spawnPoint,
            string unitID, Action<CatalogSection> unitOnBuyEvent, List<UnitVirtualCamera> virtualCameras,
            List<ApartmentMaterialParents> apartmentMaterialsData)
        {
            _newItemSpawnPoint = spawnPoint;
            _unitID = unitID;
            _unitOnBuyEvent = unitOnBuyEvent;
            _virtualCameras = virtualCameras;
            _currentApartmentArea = ApartmentAreaType.Bedroom;
            _apartmentMaterialsData = apartmentMaterialsData;
            switch (targetType)
            {
                case UnitType.Apartment:
                    var apartmentConfig = await _assetProvider.Load<CatalogConfig>
                        (AssetAddress.APARTMENT_CATALOG_DATA_PATH);
                    _catalogData = _mainCatalogData = apartmentConfig.CatalogData;
                    _extraCatalogData = apartmentConfig.BathroomCatalogData;
                    break;
                case UnitType.Garden:
                    var gardenConfig = await _assetProvider.Load<CatalogConfig>
                        (AssetAddress.GARDEN_CATALOG_DATA_PATH);
                    _catalogData = gardenConfig.CatalogData;
                    foreach (CatalogAreaUI handler in _areaSwitchers)
                        Destroy(handler.gameObject);
                    break;
            }
            if (_catalogData == null)
                return;
            await LoadAssets();
            SetSections();
            _currentSection = CatalogSection.None;
            SetCamera();
            SwitchSection(_catalogData.First().Key);
        }

        private void SwitchArea(ApartmentAreaType targetArea)
        {
            if (targetArea == ApartmentAreaType.None)
                return;
            switch (targetArea)
            {
                case ApartmentAreaType.Bathroom:
                    _catalogData = _extraCatalogData;
                    break;
                case ApartmentAreaType.Bedroom:
                    _catalogData = _mainCatalogData;
                    break;
            }
            SetSections();
            _currentApartmentArea = targetArea;
            _currentSection = CatalogSection.None;
            SetCamera();
            SwitchSection(_catalogData.First().Key);
        }

        private void SetSections()
        {
            while (_sectionsLayoutGroup.childCount > 0)
                DestroyImmediate(_sectionsLayoutGroup.transform.GetChild(0).gameObject);
            foreach (KeyValuePair<CatalogSection, List<CatalogItemStaticData>> section in _catalogData)
                CreateSection(section.Key);
        }
        private void CreateSection(CatalogSection newSection)
        {
            CatalogSectionUI section = Instantiate(_sectionPrefab, _sectionsLayoutGroup).GetComponent<CatalogSectionUI>();
            section.SetSection(newSection);
            section.PickSection += SwitchSection;
        }


        private void SwitchSection(CatalogSection newSection)
        {
            if (_currentSection == newSection)
                return;
            while (_itemsLayoutGroup.childCount > 0)
                DestroyImmediate(_itemsLayoutGroup.transform.GetChild(0).gameObject);
            foreach (CatalogItemStaticData itemData in _catalogData[newSection])
                AddItemUI(itemData);

            _currentSection = newSection;
        }

        private void SetCamera()
        {
            if (_virtualCameras.Count == 0)
                return;
            UnitVirtualCamera targetCamera = new();
            if (_virtualCameras.Count == 1)
                targetCamera = _virtualCameras[0];
            foreach (UnitVirtualCamera camData in _virtualCameras)
            {
                if (camData.ApartmentAreaType != _currentApartmentArea)
                    continue;
                targetCamera = camData;
            }
            _sceneContext.CinemachineHandler.SetCameraTo(
                    targetCamera.VirtualCamera,
                    targetCamera.IgnoredLayers);
        }

        private void AddItemUI(CatalogItemStaticData itemData)
        {
            CatalogItemUI item = Instantiate(_itemPrefab, _itemsLayoutGroup).
                GetComponent<CatalogItemUI>();
            item.SetItemData(itemData);
            item.TryPurchase += HandlePurchase;
        }


        private async void HandlePurchase(CatalogItemStaticData itemData)
        {
            if (!AbleToPurchase(itemData.Price))
                return;
            _objectInPlacingData = itemData;
            if (itemData.ItemSection == CatalogSection.Walls)
                await ApplyWalls(itemData);
            else if (itemData.ItemSection == CatalogSection.Floors)
                await ApplyFloors(itemData);
            else
                await SpawnObject(itemData);
            SwitchToPlacementUI();
        }

        private async Task ApplyFloors(CatalogItemStaticData itemData)
        {
            if (_apartmentMaterialsData.Count == 0)
                return;
            foreach (ApartmentMaterialParents apartmentMaterials in _apartmentMaterialsData)
            {
                if (apartmentMaterials.AreaType != _currentApartmentArea)
                    continue;
                Material loadedMaterial = await _assetProvider.Load<Material>
                    (itemData.PrefabAssetReference);
                apartmentMaterials.FloorsHandler.TryMaterial(loadedMaterial, itemData.PrefabAssetReference.AssetGUID);
            }
        }

        private async Task ApplyWalls(CatalogItemStaticData itemData)
        {
            if (_apartmentMaterialsData.Count == 0)
                return;
            foreach (ApartmentMaterialParents apartmentMaterials in _apartmentMaterialsData)
            {
                Debug.Log(apartmentMaterials.AreaType);
                if (apartmentMaterials.AreaType != _currentApartmentArea)
                    continue;
                Material loadedMaterial = await _assetProvider.Load<Material>
                    (itemData.PrefabAssetReference);
                apartmentMaterials.WallsHandler.TryMaterial(loadedMaterial, itemData.PrefabAssetReference.AssetGUID);
            }
        }

        private async Task SpawnObject(CatalogItemStaticData itemData)
        {
            _objectInPlacing = await _mansionFactory.CreateUnitObject(
                            reference: itemData.PrefabAssetReference.AssetGUID,
                            at: Vector3.zero,
                            parent: _newItemSpawnPoint);
            _objectInPlacing.transform.localScale = Vector3.one;
            _objectInPlacing.GetComponent<UnitItem>().SetItemData(
                unitID: _unitID,
                assetReference: itemData.PrefabAssetReference.AssetGUID,
                itemSection: _currentSection);
        }

        private bool AbleToPurchase(int price)
        {
            return _economyService.GetEconomyData(ResourceType.SoftVallue) >= price;
        }
        private void RotatePlacement()
        {
            if (_objectInPlacing == null)
                return;
            if (_objectInPlacing.TryGetComponent(out UnitItem itemHandler))
                itemHandler.Rotate();
        }

        private void CancelPlacement()
        {
            if (_objectInPlacingData.ItemSection == CatalogSection.Walls)
                CancelWallPlacement();
            else if (_objectInPlacingData.ItemSection == CatalogSection.Floors)
                CancelFloorPlacement();
            else
                Destroy(_objectInPlacing);
            SwitchToCatalogUI();
        }

        private void CancelFloorPlacement()
        {
            if (_apartmentMaterialsData.Count == 0)
                return;
            foreach (ApartmentMaterialParents apartmentMaterials in _apartmentMaterialsData)
            {
                if (apartmentMaterials.AreaType != _currentApartmentArea)
                    continue;
                apartmentMaterials.FloorsHandler.CancelMaterial();
            }
        }

        private void CancelWallPlacement()
        {
            if (_apartmentMaterialsData.Count == 0)
                return;
            foreach (ApartmentMaterialParents apartmentMaterials in _apartmentMaterialsData)
            {
                if (apartmentMaterials.AreaType != _currentApartmentArea)
                    continue;
                apartmentMaterials.WallsHandler.CancelMaterial();
            }
        }

        private void ApplylPlacement()
        {
            _economyService.SetEconomyData(ResourceType.SoftVallue, -_objectInPlacingData.Price);
            _unitOnBuyEvent?.Invoke(_currentSection);

            if (_objectInPlacingData.ItemSection == CatalogSection.Walls)
                ApplyWallPlacement();
            else if (_objectInPlacingData.ItemSection == CatalogSection.Floors)
                ApplyFloorPlacement();
            else
                ApplyFurniturePlacement();
            
            SwitchToCatalogUI();
        }

        private void ApplyWallPlacement()
        {
            if (_apartmentMaterialsData.Count == 0)
                return;
            foreach (ApartmentMaterialParents apartmentMaterials in _apartmentMaterialsData)
            {
                if (apartmentMaterials.AreaType != _currentApartmentArea)
                    continue;
                apartmentMaterials.WallsHandler.ConfirmMaterial();
            }
        }

        private void ApplyFloorPlacement()
        {
            if (_apartmentMaterialsData.Count == 0)
                return;
            foreach (ApartmentMaterialParents apartmentMaterials in _apartmentMaterialsData)
            {
                if (apartmentMaterials.AreaType != _currentApartmentArea)
                    continue;
                apartmentMaterials.FloorsHandler.ConfirmMaterial();
            }
        }

        private void ApplyFurniturePlacement()
        {
            Destroy(_objectInPlacing.GetComponent<DragAndDrop>());
            _objectInPlacing = null;
        }

        private void SwitchToPlacementUI()
        {
            _catalogUI.SetActive(false);
            _placementUI.SetActive(true);
        }
        private void SwitchToCatalogUI()
        {
            _catalogUI.SetActive(true);
            _placementUI.SetActive(false);
        }

        private void OnDestroy()
        {
            _sceneContext.CinemachineHandler.ResetCamera();
            Unsubscribe();
        }

    }

}
