using RoyalMansion.Code.UI.Windows.Catalog;
using RoyalMansion.Code.UnityLogic.NPC;
using RoyalMasion.Code.Infrastructure.Data;
using RoyalMasion.Code.Infrastructure.StateMachine;
using UnityEngine;

namespace RoyalMasion.Code.UnityLogic.MasionManagement.MansionStateMachine.States
{
    public class EmptyState : IMansionState
    {
        private IMansionStateMashine _mansionStateMachine;
        private MansionStateMachineData _staticData;
        private GuestNPC _npc;

        public void SetupStateMachine(IMansionStateMashine gameStateMachine)
        {
            _mansionStateMachine = gameStateMachine;
            _staticData = _mansionStateMachine.GetStateMachineData();
        }
        public void Enter()
        {
            if (_staticData.UnitData.UnitType == UnitType.Apartment)
            {
                _npc = _staticData.NpcFactory.SpawnNpc<GuestNPC>();
                _npc.SetNPC();
                _mansionStateMachine.NPC = _npc;
                _npc.UnitAchived += OnNPCWalkEnd;
            }
        }

        public void Stay()
        {
            if (_staticData.UnitData.UnitType != UnitType.Apartment &
                _staticData.UnitData.UnitType != UnitType.Garden)
                return;
            GameObject catalog = _staticData.UiFactory.CreateWindow(WindowID.Catalog);
            catalog.GetComponent<CatalogWindow>().SetUnitType(targetType: _staticData.UnitData.UnitType,
                spawnPoint: _staticData.ItemSpawnPoint);
        }
        private void OnNPCWalkEnd()
        {
            _mansionStateMachine.Enter<InUseState>();
        }
        public void Exit()
        {
            if (_staticData.UnitData.UnitType == UnitType.Apartment)
                _staticData.SceneContext.Kitchen.AddToOrderList(_npc);
        }
    }
}
