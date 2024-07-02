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
        [SerializeField] private Transform _waiterSpawnPoint;
        [SerializeField] private Transform _cookSpawnPoint;

        private List<GuestNPC> _guestsToOrder = new();
        private WaiterNPC _waiterNPC;
        private CookNPC _cookNPC;
        private bool _isAbleToCook;
        private bool _inOrderLoop;

        private GuestNPC _currentGuest;


        private void Start()
        {
            InitUnitData(_unitData);
            SpawnStaff();
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
        private void SpawnStaff()
        {
            _waiterNPC = _npcFactory.SpawnNpc<WaiterNPC>(_waiterSpawnPoint);
            _cookNPC = _npcFactory.SpawnNpc<CookNPC>(_cookSpawnPoint);
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
            //waiter is already spawned
            _waiterNPC.SpawnUI();
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
        public void RemoveGuestFromOrderList(GuestNPC guest)
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
