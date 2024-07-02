using RoyalMansion.Code.UnityLogic.NPC;
using RoyalMasion.Code.Infrastructure.Data;
using RoyalMasion.Code.Infrastructure.Services.SaveLoadService;
using RoyalMasion.Code.Infrastructure.Services.SceneContext;
using RoyalMasion.Code.Infrastructure.Services.SceneLoaderService;
using RoyalMasion.Code.Infrastructure.Services.StaticData;
using RoyalMasion.Code.Infrastructure.Services.UIFactory;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using VContainer;

namespace RoyalMasion.Code.Infrastructure.StateMachine.States
{
    public class LoadLevelState : IPayloadedState<string>, IPayloadedState<SceneID>
    {
        private readonly ISceneLoader _sceneLoader;
        private readonly IUIFactory _uiFactory;
        private readonly INpcFactory _npcFactory;
        private readonly ISaveLoadService _saveLoadService;
        private IGameStateMachine _gameStateMachine;

        [Inject]
        public LoadLevelState(ISceneLoader sceneLoader,
            IUIFactory uiFactory, INpcFactory npcFactory,
            ISaveLoadService saveLoadService)
        {
            _sceneLoader = sceneLoader;
            _uiFactory = uiFactory;
            _npcFactory = npcFactory;
            _saveLoadService = saveLoadService;
        }

        public void SetupStateMachine(IGameStateMachine gameStateMachine)
        {
            _gameStateMachine = gameStateMachine;
        }

        public void Enter(SceneID levelID)
        {
            InitNPCFactory();
            _sceneLoader.LoadScene(levelID, OnLoaded);
        }

        public void Enter(string levelID)
        {
            InitNPCFactory();
            var sceneIndex = SceneManager.GetSceneByName(levelID).buildIndex;
            _sceneLoader.LoadScene(sceneIndex, OnLoaded);
        }

        private async void OnLoaded()
        {
            _uiFactory.ClearUIRoot();
            SpawnPlayer();
            //await InitNPCFactory();
            ApplyProgressOnLevel();

            _gameStateMachine.Enter<GameLoopState>();
        }

        private void ApplyProgressOnLevel() => 
            _saveLoadService.ApplyProgress();

        private async Task InitNPCFactory() =>
            await _npcFactory.SetNpcFactory();

        private void SpawnPlayer()
        {

        }

        public void Exit()
        {
        }

    }
}