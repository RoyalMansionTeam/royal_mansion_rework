namespace RoyalMasion.Code.Infrastructure.Saving
{
    [System.Serializable]
    public class MansionUnitSaveData : MansionSaveableCategory
    {
        public int UnitStateID;
        public int TaskUnitID;
        public MansionUnitSaveData (string uSID, int unitState, int taskUnit)
        {
            UniqueSaveID = uSID;
            UnitStateID = unitState;
            TaskUnitID = taskUnit;
        }
    }
}
