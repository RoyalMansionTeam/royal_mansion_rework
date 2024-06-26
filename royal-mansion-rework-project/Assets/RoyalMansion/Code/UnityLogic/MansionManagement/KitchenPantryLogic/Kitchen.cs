using RoyalMansion.Code.UnityLogic.NPC;
using RoyalMasion.Code.UnityLogic.MasionManagement.ApartmentLogic;
using RoyalMasion.Code.UnityLogic.MasionManagement.GardenLogic;
using RoyalMasion.Code.UnityLogic.MasionManagement.MansionStateMachine;
using RoyalMasion.Code.UnityLogic.MasionManagement.MansionStateMachine.States;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RoyalMasion.Code.UnityLogic.MasionManagement.KitchenGardenLogic
{
    public class Kitchen : MansionUnit
    {
        [SerializeField] private KitchenStaticData _unitData;

        private List<GuestNPC> _guestsToOrder = new();
        private bool _isAbleToCook;
        private bool _inOrderLoop;

        private GuestNPC _currentGuest;


        private void OnEnable()
        {
            InitUnitData(_unitData);
            _isAbleToCook = false;
            _inOrderLoop = false;
            StateMashine.Enter<EmptyState>();
            Subscribe();
        }
        private void Subscribe()
        {
            StateMashine.StateChanged += TrackKitchenLoop;
        }


        private void Unsubscribe()
        {
            StateMashine.StateChanged -= TrackKitchenLoop;
        }
        private void HandleTouch()
        {
            StateMashine.Stay();
            HandleCookCommand();
        }

        private void Update()
        {
            if (HasOrders())
                TryHandleOrder();
            else
                DespawnUI();
        }


        public void AddToOrderList(GuestNPC guest)
        {
            if (!_guestsToOrder.Contains(guest))
                _guestsToOrder.Add(guest);
        }

        private bool HasOrders() => 
            _guestsToOrder.Count != 0;

        private void TryHandleOrder()
        {
            if (_inOrderLoop)
                return;
            if (!CheckResources())
                return;
            _isAbleToCook = true;
            //если да, разместить UI готовки и разрешить по клику переход в InUseState
            PlaceUI();
        }
        private bool CheckResources()
        {
            return true;
        }
        private void PlaceUI()
        {
        }
        private void DespawnUI()
        {
        }

        private void TrackKitchenLoop(IExitableMansionState state)
        {
            if (state.GetType() == typeof(CollectableState))
                StartDelivery();
            else if (state.GetType() == typeof(EmptyState))
                _inOrderLoop = false;

        }

        private void StartDelivery()
        {
            WaiterNPC waiter = _npcFactory.SpawnNpc<WaiterNPC>();
            waiter.SpawnUI();
        }

        private void HandleCookCommand()
        {
            if (!_isAbleToCook)
                return;
            _isAbleToCook = false;
            _inOrderLoop = true;
            _currentGuest = _guestsToOrder[UnityEngine.Random.Range(0, _guestsToOrder.Count - 1)];
            RemoveGuestFromOrderList(_currentGuest);
            StateMashine.Enter<InUseState>();
        }
        private void RemoveGuestFromOrderList(GuestNPC guest)
        {
            if (_guestsToOrder.Contains(guest))
                _guestsToOrder.Remove(guest);
        }
        private void OnDestroy()
        {
            Unsubscribe();
        }
    }

}
