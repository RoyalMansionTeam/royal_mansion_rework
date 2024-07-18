using RoyalMansion.Code.Extensions.Utils;
using RoyalMansion.Code.UnityLogic.NPC;
using RoyalMasion.Code.Editor;
using RoyalMasion.Code.Infrastructure.Data;
using RoyalMasion.Code.Infrastructure.Services.SaveLoadService;
using RoyalMasion.Code.UnityLogic.MasionManagement.GardenLogic;
using RoyalMasion.Code.UnityLogic.MasionManagement.MansionStateMachine;
using RoyalMasion.Code.UnityLogic.MasionManagement.MansionStateMachine.States;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RoyalMasion.Code.UnityLogic.MasionManagement.ApartmentLogic
{
    public class ApartmentUnit : MansionUnit
    {
        [SerializeField] private ApartmentUnitStaticData _unitData;
        [SerializeField] private ObjectClickHandler _touchHandler;
        [SerializeField] private List<ApartmentMaterialParents> _materialsData;


        private UnitFurnitureRequirements[] _basicRequirements;

        private void Start()
        {
            Subscribe();
            InitUnitData(_unitData);
            SetBasicUnitRequirements();
            InitUnitUI();
        }

        private void HandleAction()
        {
            StateMashine.Stay();
        }
        private void Subscribe()
        {
            _touchHandler.ClickHandled += HandleAction;
            ItemBoughtEvent += OnItemBought;
            StateMachineInitiated += ApplyMaterialsData;
        }


        private void Unsubscribe()
        {
            _touchHandler.ClickHandled -= HandleAction;
            ItemBoughtEvent -= OnItemBought;
            StateMachineInitiated -= ApplyMaterialsData;
        }

        public void NpcEnteredUnitEvent(NpcBase npc)
        {
            if (npc.AssignedUnitID != UnitData.UnitID)
                return;
            if (StateMashine == null || StateMashine.NPC == null)
                return;
            StateMashine.NPC.OnUnitAchieved();
        }

        private void SetBasicUnitRequirements()
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
        private void ApplyMaterialsData()
        {
            MaterialsData = _materialsData;
            StateMashine.GetStateMachineData().ApartmentMaterialsData = _materialsData;
        }

        private void OnDestroy()
        {
            Unsubscribe();
        }
    }

}

