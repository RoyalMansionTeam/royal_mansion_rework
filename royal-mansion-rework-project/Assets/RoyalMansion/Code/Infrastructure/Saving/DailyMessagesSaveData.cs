using RoyalMasion.Code.Extensions;

namespace RoyalMasion.Code.Infrastructure.Saving
{
    [System.Serializable]
    public class DailyMessagesSaveData : MansionSaveableCategory
    {
        public bool SequenceRead;
        public int ClaimedDateTime;
        public DailyMessagesSaveData(string uniqueSaveID, bool seuenceRead, int claimedDateTime)
        {
            UniqueSaveID = uniqueSaveID;
            SequenceRead = seuenceRead;
            ClaimedDateTime = claimedDateTime;
        }
    }
}