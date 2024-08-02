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
    public class NpcBase : MonoBehaviour, ISaveWriter
    {
        public string SaveableID { get; set; }

        [SerializeField] protected NavMeshAgent _agent;
        public string AssignedUnitID;
        public bool FinishedTask = false;
        public int State;
        public Vector3 TargetPosition = Vector3.zero;
        public string TargetUnitID;


        private INpcBehaviour _currentBehavior;
        private GameProgress _progress;
        public INpcBehaviour CurrentBehavior => _currentBehavior;

        public void SetProgress(IPersistentProgressService progressService)
        {
            _progress = progressService.Progress;
        }

        public void SpawnUI()
        {
            //Debug.Log("Spawn NPC HUD");
        }

        public void DespawnUI()
        {
            //Debug.Log("Despawn NPC HUD");
        }

        protected void EnterBehaviour(INpcBehaviour newBehaviour)
        {
            _currentBehavior?.Exit();
            _currentBehavior = newBehaviour;
            _currentBehavior?.Enter();
        }

        public void SaveProgress(GameProgress progress)
        {
            if (this == null)
                return;
            if (AssignedUnitID == "")
                return;
            _progress = progress;
            progress.MansionProgress.TryAddNpc(new NpcSaveData(
                uniqueSaveID: SaveableID,
                position: transform.localPosition.AsVectorData(),
                assignedUnitID: AssignedUnitID,
                state: State,
                targetPosition: TargetPosition.AsVectorData(),
                targetUnitID: TargetUnitID
                ));
        }

        public void Despawn()
        {
            if (_progress != null)
                _progress.MansionProgress.TryRemoveNpc(SaveableID);
            Destroy(gameObject);
        }
    }
}
