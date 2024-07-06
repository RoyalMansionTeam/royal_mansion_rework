using RoyalMansion.Code.UI.Windows.Catalog;
using RoyalMansion.Code.UnityLogic.NPC;
using RoyalMasion.Code.Extensions;
using RoyalMasion.Code.Extensions.Utils;
using RoyalMasion.Code.Infrastructure.Data;
using RoyalMasion.Code.Infrastructure.Saving;
using RoyalMasion.Code.Infrastructure.StateMachine;
using System;
using UnityEngine;

namespace RoyalMasion.Code.UnityLogic.MasionManagement.MansionStateMachine.States
{
    public class EmptyState : IMansionState
    {
        private IMansionStateMachine _mansionStateMachine;
        private MansionStateMachineData _staticData;
        private GuestNPC _npc;

        public void SetupStateMachine(IMansionStateMachine gameStateMachine)
        {
            _mansionStateMachine = gameStateMachine;
            _staticData = _mansionStateMachine.GetStateMachineData();
        }
        public void Enter()
        {
            SetEmptyApartmentUI();
            _staticData.BasicRequirementsMet += OnUnitBasicRequirementsMet;
            if (_staticData.NpcSaveData != null)
                SpawnNPC(_staticData.NpcSaveData);
        }
        public void Stay()
        {
            if (_staticData.UnitData.UnitType != UnitType.Apartment &
                _staticData.UnitData.UnitType != UnitType.Garden)
                return;
            GameObject catalog = _staticData.UiFactory.CreateWindow(WindowID.Catalog);
            catalog.GetComponent<CatalogWindow>().SetUnitType(
                targetType: _staticData.UnitData.UnitType,
                spawnPoint: _staticData.ItemSpawnPoint,
                unitID: _staticData.UnitData.UnitID,
                unitOnBuyEvent: _staticData.ItemBoughtEvent);
        }
        public void Exit()
        {
            /*if (_staticData.UnitData.UnitType == UnitType.Apartment)
                _staticData.SceneContext.Kitchen.AddToOrderList(_npc);*/
        }

        private void OnUnitBasicRequirementsMet()
        {
            if (_staticData.UnitData.UnitType == UnitType.Apartment)
            {
                SetFurnituredApartmentUI();
                if (_mansionStateMachine.NPC == null)
                    SpawnNPC();
            }
        }

        private async void SetFurnituredApartmentUI() =>
            await _mansionStateMachine.GetStateMachineData().UnitUIHandler.
            SetUIMessenge(InternalUnitStates.AwaitingGuests);
        private async void SetEmptyApartmentUI() =>
            await _mansionStateMachine.GetStateMachineData().UnitUIHandler.
            SetUIMessenge(InternalUnitStates.AwaitingFurniture);

        private void SpawnNPC()
        {
            _npc = _staticData.NpcFactory.SpawnNpc<GuestNPC>();
            _npc.SaveableID = _npc.GetComponent<UniqueId>().GenerateId();
            SetNpcData();
        }

        private void SpawnNPC(NpcSaveData saveData)
        {
            _npc = _staticData.NpcFactory.SpawnNpc<GuestNPC>();
            _npc.gameObject.transform.position = saveData.Position.AsUnityVector();
            _npc.SaveableID = saveData.UniqueSaveID;
            SetNpcData();
        }

        private void SetNpcData()
        {
            _npc.SetNPC(_staticData.NavMeshTarget);
            _npc.AssignedUnitID = _staticData.UnitData.UnitID;
            _mansionStateMachine.NPC = _npc;
            _npc.UnitAchived += OnNPCWalkEnd;
        }

        private void OnNPCWalkEnd()
        {
            _mansionStateMachine.Enter<InUseState>();
        }
    }
}
