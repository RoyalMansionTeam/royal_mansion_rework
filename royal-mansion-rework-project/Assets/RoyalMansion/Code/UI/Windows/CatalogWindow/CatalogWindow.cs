using RoyalMansion.Code.UnityLogic.Catalog;
using RoyalMasion.Code.Infrastructure.Data;
using RoyalMasion.Code.Infrastructure.Services.AssetProvider;
using RoyalMasion.Code.Infrastructure.Services.ProjectData;
using RoyalMasion.Code.UI.Windows;
using RoyalMasion.Code.UnityLogic.MasionManagement;
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

        [SerializeField] private List<CatalogAreaUI> _areaSwitchers;

        private IAssetProvider _assetProvider;
        private IMansionFactory _mansionFactory;
        private IEconomyDataService _economyService;

        private Dictionary<CatalogSection, List<CatalogItemStaticData>> _catalogData;
        private Dictionary<CatalogSection, List<CatalogItemStaticData>> _mainCatalogData;
        private Dictionary<CatalogSection, List<CatalogItemStaticData>> _extraCatalogData;
        private GameObject _sectionPrefab;
        private GameObject _itemPrefab;
        private CatalogSection _currentSection;
        private Transform _newItemSpawnPoint;

        private GameObject _objectInPlacing;
        private CatalogItemStaticData _objectInPlacingData;


        [Inject]
        public void Construct(IAssetProvider assetProvider, IMansionFactory mansionFactory,
            IEconomyDataService economyService)
        {
            _assetProvider = assetProvider;
            _mansionFactory = mansionFactory;
            _economyService = economyService;
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

        public async void SetUnitType(UnitType targetType, Transform spawnPoint)
        {
            _newItemSpawnPoint = spawnPoint;
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
            _currentSection = CatalogSection.None;
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
            _objectInPlacing = await _mansionFactory.CreateUnitObject(reference: itemData.PrefabAssetReference,
                at: _newItemSpawnPoint.position,
                parent: _newItemSpawnPoint);
            _objectInPlacing.transform.localScale = Vector3.one;
            SwitchToPlacementUI();
        }


        private bool AbleToPurchase(int price)
        {
            return _economyService.GetEconomyData(ResourceType.SoftVallue) >= price;
        }

        private void CancelPlacement()
        {
            Destroy(_objectInPlacing);
            SwitchToCatalogUI();
        }

        private void ApplylPlacement()
        {
            Destroy(_objectInPlacing.GetComponent<DragAndDrop>());
            _economyService.SetEconomyData(ResourceType.SoftVallue, -_objectInPlacingData.Price);
            SwitchToCatalogUI();
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
            Unsubscribe();
        }

    }

}
