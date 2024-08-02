using RoyalMansion.Code.UnityLogic.NPC;
using RoyalMasion.Code.Infrastructure.Data;
using RoyalMasion.Code.Infrastructure.Services.AssetProvider;
using RoyalMasion.Code.Infrastructure.Services.SceneContext;
using RoyalMasion.Code.Infrastructure.Services.UIFactory;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using VContainer;

namespace RoyalMasion.Code.UnityLogic.MasionManagement.StaffRecruitmentLogic
{
    public class MaidService : MonoBehaviour
    {
        [SerializeField] private Transform _maidRestRoom;
        public Action<MaidNPC> AvailableMaidFound;

        private IAssetProvider _assetProvider;
        private ISceneContextService _sceneContext;

        private RecruitmentMessagesConfig _recruitmentConfig;
        private RecruitmentMessageData _maidMessageData;
        private List<MaidNPC> _maids = new();
        private MaidNPC _targetMaid;
        private Transform _lastCalledRoomPosition;

        [Inject]
        public void Construct(IAssetProvider assetProvider, ISceneContextService sceneContext)
        {
            _assetProvider = assetProvider;
            _sceneContext = sceneContext;
        }

        private void Start()
        {
            LoadRecruitmentMessage();
        }

        public void AddMaid(MaidNPC npc)
        {
            npc.AssignedUnitID = "MaidService";
            _maids.Add(npc);
        }

        public MaidNPC TryAssignMaid(Transform targetUnitPosition)
        {
            _lastCalledRoomPosition = targetUnitPosition;
            MaidNPC accessableMaid = null;
            if (_maids.Count != 0)
            {
                FindAvailableNPC(NpcType.Maid);
                accessableMaid = _targetMaid;
            }
            else
                PlaceRecruitmentWindow();
            return accessableMaid;
        }

        public void OnNpcTargetAchieved(NpcBase npc)
        {
            if (npc.AssignedUnitID != "MaidService")
                return;
            npc.gameObject.GetComponent<MaidNPC>().OnTargetAchieved();
        }

        public void SendMaidBack(MaidNPC targetMaid)
        {
            targetMaid.SetTarget(_maidRestRoom);
            targetMaid.EnterMaidBehaviour(NpcState.Resting);
        }

        private void PlaceRecruitmentWindow()
        {
            _sceneContext.StaffRecruitmentService.SpawnRecruitmentWindow(_maidMessageData);
            _sceneContext.StaffRecruitmentService.StaffRecruited += FindAvailableNPC;
        }

        private void FindAvailableNPC(NpcType calledType)
        {
            if (calledType != NpcType.Maid)
                return;
            _targetMaid = null;
            foreach (MaidNPC maid in _maids)
            {
                if (maid.CurrentBehavior.State == NpcState.PerformingTask)
                    continue;
                _targetMaid = maid;
                break;
            }
            if (_targetMaid != null)
            {
                SetMaidTask(_targetMaid);
                AvailableMaidFound?.Invoke(_targetMaid);
            }
        }
        private void SetMaidTask(MaidNPC maid)
        {
            maid.SetTarget(_lastCalledRoomPosition);
            maid.EnterMaidBehaviour(NpcState.PerformingTask);
        }


        private async void LoadRecruitmentMessage()
        {
            _recruitmentConfig = await _assetProvider.Load<RecruitmentMessagesConfig>
                            (AssetAddress.RECRUITMENT_MESSAGES_CONFIG);
            foreach (RecruitmentMessageData message in _recruitmentConfig.MessageDatas)
            {
                if (message.Type != NpcType.Maid)
                    continue;
                _maidMessageData = message;
                break;
            }
        }

    }
}