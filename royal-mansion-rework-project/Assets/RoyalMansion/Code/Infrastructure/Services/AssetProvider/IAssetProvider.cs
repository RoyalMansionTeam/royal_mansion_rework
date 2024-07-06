using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace RoyalMasion.Code.Infrastructure.Services.AssetProvider
{
    public interface IAssetProvider
    {
        Task<GameObject> Instantiate(string path, Vector3 at);
        Task<GameObject> Instantiate(string path);
        Task<T> Load<T>(AssetReference dataPrefabReference) where T : class;
        Task<T> Load<T>(string address) where T : class;
        void Initialize();
        void Cleanup();
    }
}