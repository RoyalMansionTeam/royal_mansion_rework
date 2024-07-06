using RoyalMasion.Code.Infrastructure.Data;
using UnityEngine.AddressableAssets;

namespace RoyalMasion.Code.Infrastructure.StaticData
{
    [System.Serializable]
    public class UnitUIConfig
    {
        public InternalUnitStates UIState;
        public AssetReference PrefabReference;
    }
}