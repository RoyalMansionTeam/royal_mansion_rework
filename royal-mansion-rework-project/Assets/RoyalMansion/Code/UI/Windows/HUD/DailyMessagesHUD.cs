using RoyalMansion.Code.Extensions;
using RoyalMansion.Code.UnityLogic.Meta;
using RoyalMasion.Code.Infrastructure.Data;
using RoyalMasion.Code.Infrastructure.Services.SceneContext;
using RoyalMasion.Code.Infrastructure.Services.UIFactory;
using RoyalMasion.Code.UI.Windows;
using System;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

namespace RoyalMansion.Code.UI.Windows.HUD
{
    public class DailyMessagesHUD : MonoBehaviour
    {
        [SerializeField] private Button _openMessagesBtn;
        [SerializeField] private GameObject _unreadMessagesIcon;

        private IUIFactory _uiFactory;
        private DailyMessagesHandler _messageHandler;
        private ISceneContextService _sceneContext;
        private DailyMessageData _currentMessageSequence;
        private DailyMessagesPopUpWindow _popupWindow;

        [Inject]
        public void Construct(IUIFactory uiFactory, ISceneContextService sceneContext)
        {
            _uiFactory = uiFactory;
            _messageHandler = sceneContext.MetaMessagesHandler;
            _sceneContext = sceneContext;
        }

        private void Start()
        {
            Subscribe();
        }

        private void Subscribe()
        {
            _messageHandler.AvailableMessageFound += OnMessageFound;
            _openMessagesBtn.onClick.AddListener(TryOpenMessagePopUp);
        }

        private void OnMessageFound(DailyMessageData taregtSequenceData)
        {
            _currentMessageSequence = taregtSequenceData;
            if(!_currentMessageSequence.SequenceRead)
                _unreadMessagesIcon.SetActive(true);
        }
        private void TryOpenMessagePopUp()
        {
            if (_currentMessageSequence == null)
                return;
            _unreadMessagesIcon.SetActive(false);
            _popupWindow = _uiFactory.CreateWindow(WindowID.DailyMessagesPopUp).GetComponent<DailyMessagesPopUpWindow>();
            _popupWindow.SetPopUpWindow(_currentMessageSequence.Messages, _currentMessageSequence.SequenceRead);
            _popupWindow.ReachedLastState += OnReachedLastMessageState;
        }

        private void OnReachedLastMessageState()
        {
            if (_popupWindow == null)
                return;
            _currentMessageSequence.SequenceRead = true;
            _currentMessageSequence.ClaimedDateTime = (int)Epoch.CurrentRelativeTime(_sceneContext.DailyRewardService.CurrentEpoch.EpochStartDateTime.ToDateTime());
            _popupWindow.ReachedLastState -= OnReachedLastMessageState;
            _popupWindow = null;
        }

        private void Unsubscribe()
        {
            _messageHandler.AvailableMessageFound -= OnMessageFound;
            _openMessagesBtn.onClick.RemoveListener(TryOpenMessagePopUp);
        }
        private void OnDestroy()
        {
            Unsubscribe();
        }

    }

}
