using UnityEngine;
using UnityEngine.UI;
using RoyalMansion.Code.UI.WorldspaceUI;
using RoyalMansion.Code.UnityLogic.Meta;
using System;
using System.Collections.Generic;
using VContainer;
using RoyalMasion.Code.Infrastructure.Services.StaticData;
using RoyalMasion.Code.Infrastructure.Data;
using RoyalMasion.Code.Infrastructure.Services.ProjectData;

namespace RoyalMasion.Code.UI.Windows
{
    public class DailyMessagesPopUpWindow : WindowBase
    {
        public Action ReachedLastState;

        [SerializeField] private TextUIHandler _textHandler;
        [SerializeField] private GameObject _collectablePopUp;
        [SerializeField] private GameObject _messageArea;
        [SerializeField] private Button _nextBtn;
        [SerializeField] private Button _previouseBtn;
        [SerializeField] private Image _sprite;

        private List<MetaMessageStaticData> _messageStates;
        private bool _rewardClaimed;
        private int _currentState;
        private IStaticDataService _staticData;
        private IEconomyDataService _economyData;

        [Inject]
        public void Construct(IStaticDataService staticData, IEconomyDataService economyData)
        {
            _staticData = staticData;
            _economyData = economyData;
        }

        private void Start()
        {
            Subscribe();
        }
        private void Subscribe()
        {
            _nextBtn.onClick.AddListener(SetNextState);
            _previouseBtn.onClick.AddListener(SetPreviouseState);
        }
        private void Unsubscribe()
        {
            _nextBtn.onClick.RemoveListener(SetNextState);
            _previouseBtn.onClick.RemoveListener(SetPreviouseState);
        }

        public void SetPopUpWindow(List<MetaMessageStaticData> targetStates, bool rewardClaimed)
        {
            _messageStates = targetStates;
            _rewardClaimed = rewardClaimed;
            if (_messageStates.Count == 0)
                CloseWindow();
            _currentState = 0;
            SetMessage();
        }

        private void SetNextState()
        {
            if (_currentState >= _messageStates.Count - 1)
            {
                if (!_rewardClaimed)
                    HandleLastState();
                else
                    CloseWindow();
                return;
            }
            _currentState++;
            SetMessage();
        }

        private void HandleLastState()
        {
            _economyData.SetEconomyData(
                _staticData.GameData.PlaytestStaticData.DailyMessagesRewardType, 
                _staticData.GameData.PlaytestStaticData.DailyMessagesRewardAmount);
            _collectablePopUp.SetActive(true);
            Sprite icon = _staticData.GameData.EconomyStaticData.GetIcon(
                _staticData.GameData.PlaytestStaticData.DailyMessagesRewardType);
            _collectablePopUp.GetComponent<CollectablePopUpWindow>().SetIcon(icon);
            _collectablePopUp.GetComponent<CollectablePopUpWindow>().SetParentObject(this);
            ReachedLastState?.Invoke();
            _messageArea.SetActive(false);
        }

        private void SetPreviouseState()
        {
            if (_currentState <= 0)
                return;
            _currentState--;
            SetMessage();
        }

        private void SetMessage()
        {
            MetaMessageStaticData messageData = _messageStates[_currentState]; 
            _textHandler.SetTextField(messageData.Text);
            _sprite.sprite = messageData.CharacterSprite;
        }

        private void OnDestroy()
        {
            Unsubscribe();
        }

    }
}
