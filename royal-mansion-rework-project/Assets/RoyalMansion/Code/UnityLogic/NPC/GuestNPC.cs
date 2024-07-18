using RoyalMansion.Code.UnityLogic.NPC.NpcBehaviour;
using RoyalMasion.Code.Infrastructure.Data;
using System;
using UnityEngine;

namespace RoyalMansion.Code.UnityLogic.NPC
{
    public class GuestNPC : NpcBase
    {
        public Action UnitAchived;
        private bool _allowToMove = false;

        public void SetNPC(Transform destination)
        {
            _allowToMove = true;
            EnterBehaviour(new BasicMovingBehaviour(
                agent: _agent,
                target: destination,
                state: NpcState.PerformingTask
                ));
        }
        public void OnUnitAchieved()
        {
            _allowToMove = false;
            SpawnUI();
            UnitAchived?.Invoke();
        }
    }
}