using RoyalMasion.Code.Infrastructure.Data;
using RoyalMasion.Code.Infrastructure.Services.StaticData;
using RoyalMasion.Code.Infrastructure.StaticData;
using RoyalMasion.Code.UnityLogic.MasionManagement;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using VContainer;

namespace RoyalMansion.Code.UI.WorldspaceUI
{
    public class MansionUnitUIHandler : MonoBehaviour
    {
        [SerializeField] private Transform _unitBody;

        private IMansionFactory _mansionFactory;
        private IStaticDataService _staticDataService;

        private GameObject _activeUI;

        [Inject]
        public void Construct(IMansionFactory mansionFactory, IStaticDataService staticDataService)
        {
            _mansionFactory = mansionFactory;
            _staticDataService = staticDataService;
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
