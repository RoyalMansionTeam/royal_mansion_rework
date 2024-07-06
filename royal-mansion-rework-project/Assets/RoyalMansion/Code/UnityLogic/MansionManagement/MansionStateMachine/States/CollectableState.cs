using RoyalMansion.Code.UI.WorldspaceUI;
using RoyalMansion.Code.UnityLogic.NPC;
using RoyalMasion.Code.Extensions;
using RoyalMasion.Code.Infrastructure.Data;
using RoyalMasion.Code.Infrastructure.Saving;
using RoyalMasion.Code.Infrastructure.StateMachine;
using RoyalMasion.Code.UnityLogic.MasionManagement.ApartmentLogic;
using UnityEngine;

namespace RoyalMasion.Code.UnityLogic.MasionManagement.MansionStateMachine.States
{
    public class CollectableState : IMansionPayloadedState<UnitTaskData>
    {
        private IMansionStateMachine _mansionStateMachine;
        private UnitTaskData _stateRewardData;
        private MansionStateMachineData _stateMachineData;
        private GuestNPC _npc;

        public void SetupStateMachine(IMansionStateMachine gameStateMachine)
        {
            _mansionStateMachine = gameStateMachine;
        }
        public void Enter(UnitTaskData stateRewardData)
        {
            _stateRewardData = stateRewardData;
            _stateMachineData = _mansionStateMachine.GetStateMachineData();
            _mansionStateMachine.LastTask = stateRewardData.State;
            if (_stateMachineData.UnitData.UnitType == UnitType.Apartment)
                SetApartmentUI();
            if (_stateMachineData.NpcSaveData != null)
                SpawnNPC(_stateMachineData.NpcSaveData);
        }
        public void Stay()
        {
            _stateMachineData.EconomyData.SetEconomyData
                (
                     dataType: _stateRewardData.Reward,
                    addAmount: _stateRewardData.Amount
                );
            if (_mansionStateMachine.GetStateMachineData().UnitData.GetTaskData(UnitState.Dirty) != null)
                _mansionStateMachine.Enter<DirtyState>();
            else if (_mansionStateMachine.GetStateMachineData().UnitData.UnitType == UnitType.Kitchen)
                _mansionStateMachine.Enter<EmptyState>();
            else
                _mansionStateMachine.Enter<InUseState>();
        }
        public void Exit()
        {
            SetNpcDestination();
        }

        private async void SetApartmentUI()
        {
            GameObject uiObject = await _mansionStateMachine.GetStateMachineData().UnitUIHandler.
            SetUIMessenge(InternalUnitStates.Collectable);
            if (uiObject.TryGetComponent(out TextUIHandler textUIHandler))
                textUIHandler.SetTextField(_stateRewardData.Amount.ToString());

        }
        private void SpawnNPC(NpcSaveData saveData)
        {
            _npc = _stateMachineData.NpcFactory.SpawnNpc<GuestNPC>();
            _npc.gameObject.transform.position = saveData.Position.AsUnityVector();
            _npc.SaveableID = saveData.UniqueSaveID;
            _npc.AssignedUnitID = _stateMachineData.UnitData.UnitID;
            _mansionStateMachine.NPC = _npc;
            SetNpcDestination();
        }

        private void SetNpcDestination()
        {
            _mansionStateMachine.NPC.SetNPC(_stateMachineData.SceneContext.MansionSpawnPoints.GuestSpawnPoint);
        }
    }
}
