using RoyalMansion.Code.Extensions.Utils;
using RoyalMasion.Code.UnityLogic.MasionManagement.ApartmentLogic;
using System;
using UnityEngine;

namespace RoyalMasion.Code.UnityLogic.MasionManagement.MansionStateMachine.States
{
    public class DirtyState : IMansionState
    {
        private IMansionStateMashine _mansionStateMachine;
        private MansionStateMachineData _stateMachineData;
        private IMansionFactory _mansionFactory;
        private Timer _timer;
        private UnitTaskData _stateRewardData;

        public void SetupStateMachine(IMansionStateMashine gameStateMachine)
        {
            _mansionStateMachine = gameStateMachine;

        }
        public void Enter()
        {
            _stateMachineData = _mansionStateMachine.GetStateMachineData();
            _mansionFactory = _stateMachineData.MansionFactory;

            _stateRewardData = _stateMachineData.UnitData.GetTaskData
                (_mansionStateMachine.GetUnitStateEnum(GetType()));
        }


        public void Stay()
        {
            if (_timer != null)
                Debug.Log("Offer to boost timer");
            else
                SpawnTimer(_stateMachineData.TimerSpawn);
        }

        private async void SpawnTimer(TimerSpawnData spawnData)
        {
            GameObject timerObj = await _mansionFactory.CreateTimer(spawnData.At, spawnData.Parent);
            timerObj.transform.localScale = new Vector3(.5f, .5f, .5f);
            _timer = timerObj.GetComponent<Timer>();
            _timer.InitTimer(_stateRewardData.Time.ToFloat());
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
