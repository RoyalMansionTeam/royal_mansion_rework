using RoyalMasion.Code.Infrastructure.Data;
using RoyalMasion.Code.Infrastructure.Services.ProjectData;
using RoyalMasion.Code.Infrastructure.Services.SaveLoadService;
using RoyalMasion.Code.Infrastructure.Services.StaticData;
using RoyalMasion.Code.Infrastructure.Services.UIFactory;
using System.Threading.Tasks;
using UnityEngine.SceneManagement;
using VContainer;

namespace RoyalMasion.Code.Infrastructure.StateMachine.States
{
    public class BootstrapState : IState
    {
        private readonly IEconomyDataService _economyData;
        private readonly IUIFactory _uiFactory;
        private readonly IStaticDataService _staticDataService;
        private readonly ISaveLoadService _saveLoadService;
        private readonly IPersistentProgressService _progressService;
        private IGameStateMachine _gameStateMachine;

        [Inject]
        public BootstrapState(IUIFactory uiFactory,
            IStaticDataService staticDataService, IEconomyDataService economyData, 
            ISaveLoadService saveLoadService, IPersistentProgressService progressService)
        {
            _staticDataService = staticDataService;
            _uiFactory = uiFactory;
            _economyData = economyData;
            _saveLoadService = saveLoadService;
            _progressService = progressService;
        }

        public void SetupStateMachine(IGameStateMachine gameStateMachine)
        {
            _gameStateMachine = gameStateMachine;
        }

        public async void Enter()
        {
            LoadProgressOrInitNew();
            InitEconomyData();
            await LoadServiceData();
            OnLoaded();
        }
        private void LoadProgressOrInitNew()
        {
            if (_saveLoadService.LoadProgress() == null)
                _progressService.Progress = NewProgress();
            else
                _progressService.Progress = _saveLoadService.LoadProgress();
        }

        private void InitEconomyData() => 
            _economyData.InitEconomyService();


        private GameProgress NewProgress()
        {
            var progress = new GameProgress();
            return progress;
        }

        private async Task LoadServiceData()
        {
            await _staticDataService.Load();
            _uiFactory.CreateUiRoot();
        }

        private void OnLoaded()
        {
            //Editor stuff, needed to have ability to start playmode at any scene, not only from bootstrap scene
#if UNITY_EDITOR
            if (SceneManager.GetActiveScene().name == SceneID.BootstrapScene.ToString())
                _gameStateMachine.Enter<LoadLevelState, SceneID>(SceneID.MansionScene);
            else
                _gameStateMachine.Enter<LoadLevelState, string>(SceneManager.GetActiveScene().name);

#else
  _gameStateMachine.Enter<LoadLevelState, SceneID>(SceneID.GamePlayScene);
#endif
        }

        public void Exit()
        {
        }
    }
}