using RoyalMansion.Code.UnityLogic.NPC.NpcBehaviour;
using RoyalMasion.Code.Extensions;
using RoyalMasion.Code.Infrastructure.Saving;
using RoyalMasion.Code.Infrastructure.Services.SaveLoadService;
using RoyalMasion.Code.UnityLogic.MasionManagement.ApartmentLogic;
using System;
using UnityEngine;
using UnityEngine.AI;

namespace RoyalMansion.Code.UnityLogic.NPC
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class NpcBase : MonoBehaviour, ISaveWriter, ISaveReader
    {
        public string SaveableID { get; set; }
        public string AssignedUnitID;

        [SerializeField] protected NavMeshAgent _agent;

        private INpcBehaviour _currentBehavior;
        private GameProgress _progress;

        public void SpawnUI()
        {
            Debug.Log("Spawn NPC HUD");
        }

        public void DespawnUI()
        {
            Debug.Log("Despawn NPC HUD");
        }

        protected void EnterBehaviour(INpcBehaviour newBehaviour)
        {
            _currentBehavior?.Exit();
            _currentBehavior = newBehaviour;
            _currentBehavior?.Enter();
        }

        public void SaveProgress(GameProgress progress)
        {
            _progress = progress;
            progress.MansionProgress.TryAddNpc(new NpcSaveData(
                uniqueSaveID: SaveableID,
                position: transform.localPosition.AsVectorData(),
                assignedUnitID: AssignedUnitID
                ));
        }

        private void OnDestroy()
        {
            if (_progress == null)
                return;
            _progress.MansionProgress.TryRemoveNpc(SaveableID);
        }

        public void LoadProgress(GameProgress progress)
        {
        }
    }
}
