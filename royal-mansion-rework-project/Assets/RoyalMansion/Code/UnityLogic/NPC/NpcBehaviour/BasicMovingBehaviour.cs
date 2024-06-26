using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace RoyalMansion.Code.UnityLogic.NPC.NpcBehaviour
{
    public class BasicMovingBehaviour : INpcBehaviour
    {
        private readonly NavMeshAgent _agent;
        private readonly Transform _target;

        public BasicMovingBehaviour(NavMeshAgent agent, Transform target)
        {
            _agent = agent;
            _target = target;
        }
        public void Enter()
        {
            _agent.isStopped = false;
            _agent.SetDestination(_target.position);
        }
        public void Exit()
        {
            _agent.isStopped = true;
        }
    }

}

