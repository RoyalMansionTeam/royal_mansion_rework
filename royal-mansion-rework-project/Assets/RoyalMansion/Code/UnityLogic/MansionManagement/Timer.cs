using RoyalMansion.Code.Extensions;
using RoyalMasion.Code.Infrastructure.Saving;
using RoyalMasion.Code.Infrastructure.Services.SaveLoadService;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using VContainer;

namespace RoyalMasion.Code.UnityLogic.MasionManagement
{
    public class Timer : MonoBehaviour,  ISaveWriter, ISaveReader
    {
        [SerializeField] private Image _timerBar;
        public Action TimerDone;

        private GameProgress _gameProgress;
        private float _elapsedTime;
        private float _taskTime;
        private float _taskEndRealime;

        public string SaveableID { get; set; }

        public void InitProgressService(IPersistentProgressService progressService)
        {
            _gameProgress = progressService.Progress;
        }
        public void InitTimer(float taskTime, string UnitID)
        {
            SaveableID = UnitID;
            _taskTime = _elapsedTime = taskTime;
            if (TryLoadProgress(out TimerSaveData loadData) != null)
                LoadTimer(loadData.TaskEndRealtime);
            else
                SetNewTimer();
        }
        public void Despawn()
        {
            Destroy(gameObject);
        }
        private TimerSaveData TryLoadProgress(out TimerSaveData data)
        {
            data = null;
            if (_gameProgress.MansionProgress.TimerSaveData == null)
                return data;
            foreach(TimerSaveData timerData in _gameProgress.MansionProgress.TimerSaveData)
            {
                if (timerData.UniqueSaveID != SaveableID)
                    continue;
                data = timerData;
                break;
            }
            return data;
        }

        private void SetNewTimer() => 
            _taskEndRealime = Epoch.Current() + _taskTime;

        private void LoadTimer(float savedEndRealtime)
        {
            _taskEndRealime = savedEndRealtime;
            _elapsedTime = Epoch.SecondsElapsed(_taskEndRealime);
            if(_elapsedTime<=0)
                TimerDone?.Invoke();
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

        public void SaveProgress(GameProgress progress)
        {
            progress.MansionProgress.TryAddTimer(
                new TimerSaveData(
                    uniqueSaveID: SaveableID,
                    taskEndRealtime: _taskEndRealime
                ));
        }

        public void LoadProgress(GameProgress progress)
        {
        }
    }

    
}