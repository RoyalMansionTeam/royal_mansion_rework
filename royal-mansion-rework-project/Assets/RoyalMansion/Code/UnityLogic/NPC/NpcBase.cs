using RoyalMasion.Code.UnityLogic.MasionManagement.ApartmentLogic;
using System;
using UnityEngine;

namespace RoyalMansion.Code.UnityLogic.NPC
{
    public class NpcBase : MonoBehaviour
    {
        public Transform NavMeshTarget { get; private set; }

        public void SetTarget(Transform target)
        {
            NavMeshTarget = target;
        }

        public void SpawnUI()
        {
            Debug.Log("Spawn NPC HUD");
        }

        public void DespawnUI()
        {
            Debug.Log("Despawn NPC HUD");
        }

    }
}
