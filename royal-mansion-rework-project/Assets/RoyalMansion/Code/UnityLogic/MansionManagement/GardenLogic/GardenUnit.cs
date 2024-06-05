using RoyalMasion.Code.Infrastructure.Data;
using RoyalMasion.Code.UnityLogic.MasionManagement.ApartmentLogic;
using RoyalMasion.Code.UnityLogic.MasionManagement.MansionStateMachine.States;
using RoyalMasion.Code.UnityLogic.MasionManagement.MansionStateMachine;
using System;
using UnityEngine;
using RoyalMansion.Code.Extensions.Utils;

namespace RoyalMasion.Code.UnityLogic.MasionManagement.GardenLogic
{
    public class GardenUnit : MansionUnit
    {
        [SerializeField] private GardenUnitStaticData _unitData;

        private void OnEnable()
        {
            InitUnitData(_unitData);
            StateMashine.Enter<LockedState, UnitStaticData>(null);
            Subscribe();
        }

        private void HandleAction()
        {
            StateMashine.Stay();
        }
        private void Subscribe()
        {
            UnitActionBtn.onClick.AddListener(HandleAction);
        }
        private void Unsubscribe()
        {
            UnitActionBtn.onClick.RemoveListener(HandleAction);
        }


        private void OnDestroy()
        {
            Unsubscribe();
        }
    }
}
