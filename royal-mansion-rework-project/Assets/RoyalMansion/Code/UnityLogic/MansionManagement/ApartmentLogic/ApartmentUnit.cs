using RoyalMasion.Code.UnityLogic.MasionManagement.GardenLogic;
using RoyalMasion.Code.UnityLogic.MasionManagement.MansionStateMachine.States;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RoyalMasion.Code.UnityLogic.MasionManagement.ApartmentLogic
{
    public class ApartmentUnit : MansionUnit
    {
        [SerializeField] private ApartmentUnitStaticData _unitData;

        private void OnEnable()
        {
            Subscribe();
            InitUnitData(_unitData);
            StateMashine.Enter<LockedState, UnitStaticData>(null);
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

