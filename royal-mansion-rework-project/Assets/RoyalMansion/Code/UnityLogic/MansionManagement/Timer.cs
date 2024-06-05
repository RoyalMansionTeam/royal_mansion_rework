using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace RoyalMasion.Code.UnityLogic.MasionManagement
{
    public class Timer : MonoBehaviour
    {
        [SerializeField] private Image _timerBar;
        public Action TimerDone;

        private float _elapsedTime;
        private float _taskTime;

        public void InitTimer(float taskTime)
        {
            //_elapsedTime = targetTime;
            _taskTime = _elapsedTime = taskTime;
        }
        public void Despawn()
        {
            CleanUp();
            Destroy(gameObject);
        }
        private void Update()
        {
            UpdateTime();
        }

        private void UpdateTime()
        {
            if (_elapsedTime <= 0)
                return;
            _elapsedTime -= Time.deltaTime;
            if (_elapsedTime <= 0)
                TimerDone?.Invoke();
            float timeLeftPercent = _elapsedTime / _taskTime;
            UpdateUI(timeLeftPercent);
        }

        private void UpdateUI(float timePercent)
        {
            _timerBar.fillAmount = timePercent;
        }

        private void CleanUp()
        {
            //Clear player prefs of this timer

        }

    }

    
}