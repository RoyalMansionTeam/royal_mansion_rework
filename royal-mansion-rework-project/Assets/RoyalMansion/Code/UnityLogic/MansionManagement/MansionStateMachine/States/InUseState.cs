using RoyalMansion.Code.Extensions.Utils;
using RoyalMasion.Code.Infrastructure.StateMachine;
using UnityEngine;
using VContainer;
using RoyalMasion.Code.Infrastructure.Data;
using RoyalMasion.Code.UnityLogic.MasionManagement.ApartmentLogic;
using System;

namespace RoyalMasion.Code.UnityLogic.MasionManagement.MansionStateMachine.States
{
    public class InUseState : IMansionState
    {
        private MansionStateMachineData _stateMachineData;
        private IMansionFactory _mansionFactory;
        private IMansionStateMashine _mansionStateMachine;

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
            SpawnTimer(_stateMachineData.TimerSpawn);

        }

        public void Stay()
        {
            Debug.Log("Offer to boost timer");
        }
        public void Exit()
        {
            _timer.Despawn();
            if (_stateMachineData.UnitData.UnitType == UnitType.Apartment)
                EndNPCStay();
        }

        private void EndNPCStay()
        {
            _mansionStateMachine.NPC.EndStaySequence();
            _mansionStateMachine.NPC = null;
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
            _mansionStateMachine.Enter<CollectableState, UnitTaskData>(_stateRewardData);
        }

    }

}
