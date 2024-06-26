using RoyalMansion.Code.Extensions.Utils;
using RoyalMasion.Code.Editor;
using RoyalMasion.Code.Infrastructure.Data;
using RoyalMasion.Code.Infrastructure.Services.SaveLoadService;
using RoyalMasion.Code.UnityLogic.MasionManagement.GardenLogic;
using RoyalMasion.Code.UnityLogic.MasionManagement.MansionStateMachine;
using RoyalMasion.Code.UnityLogic.MasionManagement.MansionStateMachine.States;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RoyalMasion.Code.UnityLogic.MasionManagement.ApartmentLogic
{
    public class ApartmentUnit : MansionUnit
    {
        [SerializeField] private ApartmentUnitStaticData _unitData;
        [SerializeField] private ObjectClickHandler _touchHandler;


        private UnitFurnitureRequirements[] _basicRequirements;

        private void Start()
        {
            Subscribe();
            InitUnitData(_unitData);
            //EnterFirstState();
            SetBasicUnitRequirements();
        }

        private void HandleAction()
        {
            StateMashine.Stay();
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

        public void SetBasicUnitRequirements()
        {
            _basicRequirements = UnitData.BasicRequirements;
            if (_basicRequirements.Length == 0)
                return;
            foreach (UnitFurnitureRequirements requiredItem in _basicRequirements) //TODO: Chech for saved items
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
                requiredItem.Amount +=1;
                break;
            }
            BasicUnitRequirementsMet = CheckRequirements();
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

