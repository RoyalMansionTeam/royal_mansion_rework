using RoyalMansion.Code.UnityLogic.NPC;
using RoyalMasion.Code.Infrastructure.Services.AssetProvider;
using RoyalMasion.Code.Infrastructure.Services.ProjectData;
using RoyalMasion.Code.Infrastructure.Services.SaveLoadService;
using RoyalMasion.Code.Infrastructure.Services.SceneContext;
using RoyalMasion.Code.Infrastructure.Services.SceneLoaderService;
using RoyalMasion.Code.Infrastructure.Services.StaticData;
using RoyalMasion.Code.Infrastructure.Services.UIFactory;
using RoyalMasion.Code.Infrastructure.Services.UpdateBehaviorService;
using RoyalMasion.Code.Infrastructure.StateMachine;
using RoyalMasion.Code.Infrastructure.StateMachine.States;
using RoyalMasion.Code.UnityLogic.Coroutines;
using RoyalMasion.Code.UnityLogic.MasionManagement;
using System;
using UnityEngine;
using VContainer;
using VContainer.Unity;
using static RoyalMasion.Code.Infrastructure.Services.SaveLoadService.SaveLoadService;

namespace RoyalMasion.Code.Infrastructure.DI
{
    public class RootLifetimeScope : LifetimeScope
    {
        [SerializeField] private GameObject _updateBehaviourPrefab;
        [SerializeField] private GameObject _sceneLoaderPrefab;
        [SerializeField] private GameObject _gameBootstraperPrefab;
        [SerializeField] private GameObject _eventSystemPrefab;
        [SerializeField] private GameObject _projectTimeTracker;

        private IContainerBuilder _containerBuilder;

        private UpdateBehaviour _updateBehaviour;
        private SceneLoader _sceneLoader;
        private GameBootstrapper _gameBootstrapper;
        private CoroutineRunner _coroutineRunner;
        private ProjectTimeTracker _timeTracker;

        public static GameObject Instance;

        protected override void Configure(IContainerBuilder builder)
        {
            _containerBuilder = builder;
            InstantiateRootObject();
            BindServices();
            BindGameStateMachine();
            BindFactories();
        }

        private void BindFactories()
        {
            _containerBuilder.Register<IUIFactory, UIFactory>(Lifetime.Singleton);
            _containerBuilder.Register<IMansionFactory, MansionFactory>(Lifetime.Singleton);
            _containerBuilder.Register<INpcFactory, NpcFactory>(Lifetime.Singleton);
        }

        private void BindGameStateMachine()
        {
            _containerBuilder.Register<IGameStateMachine, GameStateMachine>(Lifetime.Singleton);
            _containerBuilder.Register<BootstrapState, BootstrapState>(Lifetime.Singleton);
            _containerBuilder.Register<LoadLevelState, LoadLevelState>(Lifetime.Singleton);
            _containerBuilder.Register<GameLoopState, GameLoopState>(Lifetime.Singleton);
        }

        private void BindServices()
        {
            _containerBuilder.Register<IAssetProvider, AssetProvider>(Lifetime.Singleton);
            _containerBuilder.Register<IStaticDataService, StaticDataService>(Lifetime.Singleton);
            _containerBuilder.Register<ISceneContextService, SceneContextService>(Lifetime.Singleton);
            _containerBuilder.Register<ISaveLoadService, SaveLoadService>(Lifetime.Singleton);
            _containerBuilder.Register<IPersistentProgressService, PersistentProgressService>(Lifetime.Singleton);
            _containerBuilder.Register<IEconomyDataService, EconomyDataService>(Lifetime.Singleton);

            _containerBuilder.RegisterInstance<IUpdateBehaviourService>(_updateBehaviour);
            _containerBuilder.RegisterInstance<ISceneLoader>(_sceneLoader);
            _containerBuilder.RegisterInstance(_gameBootstrapper);
            _containerBuilder.RegisterInstance<ICoroutineRunner>(_coroutineRunner);
            _containerBuilder.RegisterInstance(_timeTracker);
        }

        private void InstantiateRootObject()
        {
            Instance = new GameObject("RootLifetimeScope");
            DontDestroyOnLoad(Instance);
            InstantiateServices();
        }

        private void InstantiateServices()
        {
            GameObject updateBehaviourServise = Instantiate(_updateBehaviourPrefab);
            updateBehaviourServise.transform.SetParent(Instance.transform, false);
            _updateBehaviour = updateBehaviourServise.GetComponent<UpdateBehaviour>();

            GameObject sceneLoaderServise = Instantiate(_sceneLoaderPrefab);
            sceneLoaderServise.transform.SetParent(Instance.transform, false);
            _sceneLoader = sceneLoaderServise.GetComponent<SceneLoader>();

            GameObject bootstrapperInstance = Instantiate(_gameBootstraperPrefab);
            bootstrapperInstance.transform.SetParent(Instance.transform, false);
            _gameBootstrapper = bootstrapperInstance.GetComponent<GameBootstrapper>();

            GameObject projectTimeTracker = Instantiate(_projectTimeTracker);
            projectTimeTracker.transform.SetParent(Instance.transform, false);
            _timeTracker = projectTimeTracker.GetComponent<ProjectTimeTracker>();

            GameObject eventSystem = Instantiate(_eventSystemPrefab);
            eventSystem.transform.SetParent(Instance.transform, false);

            _coroutineRunner = new GameObject("CoroutineRunner").AddComponent<CoroutineRunner>();
            _coroutineRunner.transform.SetParent(Instance.transform, false);
        }
    }

}
