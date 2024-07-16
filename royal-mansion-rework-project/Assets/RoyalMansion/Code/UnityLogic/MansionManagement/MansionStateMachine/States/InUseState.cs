using RoyalMansion.Code.Extensions.Utils;
using RoyalMasion.Code.Infrastructure.StateMachine;
using UnityEngine;
using VContainer;
using RoyalMasion.Code.Infrastructure.Data;
using RoyalMasion.Code.UnityLogic.MasionManagement.ApartmentLogic;
using System;
using RoyalMasion.Code.Infrastructure.Saving;
using RoyalMansion.Code.UnityLogic.NPC;
using RoyalMasion.Code.Extensions;

namespace RoyalMasion.Code.UnityLogic.MasionManagement.MansionStateMachine.States
{
    public class InUseState : IMansionState
    {
        private MansionStateMachineData _stateMachineData;
        private IMansionStateMachine _mansionStateMachine;

        private Timer _timer;
        private UnitTaskData _stateRewardData;
        private GuestNPC _npc;

        public void SetupStateMachine(IMansionStateMachine gameStateMachine)
        {
            _mansionStateMachine = gameStateMachine;
        }

        public void Enter()
        {
            _stateMachineData = _mansionStateMachine.GetStateMachineData();

            _stateRewardData = _stateMachineData.UnitData.GetTaskData
                (_mansionStateMachine.GetUnitStateEnum(GetType()));
            SpawnTimer();
            if (_stateMachineData.UnitData.UnitType != UnitType.Apartment)
                return;
            if (_stateMachineData.NpcSaveData != null & _mansionStateMachine.NPC == null)
                SpawnNPC(_stateMachineData.NpcSaveData);
            _stateMachineData.SceneContext.Kitchen.AddToOrderList(_npc);
        }

        public void Stay()
        {
            Debug.Log("Offer to boost timer");
        }
        public void Exit()
        {
            _timer.Despawn();
        }

        private void SpawnNPC(NpcSaveData saveData)
        {
            _npc = _stateMachineData.NpcFactory.SpawnNpc<GuestNPC>();
            _npc.gameObject.transform.localPosition = saveData.Position.AsUnityVector();
            _npc.SaveableID = saveData.UniqueSaveID;
            _npc.SetNPC(_npc.gameObject.transform);
            _npc.AssignedUnitID = _stateMachineData.UnitData.UnitID;
            _mansionStateMachine.NPC = _npc;
        }

        private async void SpawnTimer()
        {
            if(_stateMachineData.UnitData.UnitType == UnitType.Apartment)
                _timer = await _stateMachineData.UnitUIHandler.SetUnitTimer(InternalUnitStates.ApartmentStayTimer);
            else if (_stateMachineData.UnitData.UnitType == UnitType.Garden)
                _timer = await _stateMachineData.UnitUIHandler.SetUnitTimer(InternalUnitStates.GardenTimer);
            _timer.TimerDone += OnTimerDone;
            _timer.InitTimer(_stateRewardData.Time.ToFloat(), _stateMachineData.UnitData.UnitID);
        }

        private void OnTimerDone()
        {
            _mansionStateMachine.Enter<CollectableState, UnitTaskData>(_stateRewardData);
        }

    }

}
