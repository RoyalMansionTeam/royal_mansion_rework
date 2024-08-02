using RoyalMansion.Code.UI.WorldspaceUI;
using RoyalMansion.Code.UnityLogic.CameraLogic;
using RoyalMansion.Code.UnityLogic.NPC.NpcBehaviour;
using RoyalMasion.Code.Infrastructure.Data;
using RoyalMasion.Code.Infrastructure.Services.UIFactory;
using System;
using UnityEngine;

namespace RoyalMansion.Code.UnityLogic.NPC
{
    public class MaidNPC : NpcBase
    {
        public Action UnitAchived;

        private Transform _destination;
        private IUIFactory _uiFactory;
        private MansionUnitUIHandler _uiHandler;

        public void SetNPCData(IUIFactory uiFactory, DraggableCamera cameraController)
        {
            AssignedUnitID = "Waiter";
            EnterBehaviour(new BasicMovingBehaviour(
                agent: _agent,
                target: transform,
                NpcState.Resting
                ));
            /*_uiFactory = uiFactory;
            _uiHandler = _uiFactory.CreateUnitUIHandler();
            _uiHandler.SetHandlerData(transform.position, cameraController);*/
        }

        public void SetTarget(Transform destination) =>
            _destination = destination;

        public void EnterMaidBehaviour(NpcState state)
        {
            EnterBehaviour(new BasicMovingBehaviour(
                agent: _agent,
                target: _destination,
                state: state
                ));
        }
        public void OnTargetAchieved()
        {
            //SpawnUI();
            UnitAchived?.Invoke();
        }
    }
}