using RoyalMansion.Code.UI.Windows.DailyRewardWindow;
using RoyalMansion.Code.UnityLogic.DailyRewards;
using RoyalMansion.Code.UnityLogic.Meta;
using RoyalMasion.Code.Infrastructure.Data;
using RoyalMasion.Code.Infrastructure.Services.SceneContext;
using RoyalMasion.Code.Infrastructure.Services.UIFactory;
using System;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

namespace RoyalMansion.Code.UI.Windows.HUD
{
    public class DailyRewardsHUD : MonoBehaviour
    {
        [SerializeField] private Button _openPopUpBtn;
        [SerializeField] private GameObject _unclaimedRewardsIcon;
        private IUIFactory _uiFactory;
        private DailyRewardService _rewardService;

        [Inject]
        public void Construct(IUIFactory uiFactory, ISceneContextService sceneContext)
        {
            _uiFactory = uiFactory;
            _rewardService = sceneContext.DailyRewardService;
        }
        private void Start()
        {
            Subscribe();
        }

        private void Subscribe()
        {
            _openPopUpBtn.onClick.AddListener(OpenPopUp);
            _rewardService.FoundUnclaimedReward += OnRewardFound;
        }

        private void OnRewardFound(bool isClaimable)
        {
            if (!isClaimable)
                return;
            _unclaimedRewardsIcon.SetActive(isClaimable);
        }

        private void OpenPopUp()
        {
            DailyRewardsWindow windowHandler = _uiFactory.CreateWindow(WindowID.DailyRewardsPopUp).
                GetComponent<DailyRewardsWindow>();
            windowHandler.SetRewardWindow(_rewardService.CurrentEpoch);
            windowHandler.RewardClaimed += HideUnclaimedMark;
        }

        private void HideUnclaimedMark() => 
            _unclaimedRewardsIcon.SetActive(false);

        private void Unsubscribe()
        {
            _openPopUpBtn.onClick.RemoveListener(OpenPopUp);
            _rewardService.FoundUnclaimedReward -= OnRewardFound;
        }

        private void OnDestroy()
        {
            Unsubscribe();
        }

    }

}
