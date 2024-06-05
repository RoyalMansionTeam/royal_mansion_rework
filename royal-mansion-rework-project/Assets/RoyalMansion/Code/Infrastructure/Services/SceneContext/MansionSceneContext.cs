using RoyalMansion.Code.UnityLogic.ContextObjects.MansionContext;
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
        }
    }
}