using RoyalMansion.Code.UnityLogic.NPC;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RoyalMasion.Code.UnityLogic.MasionManagement.MansionTriggers
{
    [RequireComponent(typeof(BoxCollider))]
    public class NPCDespawnTriggerZone : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.transform.parent == null)
                return;
            if (other.gameObject.TryGetComponent(out NpcBase npc))
            {
                if (npc.FinishedTask)
                    npc.Despawn();
            }
        }
    }
}