using RoyalMansion.Code.Extensions.Utils;
using RoyalMasion.Code.Infrastructure.Data;
using RoyalMasion.Code.UnityLogic.MasionManagement.ApartmentLogic;
using System;
using UnityEngine;

namespace RoyalMasion.Code.UnityLogic.MasionManagement.MansionStateMachine.States
{
    public class DirtyState : IMansionState
    {
        private IMansionStateMachine _mansionStateMachine;
        private MansionStateMachineData _stateMachineData;
        private Timer _timer;
        private UnitTaskData _stateRewardData;

        public void SetupStateMachine(IMansionStateMachine gameStateMachine)
        {
            _mansionStateMachine = gameStateMachine;

        }
        public void Enter()
        {
            _stateMachineData = _mansionStateMachine.GetStateMachineData();

            _stateRewardData = _stateMachineData.UnitData.GetTaskData
                (_mansionStateMachine.GetUnitStateEnum(GetType()));
            SetUI();
        }

        private async void SetUI() =>
            await _mansionStateMachine.GetStateMachineData().UnitUIHandler.
            SetUIMessenge(InternalUnitStates.AwaitingCleaning);

        public void Stay()
        {
            if (_timer != null)
                Debug.Log("Offer to boost timer");
            else
                SpawnTimer();
        }

        private async void SpawnTimer()
        {
            _timer = await _stateMachineData.UnitUIHandler.SetUnitTimer(InternalUnitStates.CleaningTimer);
            _timer.InitTimer(
                taskTime: _stateRewardData.Time.ToFloat(),
                UnitID: _stateMachineData.UnitData.UnitID);
            _timer.TimerDone += OnTimerDone;
        }
        private void OnTimerDone()
        {
            
            _mansionStateMachine.Enter<EmptyState>();
        }

        public void Exit()
        {
            _timer.Despawn();
            _timer = null;
        }
    }
}
