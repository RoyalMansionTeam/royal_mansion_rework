using RoyalMasion.Code.Infrastructure.Data;
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
        private readonly NpcState _state;

        public NpcState State => _state;
        public BasicMovingBehaviour(NavMeshAgent agent, Transform target, NpcState state)
        {
            _agent = agent;
            _target = target;
            _state = state;
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

