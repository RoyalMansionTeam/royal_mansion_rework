using RoyalMansion.Code.UnityLogic.CameraLogic;
using RoyalMansion.Code.UnityLogic.ContextObjects.MansionContext;
using RoyalMansion.Code.UnityLogic.Meta;
using RoyalMasion.Code.UnityLogic.MasionManagement.KitchenGardenLogic;
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
        [SerializeField] private MansionCinemachineHandler _cinemachineHandler;
        [SerializeField] private DailyMessagesHandler _dailyMessagesHandler;

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
            _contextService.CinemachineHandler = _cinemachineHandler;
            _contextService.MetaMessagesHandler = _dailyMessagesHandler;
        }
    }
}