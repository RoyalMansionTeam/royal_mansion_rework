namespace RoyalMasion.Code.Infrastructure.Saving
{
    [System.Serializable]
    public class TimerSaveData : MansionSaveableCategory
    {
        public float TaskEndRealtime;

        public TimerSaveData(string uniqueSaveID, float taskEndRealtime)
        {
            UniqueSaveID = uniqueSaveID;
            TaskEndRealtime = taskEndRealtime;
        }
    }
}
