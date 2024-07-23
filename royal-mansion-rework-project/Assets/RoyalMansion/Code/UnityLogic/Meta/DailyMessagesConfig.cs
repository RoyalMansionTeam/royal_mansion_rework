using System.Collections.Generic;
using UnityEngine;

namespace RoyalMansion.Code.UnityLogic.Meta
{
    [CreateAssetMenu(fileName = "MetaMessagesConfig",
        menuName = "Static Data/Meta/Daily Messages/Meta Messages Config")]
    public class DailyMessagesConfig : ScriptableObject
    {
        [SerializeField] public List<DailyMessageData> DailyMessages;
    }

    [System.Serializable]
    public class DailyMessageData
    {
        [SerializeField] public string SequenceID;
        [SerializeField] public List<MetaMessageStaticData> Messages;
        [SerializeField] public bool SequenceRead;
        [SerializeField] public string ClaimedDateTime;
    }
}
