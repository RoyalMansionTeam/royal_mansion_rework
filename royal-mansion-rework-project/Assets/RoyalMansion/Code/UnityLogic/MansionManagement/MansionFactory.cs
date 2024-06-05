using RoyalMasion.Code.Infrastructure.Services.AssetProvider;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using VContainer;

namespace RoyalMasion.Code.UnityLogic.MasionManagement
{
    public class MansionFactory : IMansionFactory
    {
        private readonly IAssetProvider _assetProvider;

        [Inject]
        public MansionFactory(IAssetProvider assetProvider)
        {
            _assetProvider = assetProvider;
        }
        public async Task<GameObject> CreateTimer(Vector3 at, Transform parent)
        {
            GameObject instance = await _assetProvider.Instantiate(
                path: AssetAddress.UNIT_TIMER_PATH, 
                at: at);
            instance.transform.SetParent(parent);
            return instance;
        }

        public async Task<GameObject> CreateUnitObject(AssetReference reference, Vector3 at, Transform parent)
        {
            GameObject instance = await _assetProvider.Instantiate(reference.AssetGUID, at);
            instance.transform.SetParent(parent);
            return instance;
        }
    }
}
