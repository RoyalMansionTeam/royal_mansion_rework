using System;
using UnityEngine;

namespace RoyalMansion.Code.UnityLogic.NPC
{
    public class GuestNPC : NpcBase
    {
        public Action UnitAchived;

        private bool _allowToMove = false;

        //при создании начинает движение к заданной комнате
        public void SetNPC()
        {
            _allowToMove = true;
        }
        private void Update()
        {
            MoveToUnit();
        }

        private void MoveToUnit()
        {
            if (!_allowToMove)
                return;
            if (true) //if walked to target unit
                OnUnitAchieved();
        }
        //дальше триггерит смену стейта
        private void OnUnitAchieved()
        {
            _allowToMove = false;
            SpawnUI();
            UnitAchived?.Invoke();
        }
        //дальше активирует накопительную вероятность заказа
        //обрабатывает заказ (в работе с кухней), пока заглушка
        //по сигналу от стейт машины (вошли в коллектабл стейт) запускает процесс деспавна
        public void EndStaySequence()
        {
            //поменять цель на зону деспавна
            Despawn();
        }

        private void Despawn()
        {
            Destroy(gameObject);
        }
    }
}