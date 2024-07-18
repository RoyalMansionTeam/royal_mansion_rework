using RoyalMansion.Code.UnityLogic.CameraLogic;
using RoyalMasion.Code.Extensions;
using RoyalMasion.Code.Infrastructure.Data;
using RoyalMasion.Code.Infrastructure.Services.StaticData;
using RoyalMasion.Code.Infrastructure.Services.UIFactory;
using RoyalMasion.Code.Infrastructure.StaticData;
using RoyalMasion.Code.UnityLogic.MasionManagement;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

namespace RoyalMansion.Code.UI.WorldspaceUI
{
    public class MansionUnitUIHandler : MonoBehaviour
    {
        [SerializeField] private Transform _unitBody;

        private IMansionFactory _mansionFactory;
        private IStaticDataService _staticDataService;
        private IUIFactory _uiFactory;

        private RectTransform _rectTransform;
        private Vector3 _anchoredPoint;
        private GameObject _activeUI;

        [Inject]
        public void Construct(IMansionFactory mansionFactory, IStaticDataService staticDataService, IUIFactory uiFactory)
        {
            _mansionFactory = mansionFactory;
            _staticDataService = staticDataService;
            _uiFactory = uiFactory;
        }

        public void SetHandlerData(Vector3 anchorPoint, DraggableCamera draggableCamera)
        {
            _rectTransform = (RectTransform)transform;
            _anchoredPoint = anchorPoint;
            draggableCamera.CameraPositionChanged += RefreshPosition;
            RefreshPosition();
            UpdateLayoutGroup();
        }


        public async Task<GameObject> SetUIMessenge(InternalUnitStates unitState)
        {
            UpdateUI();
            UnitUIConfig reference = LoadAssetReference(unitState);
            if (reference == null)
                return null;
            _activeUI = await _mansionFactory.CreateUnitObject
                    (reference.PrefabReference.AssetGUID, Vector3.zero, _unitBody);
            return _activeUI;
        }


        public async Task<Timer> SetUnitTimer(InternalUnitStates unitState)
        {

            UpdateUI();
            UnitUIConfig reference = LoadAssetReference(unitState);
            if (reference == null)
                return null;
            Timer instance = await _mansionFactory.CreateTimer
                    (reference.PrefabReference.AssetGUID, Vector3.zero, _unitBody);
            _activeUI = instance.gameObject;
            return instance;
        }
        private void UpdateUI()
        {
            if (_unitBody != null)
                _unitBody.gameObject.SetActive(true);
            if (_activeUI != null)
                Destroy(_activeUI);
        }
        private void RefreshPosition() => 
            _rectTransform.anchoredPosition = _uiFactory.UICanvas.WorldToCanvasPosition(_anchoredPoint);
        private void UpdateLayoutGroup() =>
            transform.GetChild(0).GetComponent<VerticalLayoutGroup>().CalculateLayoutInputVertical();


        private UnitUIConfig LoadAssetReference(InternalUnitStates unitState)
        {
            if (_staticDataService.GameData == null)
                return null;
            foreach (UnitUIConfig uiPrefab in _staticDataService.GameData.MansionStaticData.UnitUIConfigs)
            {
                if (uiPrefab.UIState != unitState)
                    continue;
                return uiPrefab;
            }
            return null;
        }

        public void Clear()
        {
            if (_activeUI != null)
                Destroy(_activeUI);
            _unitBody.gameObject.SetActive(false);
        }
    }
}
