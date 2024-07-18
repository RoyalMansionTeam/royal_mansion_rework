using RoyalMansion.Code.Extensions.Utils;
using RoyalMansion.Code.UI.WorldspaceUI;
using RoyalMansion.Code.UnityLogic.CameraLogic;
using RoyalMansion.Code.UnityLogic.NPC.NpcBehaviour;
using RoyalMasion.Code.Infrastructure.Data;
using RoyalMasion.Code.Infrastructure.Services.UIFactory;
using RoyalMasion.Code.Infrastructure.StaticData;
using RoyalMasion.Code.UnityLogic.MasionManagement;
using System;
using UnityEngine;
using VContainer;

namespace RoyalMansion.Code.UnityLogic.NPC
{
    public class CookNPC : NpcBase
    {
        public bool AvailableToCollect = false;

        private WaiterNPC _assignedWaiter;
        private IUIFactory _uiFactory;
        private MansionUnitUIHandler _uiHandler;
        private KitchenOrderData _orderData;
        private Timer _timer;

        public int OrderReward => _orderData.OrderReward;
        public void SetNPCData(IUIFactory uiFactory, DraggableCamera cameraController)
        {
            EnterBehaviour(new BasicMovingBehaviour(
                agent: _agent,
                target: transform,
                NpcState.Resting
                ));

            _uiFactory = uiFactory;
            _uiHandler = _uiFactory.CreateUnitUIHandler();
            _uiHandler.SetHandlerData(transform.position, cameraController);
            _uiHandler.gameObject.SetActive(false);
            AvailableToCollect = false;
        }

        public async void PlaceUI(InternalUnitStates type)
        {
            _uiHandler.gameObject.SetActive(true);
            if (type == InternalUnitStates.ApartmentStayTimer)
            {
                _timer = await _uiHandler.SetUnitTimer(type);
                _timer.TimerDone += OnTimerDone;
                _timer.InitTimer(_orderData.OrderMakingTime.ToFloat(), "Kitchen");
            }
            else
            {
                GameObject uiElement = await _uiHandler.SetUIMessenge(type);
                if (uiElement.TryGetComponent(out TextUIHandler textUIHandler))
                    textUIHandler.SetTextField(_orderData.OrderReward.ToString());
            }
        }


        public void SetTaskBehaviour(WaiterNPC assignedWaiter, KitchenOrderData orderData)
        {
            _assignedWaiter = assignedWaiter;
            EnterBehaviour(new BasicMovingBehaviour(
                agent: _agent,
                target: transform,
                NpcState.PerformingTask
                ));
            _orderData = orderData;
            PlaceUI(InternalUnitStates.ApartmentStayTimer);
        }

        public void Clear()
        {
            _timer = null;
            _orderData = null;
            _assignedWaiter = null;
            _uiHandler.Clear();
            _uiHandler.gameObject.SetActive(false);
            AvailableToCollect = false;
        }

        private void OnTimerDone()
        {
            _timer.Despawn();
            AvailableToCollect = true;
            PlaceUI(InternalUnitStates.CollectableApartment);
            _assignedWaiter.EnterWaiterBehaviour(NpcState.PerformingTask);
            EnterBehaviour(new BasicMovingBehaviour(
                agent: _agent,
                target: transform,
                NpcState.Resting
                ));

        }
    }
}
