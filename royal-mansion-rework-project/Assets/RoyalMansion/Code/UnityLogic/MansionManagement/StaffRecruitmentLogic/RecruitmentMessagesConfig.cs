using RoyalMasion.Code.Infrastructure.Data;
using System.Collections.Generic;
using UnityEngine;

namespace RoyalMasion.Code.UnityLogic.MasionManagement.StaffRecruitmentLogic
{
    [CreateAssetMenu(fileName = "RecruitmentMessagesConfig", menuName = "Static Data/RecruitmentMessagesConfig")]
    public class RecruitmentMessagesConfig : ScriptableObject
    {
        [SerializeField] public List<RecruitmentMessageData> MessageDatas;      
    }

    [System.Serializable]
    public class RecruitmentMessageData
    {
        [SerializeField] public NpcType Type;
        [TextArea(4, 5)] public string Message;
        [SerializeField] public Sprite Icon;
    }
}