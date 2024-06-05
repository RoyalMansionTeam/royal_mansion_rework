using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace RoyalMasion.Code.UnityLogic.MasionManagement
{
    public interface IMansionFactory
    {
        Task<GameObject> CreateTimer(Vector3 at, Transform parent);
        Task<GameObject> CreateUnitObject(AssetReference reference, Vector3 at, Transform parent);
    }
}
