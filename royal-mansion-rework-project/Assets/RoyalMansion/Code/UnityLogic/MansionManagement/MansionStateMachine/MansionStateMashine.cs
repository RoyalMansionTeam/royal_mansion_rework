using RoyalMasion.Code.Infrastructure.StateMachine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RoyalMasion.Code.UnityLogic.MasionManagement.MansionStateMachine.States;
using VContainer;
using RoyalMasion.Code.Infrastructure.Services.ProjectData;
using RoyalMansion.Code.Extensions.Utils;
using RoyalMasion.Code.UnityLogic.MasionManagement.ApartmentLogic;
using RoyalMasion.Code.Infrastructure.Data;
using RoyalMansion.Code.UnityLogic.NPC;
using RoyalMasion.Code.Infrastructure.Services.SceneContext;
using RoyalMasion.Code.Infrastructure.Services.UIFactory;

namespace RoyalMasion.Code.UnityLogic.MasionManagement.MansionStateMachine
{
    public class MansionStateMashine : IMansionStateMashine
    {
        public IExitableMansionState ActiveState { get; private set; }
        public GuestNPC NPC { get; set; }
        public Action<IExitableMansionState> StateChanged { get; set; }

        private readonly Dictionary<Type, IExitableMansionState> _states;
        private readonly MansionStateMachineData _stateMachineData;


        public MansionStateMashine(MansionStateMachineData stateMachineData)
        {
            _states = new Dictionary<Type, IExitableMansionState>
            {
                [typeof(LockedState)] = new LockedState(),
                [typeof(EmptyState)] = new EmptyState(),
                [typeof(InUseState)] = new InUseState(),
                [typeof(CollectableState)] = new CollectableState(),
                [typeof(DirtyState)] = new DirtyState()
            };
            _stateMachineData = stateMachineData;
            foreach (var state in _states.Values)
            {
                state.SetupStateMachine(this);
            }
        }

        public void Enter<TState>() where TState : class, IMansionState
        {
            IMansionState state = ChangeState<TState>();
            StateChanged?.Invoke(_states[typeof(TState)]);
            state.Enter();
        }

        public void Enter<TState, TPayload>(TPayload payload) where TState : class, IMansionPayloadedState<TPayload>
        {
            var state = ChangeState<TState>();
            StateChanged?.Invoke(_states[typeof(TState)]);
            state.Enter(payload);
        }
        public void Stay()
        {
            ActiveState.Stay();
        }
        public MansionStateMachineData GetStateMachineData() => 
            _stateMachineData;
        public UnitState GetUnitStateEnum(Type type) => 
            MansionUtils.StateCorrelation[type];
        private TState ChangeState<TState>() where TState : class, IExitableMansionState
        {
            ActiveState?.Exit();

            var state = GetState<TState>();
            ActiveState = state;

            return state;
        }
          
        private TState GetState<TState>() where TState : class, IExitableMansionState
        {
            return _states[typeof(TState)] as TState;
        }
        public void CleanUp()
        {
            ActiveState = null;
            _states.Clear();
        }

    }

    public class MansionStateMachineData
    {
        public ISceneContextService SceneContext { get; }

        public IMansionFactory MansionFactory { get; }
        public IEconomyDataService EconomyData { get; }
        public INpcFactory NpcFactory { get; }
        public TimerSpawnData TimerSpawn { get; }
        public UnitStaticData UnitData { get; }
        public IUIFactory UiFactory { get; }
        public Transform ItemSpawnPoint { get; }

        public MansionStateMachineData(IMansionFactory mansionFactory, IEconomyDataService economyDataService,
            INpcFactory npcFactory, TimerSpawnData timerSpawnData, UnitStaticData unitStaticData,
            ISceneContextService sceneContext, IUIFactory uiFactory, Transform itemSpawnPoint)
        {
            MansionFactory = mansionFactory;
            EconomyData = economyDataService;
            NpcFactory = npcFactory;
            TimerSpawn = timerSpawnData;
            UnitData = unitStaticData;
            SceneContext = sceneContext;
            UiFactory = uiFactory;
            ItemSpawnPoint = itemSpawnPoint;
        }

    }
}
