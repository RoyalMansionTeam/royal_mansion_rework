using RoyalMasion.Code.Extensions;

namespace RoyalMasion.Code.Infrastructure.Saving
{
    [System.Serializable]
    public class DailyMessagesSaveData : MansionSaveableCategory
    {
        public bool SequenceRead;
        public string ClaimedDateTime;
        public DailyMessagesSaveData(string uniqueSaveID, bool seuenceRead, string claimedDateTime)
        {
            UniqueSaveID = uniqueSaveID;
            SequenceRead = seuenceRead;
            ClaimedDateTime = claimedDateTime;
        }
    }
}