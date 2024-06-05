using RoyalMasion.Code.Infrastructure.StateMachine;
using RoyalMasion.Code.UnityLogic.MasionManagement.ApartmentLogic;
using System;
using UnityEngine;

namespace RoyalMasion.Code.UnityLogic.MasionManagement.MansionStateMachine.States
{
    public class LockedState : IMansionPayloadedState<UnitStaticData>
    {
        private IMansionStateMashine _mansionStateMachine;
        public void SetupStateMachine(IMansionStateMashine gameStateMachine)
        {
            _mansionStateMachine = gameStateMachine;
        }
        public void Enter(UnitStaticData unitData)
        {
        }
        public void Stay()
        {
            _mansionStateMachine.Enter<EmptyState>();
        }
        public void Exit()
        {
        }
    }
}
