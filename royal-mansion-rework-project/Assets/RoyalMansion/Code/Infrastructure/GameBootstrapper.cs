using RoyalMasion.Code.Infrastructure.StateMachine;
using RoyalMasion.Code.Infrastructure.StateMachine.States;
using UnityEngine;
using VContainer;

namespace RoyalMasion.Code.Infrastructure
{
    public class GameBootstrapper : MonoBehaviour
    {
        private IGameStateMachine _gameStateMachine;

        [Inject]
        public void Construct(IGameStateMachine gameStateMachine)
        {
            _gameStateMachine = gameStateMachine;
            Init();
        }

        private void Init()
        {
            _gameStateMachine.Enter<BootstrapState>();
            DontDestroyOnLoad(gameObject);
        }
    }
}