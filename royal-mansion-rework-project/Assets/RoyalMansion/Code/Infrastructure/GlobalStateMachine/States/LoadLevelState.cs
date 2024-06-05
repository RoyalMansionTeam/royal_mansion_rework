using RoyalMansion.Code.UnityLogic.NPC;
using RoyalMasion.Code.Infrastructure.Data;
using RoyalMasion.Code.Infrastructure.Services.SceneContext;
using RoyalMasion.Code.Infrastructure.Services.SceneLoaderService;
using RoyalMasion.Code.Infrastructure.Services.StaticData;
using RoyalMasion.Code.Infrastructure.Services.UIFactory;
using UnityEngine.SceneManagement;
using VContainer;

namespace RoyalMasion.Code.Infrastructure.StateMachine.States
{
    public class LoadLevelState : IPayloadedState<string>, IPayloadedState<SceneID>
    {
        private readonly ISceneLoader _sceneLoader;
        private readonly IStaticDataService _staticDataService;
        private readonly ISceneContextService _sceneContext;
        private readonly IUIFactory _uiFactory;
        private readonly INpcFactory _npcFactory;
        private IGameStateMachine _gameStateMachine;

        [Inject]
        public LoadLevelState(ISceneLoader sceneLoader,
            IStaticDataService staticDataService, ISceneContextService sceneContext, 
            IUIFactory uiFactory, INpcFactory npcFactory)
        {
            _sceneLoader = sceneLoader;
            _staticDataService = staticDataService;
            _sceneContext = sceneContext;
            _uiFactory = uiFactory;
            _npcFactory = npcFactory;
        }

        public void SetupStateMachine(IGameStateMachine gameStateMachine)
        {
            _gameStateMachine = gameStateMachine;
        }

        public void Enter(SceneID levelID)
        {
            _sceneLoader.LoadScene(levelID, OnLoaded);
        }

        public void Enter(string levelID)
        {
            var sceneIndex = SceneManager.GetSceneByName(levelID).buildIndex;
            _sceneLoader.LoadScene(sceneIndex, OnLoaded);
        }

        private void OnLoaded()
        {
            _uiFactory.ClearUIRoot();
            SpawnPlayer();
            InitNPCFactory();

            _gameStateMachine.Enter<GameLoopState>();
        }

        private void InitNPCFactory()
        {
            _npcFactory.SetNpcFactory();
        }

        private void SpawnPlayer()
        {

        }

        public void Exit()
        {
        }

    }
}