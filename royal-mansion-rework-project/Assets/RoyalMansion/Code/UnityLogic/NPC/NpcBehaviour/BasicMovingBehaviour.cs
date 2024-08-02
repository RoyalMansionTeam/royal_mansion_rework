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
        private readonly Vector3 _targetV3;
        private readonly NpcState _state;

        public NpcState State => _state;
        public BasicMovingBehaviour(NavMeshAgent agent, Transform target, NpcState state)
        {
            _agent = agent;
            _target = target;
            _state = state;
        }
        public BasicMovingBehaviour(NavMeshAgent agent, Vector3 target, NpcState state)
        {
            _agent = agent;
            _targetV3 = target;
            _state = state;
        }


        public void Enter()
        {
            _agent.isStopped = false;
            if (_target != null)
                _agent.SetDestination(_target.position);
            else
                _agent.SetDestination(_targetV3);
        }
        public void Exit()
        {
            _agent.isStopped = true;
        }
    }

}

