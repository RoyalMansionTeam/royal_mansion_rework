using RoyalMansion.Code.UI.WorldspaceUI;
using RoyalMansion.Code.UnityLogic.CameraLogic;
using RoyalMasion.Code.Infrastructure.Data;
using RoyalMasion.Code.Infrastructure.Services.ProjectData;
using RoyalMasion.Code.Infrastructure.Services.SceneContext;
using RoyalMasion.Code.Infrastructure.Services.StaticData;
using RoyalMasion.Code.Infrastructure.Services.UIFactory;
using System;
using UnityEngine;
using VContainer;

namespace RoyalMasion.Code.UnityLogic.MasionManagement.KitchenGardenLogic
{
    public class Pantry : MonoBehaviour
    {
        private const ResourceType pantry_resource_type = ResourceType.Fruit;

        [SerializeField] private Transform _uiHandlerAnchor;

        private IUIFactory _uiFactory;
        private ISceneContextService _sceneContext;
        private IEconomyDataService _economyData;
        private IStaticDataService _statiDataService;

        private MansionUnitUIHandler _uiHandler;
        private TextUIHandler _textHandler;

        private int _pantryCapacity;


        [Inject]
        public void Construct(IUIFactory uiFactory, ISceneContextService sceneContext,
            IEconomyDataService economyData, IStaticDataService staticDataService)
        {
            _uiFactory = uiFactory;
            _sceneContext = sceneContext;
            _economyData = economyData;
            _statiDataService = staticDataService;

            InitPantryData();
            InitUI();
            Subscribe();
        }

        private void InitPantryData()
        {
            _pantryCapacity = _statiDataService.GameData.PlaytestStaticData.PantryCapacity;
        }

        private async void InitUI()
        {
            _uiHandler = _uiFactory.CreateUnitUIHandler();
            _uiHandler.SetHandlerData(_uiHandlerAnchor.position, 
                _sceneContext.CinemachineHandler.MainCamera.GetComponent<DraggableCamera>());
            GameObject pantryUI = await _uiHandler.SetUIMessenge(InternalUnitStates.Pantry);
            _textHandler = pantryUI.GetComponent<TextUIHandler>();
            SetResourcesUI(pantry_resource_type, _economyData.GetEconomyData(pantry_resource_type));
        }

        private void Subscribe()
        {
            _economyData.ResourceChanged += SetResourcesUI;
        }

        private void Unsubscribe()
        {
            _economyData.ResourceChanged -= SetResourcesUI;
        }

        private void SetResourcesUI(ResourceType type, int currentAmount)
        {
            if (type != pantry_resource_type)
                return;
            Debug.Log(currentAmount / (float)_pantryCapacity);
            string percentMessenge = (currentAmount / (float)_pantryCapacity * 100).ToString();
            _textHandler.SetTextField(percentMessenge);
        }

        private void OnDestroy()
        {
            Unsubscribe();
        }

    }
}
