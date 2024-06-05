using RoyalMasion.Code.Infrastructure.Services.UIFactory;
using VContainer;

namespace RoyalMasion.Code.Infrastructure.StateMachine.States
{
    public class GameLoopState : IState
    {
        private readonly GameStateMachine _stateMachine;
        private readonly IUIFactory _uiFactory;
        private IGameStateMachine _gameStateMachine;

        [Inject]
        public GameLoopState(IUIFactory uiFactory)
        {
            _uiFactory = uiFactory;
        }

        public void SetupStateMachine(IGameStateMachine gameStateMachine)
        {
            _gameStateMachine = gameStateMachine;
        }

        public void Enter()
        {
            SpawnHUD();
        }

        private void SpawnHUD()
        {
            _uiFactory.CreateWindow(Data.WindowID.MansionHUD);
        }

        public void Exit()
        {
            _uiFactory.ClearUIRoot();
        }
    }
}