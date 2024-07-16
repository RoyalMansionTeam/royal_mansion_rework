using RoyalMansion.Code.UnityLogic.NPC.NpcBehaviour;
using System;
using UnityEngine;

namespace RoyalMansion.Code.UnityLogic.NPC
{
    public class WaiterNPC : NpcBase
    {
        public Action UnitAchived;
        private bool _allowToMove;

        public void SetWaiterTarget(Transform destination)
        {
            _allowToMove = true;
            EnterBehaviour(new BasicMovingBehaviour(
                agent: _agent,
                target: destination
                ));
        }
        public void OnTargetAchieved()
        {
            _allowToMove = false;
            SpawnUI();
            UnitAchived?.Invoke();
        }
    }
}