using RoyalMasion.Code.Infrastructure.Data;
using RoyalMasion.Code.Infrastructure.StateMachine;
using RoyalMasion.Code.UnityLogic.MasionManagement.ApartmentLogic;
using UnityEngine;

namespace RoyalMasion.Code.UnityLogic.MasionManagement.MansionStateMachine.States
{
    public class CollectableState : IMansionPayloadedState<UnitTaskData>
    {
        private IMansionStateMashine _mansionStateMachine;
        private UnitTaskData _stateRewardData;
        private MansionStateMachineData _stateMachineData;

        public void SetupStateMachine(IMansionStateMashine gameStateMachine)
        {
            _mansionStateMachine = gameStateMachine;
        }
        public void Enter(UnitTaskData stateRewardData)
        {
            Debug.Log("Enter Collectable");
            _stateRewardData = stateRewardData;
            _stateMachineData = _mansionStateMachine.GetStateMachineData();
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
        }
    }
}
