using RoyalMansion.Code.Extensions.Utils;
using RoyalMansion.Code.UnityLogic.NPC;
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
        private MaidNPC _assignedMaid;

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
            if (_assignedMaid == null)
            {
                TryGetMaid();
                return;
            }
            if (_timer != null)
                Debug.Log("Offer to boost timer");
        }
        private void TryGetMaid()
        {
            _assignedMaid = _stateMachineData.SceneContext.MaidService.TryAssignMaid
                (_stateMachineData.NavMeshTarget);
            if (_assignedMaid == null)
                _stateMachineData.SceneContext.MaidService.AvailableMaidFound +=
                    OnMaidFound;
            else
                OnMaidFound(_assignedMaid);
        }

        private void OnMaidFound(MaidNPC maid)
        {
            _assignedMaid = maid;
            _assignedMaid.UnitAchived += SpawnTimer;
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
            _stateMachineData.SceneContext.MaidService.SendMaidBack(_assignedMaid);
            _mansionStateMachine.Enter<EmptyState>();
        }

        public void Exit()
        {
            _stateMachineData.SceneContext.MaidService.AvailableMaidFound -=
                OnMaidFound;
            _assignedMaid.UnitAchived -= SpawnTimer;
            _assignedMaid = null;
            _timer.Despawn();
            _timer = null;
        }
    }
}
