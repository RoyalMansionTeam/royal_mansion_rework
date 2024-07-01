using RoyalMansion.Code.UI.WorldspaceUI;
using RoyalMasion.Code.Infrastructure.Data;
using RoyalMasion.Code.Infrastructure.StateMachine;
using RoyalMasion.Code.UnityLogic.MasionManagement.ApartmentLogic;
using System;
using System.Threading.Tasks;
using UnityEngine;

namespace RoyalMasion.Code.UnityLogic.MasionManagement.MansionStateMachine.States
{
    public class LockedState : IMansionState
    {
        private IMansionStateMachine _mansionStateMachine;
        private MansionStateMachineData _mansionStateMachineData;
        public void SetupStateMachine(IMansionStateMachine gameStateMachine)
        {
            _mansionStateMachine = gameStateMachine;
            _mansionStateMachineData = _mansionStateMachine.GetStateMachineData();
        }

        public void Enter()
        {
            Subscribe();
            OnResourcesChanged(ResourceType.SoftVallue,
                _mansionStateMachineData.EconomyData.GetEconomyData(ResourceType.SoftVallue));
        }

        private void Subscribe()
        {
            _mansionStateMachineData.EconomyData.ResourceChanged += OnResourcesChanged;
        }
        private void Unsubscribe()
        {
            _mansionStateMachineData.EconomyData.ResourceChanged -= OnResourcesChanged;
        }

        private void OnResourcesChanged(ResourceType resourceType, int amount)
        {
            if (!CheckPriceConditions(resourceType, amount))
                SetLockedUI();
            else
                SetUnlockableUI();
        }


        private bool CheckPriceConditions(ResourceType resourceType, int amount)
        {
            if (resourceType != ResourceType.SoftVallue)
                return false;
            return amount >= _mansionStateMachine.GetStateMachineData().UnitData.UnitPrice;
        }

        private async void SetLockedUI()
        {
            GameObject uiObject = await _mansionStateMachine.GetStateMachineData().UnitUIHandler.
            SetUIMessenge(InternalUnitStates.Locked);
            if (uiObject == null)
                return;
            if (uiObject.TryGetComponent(out TextUIHandler textUIHandler))
                textUIHandler.SetTextField(_mansionStateMachine.GetStateMachineData().
                    UnitData.UnitPrice.ToString());
        }
        private async void SetUnlockableUI()
        {
            GameObject uiObject = await _mansionStateMachine.GetStateMachineData().UnitUIHandler.
            SetUIMessenge(InternalUnitStates.Unlockable);
            if (uiObject == null)
                return;
            Debug.Log(uiObject.name);
            if (uiObject.TryGetComponent(out TextUIHandler textUIHandler))
            {
                Debug.Log("HasUnlockable");
                textUIHandler.SetTextField(_mansionStateMachine.GetStateMachineData().
                      UnitData.UnitPrice.ToString());
            }
        }

        public void Stay()
        {
            TryToBuyUnit();
        }

        private void TryToBuyUnit()
        {
            if (!CheckPriceConditions(ResourceType.SoftVallue,
                _mansionStateMachineData.EconomyData.GetEconomyData(ResourceType.SoftVallue)))
                return;
            Unsubscribe();
            _mansionStateMachineData.EconomyData.SetEconomyData(ResourceType.SoftVallue,
                -_mansionStateMachineData.UnitData.UnitPrice);
            _mansionStateMachine.Enter<EmptyState>();
        }

        public void Exit()
        {
        }
    }
}
