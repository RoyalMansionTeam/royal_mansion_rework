using RoyalMasion.Code.Infrastructure.Services.AssetProvider;
using RoyalMasion.Code.Infrastructure.Services.SaveLoadService;
using RoyalMasion.Code.Infrastructure.Services.SceneContext;
using RoyalMasion.Code.UnityLogic.MasionManagement;
using RoyalMasion.Code.UnityLogic.MasionManagement.ApartmentLogic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using VContainer;

namespace RoyalMansion.Code.UnityLogic.NPC
{
    public class NpcFactory : INpcFactory
    {
        private readonly IAssetProvider _assetProvider;
        private readonly ISceneContextService _sceneContext;
        private readonly IPersistentProgressService _progressService;
        private NpcPrefabData _prefabData;
        private Dictionary<Type, List<GameObject>> _prefabs;
        private IMansionFactory _mansionFactory;

        [Inject]
        public NpcFactory(IAssetProvider assetProvider, ISceneContextService sceneContext,
            IMansionFactory mansionFactory, IPersistentProgressService progressService)
        {
            _assetProvider = assetProvider;
            _sceneContext = sceneContext;
            _mansionFactory = mansionFactory;
            _progressService = progressService;
        }

        public async Task SetNpcFactory() 
        {
            _prefabData = await _assetProvider.Load<NpcPrefabData>
                (address: AssetAddress.NPC_DATA_PATH);
            _prefabs = _prefabData.Prefabs;
            Debug.Log("LoadAssets");

        }
        public TNpc SpawnNpc<TNpc>() where TNpc : NpcBase
        {
            GameObject instance = UnityEngine.Object.Instantiate(_prefabs[typeof(TNpc)]
                [UnityEngine.Random.Range(0, _prefabs.Count-1)],
                _sceneContext.MansionSpawnPoints.GuestSpawnPoint);
            instance.transform.position = _sceneContext.MansionSpawnPoints.GuestSpawnPoint.position;
            TNpc npcComponent = instance.GetComponent<TNpc>();
            npcComponent.SetProgress(_progressService);
            _mansionFactory.RegisterSaveableEntity(npcComponent);
            return npcComponent;
        }

        public TNpc SpawnNpc<TNpc>(Transform at) where TNpc : NpcBase
        {
            GameObject instance = UnityEngine.Object.Instantiate(_prefabs[typeof(TNpc)]
                [UnityEngine.Random.Range(0, _prefabs.Count - 1)],
                at);
            instance.transform.position = _sceneContext.MansionSpawnPoints.GuestSpawnPoint.position;
            TNpc npcComponent = instance.GetComponent<TNpc>();
            npcComponent.SetProgress(_progressService);
            _mansionFactory.RegisterSaveableEntity(npcComponent);
            return npcComponent;
        }

        public void Clear() 
        {
            _prefabs.Clear();
        }

    }
}
