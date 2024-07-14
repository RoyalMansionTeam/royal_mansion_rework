using RoyalMasion.Code.Extensions;
using RoyalMasion.Code.Infrastructure.Saving;
using RoyalMasion.Code.Infrastructure.Services.AssetProvider;
using RoyalMasion.Code.Infrastructure.Services.SaveLoadService;
using System;
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
        private readonly IPersistentProgressService _progressService;

        public List<ISaveReader> ProgressReaders { get; } = new();
        public List<ISaveWriter> ProgressWriters { get; } = new();

        [Inject]
        public MansionFactory(IAssetProvider assetProvider, IPersistentProgressService progressService)
        {
            _assetProvider = assetProvider;
            _progressService = progressService;
        }

        public async Task<Timer> CreateTimer(string reference, Vector3 at, Transform parent)
        {
            GameObject timerObj = await InstatiateRegistered(reference, at, parent);
            Timer timer = timerObj.GetComponent<Timer>();
            timer.InitProgressService(_progressService);  
            return timer;
        }

        public async Task<GameObject> CreateUnitObject(string reference, Vector3 at, Transform parent) => 
            await InstatiateRegistered(reference, at, parent);

        public void Cleanup()
        {
            ProgressReaders.Clear();
            ProgressWriters.Clear();
        }
        public async void TryLoadMansionEnteties(GameProgress progress, string unitID)
        {
            foreach(CatalogItemSaveData catalogItem in progress.MansionProgress.CatalogItemsSave)
            {
                if (catalogItem.AssignedUnitID != unitID)
                    continue;
                await CreateUnitObject(
                    catalogItem.AssetGUID,
                    catalogItem.Position.AsUnityVector(),
                    null);
            }
        }
        public void RegisterSaveableEntity(ISaveReader reader)
        {
            Register(reader);
        }

        private async Task<GameObject> InstatiateRegistered(string prefabPath, Vector3 at, Transform parent)
        {
            GameObject instance = await _assetProvider.Instantiate(
                path: prefabPath,
                at: at);
            instance.transform.SetParent(parent, false);
            RegisterProgressWatchers(instance);
            return instance;
        }
        private void RegisterProgressWatchers(GameObject gameObject)
        {
            foreach (ISaveReader progressReader in gameObject.GetComponentsInChildren<ISaveReader>())
                Register(progressReader);
        }
        private void Register(ISaveReader progressReader)
        {
            if (progressReader is ISaveWriter progressWriter)
                ProgressWriters.Add(progressWriter);
            ProgressReaders.Add(progressReader);
        }

    }
}
