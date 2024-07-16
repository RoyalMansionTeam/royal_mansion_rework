using RoyalMasion.Code.Infrastructure.Data;
using RoyalMasion.Code.UnityLogic.MasionManagement.ApartmentLogic;
using RoyalMasion.Code.UnityLogic.MasionManagement.MansionStateMachine.States;
using RoyalMasion.Code.UnityLogic.MasionManagement.MansionStateMachine;
using System;
using UnityEngine;
using RoyalMansion.Code.Extensions.Utils;
using RoyalMasion.Code.Editor;

namespace RoyalMasion.Code.UnityLogic.MasionManagement.GardenLogic
{
    public class GardenUnit : MansionUnit
    {
        [SerializeField] private GardenUnitStaticData _unitData;
        [SerializeField] private ObjectClickHandler _touchHandler;

        private UnitFurnitureRequirements[] _basicRequirements;

        private void OnEnable()
        {
            Subscribe();
            InitUnitData(_unitData);
            SetBasicUnitRequirements();
        }

        private void Subscribe()
        {
            _touchHandler.ClickHandled += HandleAction;
            ItemBoughtEvent += OnItemBought;
        }

        private void Unsubscribe()
        {
            _touchHandler.ClickHandled -= HandleAction;
            ItemBoughtEvent -= OnItemBought;
        }

        private void HandleAction()
        {
            StateMashine.Stay();
        }
        public void SetBasicUnitRequirements()
        {
            _basicRequirements = UnitData.GetUnitBasicRequirements();
            if (_basicRequirements.Length == 0)
                return;
            foreach (UnitFurnitureRequirements requiredItem in _basicRequirements)
                requiredItem.Amount = 0;
            BasicUnitRequirementsMet = false;
        }
        private void OnItemBought(CatalogSection itemSection)
        {
            if (BasicUnitRequirementsMet)
                return;
            foreach (UnitFurnitureRequirements requiredItem in _basicRequirements)
            {
                if (requiredItem.ItemType != itemSection)
                    continue;
                requiredItem.Amount += 1;
                break;
            }
            BasicUnitRequirementsMet = _stateMashineData.BasicRequirementsMetState = CheckRequirements();
            if (BasicUnitRequirementsMet)
                _stateMashineData.BasicRequirementsMet?.Invoke();
        }

        private bool CheckRequirements()
        {
            bool requirementsMet = true;
            foreach (UnitFurnitureRequirements requiredItem in _basicRequirements)
                if (requiredItem.Amount == 0)
                    requirementsMet = false;
            return requirementsMet;
        }

        private void OnDestroy()
        {
            Unsubscribe();
        }
    }
}
