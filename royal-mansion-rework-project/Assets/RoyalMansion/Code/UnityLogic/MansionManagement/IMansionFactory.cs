using RoyalMasion.Code.Infrastructure.Services.SaveLoadService;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace RoyalMasion.Code.UnityLogic.MasionManagement
{
    public interface IMansionFactory
    {
        List<ISaveReader> ProgressReaders { get; }
        List<ISaveWriter> ProgressWriters { get; }
        Task<Timer> CreateTimer(string reference, Vector3 at, Transform parent);
        Task<GameObject> CreateUnitObject(string reference, Vector3 at, Transform parent);
        void RegisterSaveableEntity(ISaveReader reader);
        void Cleanup();
    }
}
