using RoyalMansion.Code.UnityLogic.CameraLogic;
using RoyalMansion.Code.UnityLogic.ContextObjects.MansionContext;
using RoyalMansion.Code.UnityLogic.DailyRewards;
using RoyalMansion.Code.UnityLogic.Meta;
using RoyalMasion.Code.UnityLogic.MasionManagement;
using RoyalMasion.Code.UnityLogic.MasionManagement.KitchenGardenLogic;
using RoyalMasion.Code.UnityLogic.MasionManagement.StaffRecruitmentLogic;
using System;
using UnityEngine;
using VContainer;

namespace RoyalMasion.Code.Infrastructure.Services.SceneContext
{
    public class MansionSceneContext : MonoBehaviour
    {
        private ISceneContextService _contextService;
        [SerializeField] private MansionSpawnPointData _mansionSpawnPoints;
        [SerializeField] private Kitchen _kitchen;
        [SerializeField] private MaidService _maidService;
        [SerializeField] private MansionCinemachineHandler _cinemachineHandler;
        [SerializeField] private DailyMessagesHandler _dailyMessagesHandler;
        [SerializeField] private DailyRewardService _dailyRewardService;
        [SerializeField] private StaffRecruitmentService _recruitmentService;

        [Inject]
        public void Construct(ISceneContextService contextService)
        {
            _contextService = contextService;
            InitSceneContext();
        }

        private void InitSceneContext()
        {
            _contextService.MansionSpawnPoints = _mansionSpawnPoints;
            _contextService.Kitchen = _kitchen;
            _contextService.MaidService = _maidService;
            _contextService.CinemachineHandler = _cinemachineHandler;
            _contextService.MetaMessagesHandler = _dailyMessagesHandler;
            _contextService.DailyRewardService = _dailyRewardService;
            _contextService.StaffRecruitmentService = _recruitmentService;
        }
    }
}