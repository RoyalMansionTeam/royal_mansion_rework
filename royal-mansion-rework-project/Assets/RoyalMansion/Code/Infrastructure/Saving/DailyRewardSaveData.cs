namespace RoyalMasion.Code.Infrastructure.Saving
{
    [System.Serializable]
    public class DailyRewardSaveData : MansionSaveableCategory
    {
        public int ClaimedDateTime;

        public DailyRewardSaveData(string uniqueSaveID, int claimedDateTime)
        {
            UniqueSaveID = uniqueSaveID;
            ClaimedDateTime = claimedDateTime;
        }
    }
}
