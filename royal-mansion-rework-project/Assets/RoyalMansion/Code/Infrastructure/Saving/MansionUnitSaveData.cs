namespace RoyalMasion.Code.Infrastructure.Saving
{
    [System.Serializable]
    public class MansionUnitSaveData : MansionSaveableCategory
    {
        public int UnitStateID;
        public int TaskUnitID;
        public string BathroomWallID;
        public string BedroomWallID;
        public string BathroomFloorID;
        public string BedroomFloorID;
        public MansionUnitSaveData(string uSID, int unitState, int taskUnit, 
            string bathroomWallID, string bedroomWallID, string bathroomFloorID, string bedroomFloorID)
        {
            UniqueSaveID = uSID;
            UnitStateID = unitState;
            TaskUnitID = taskUnit;
            BathroomWallID = bathroomWallID;
            BedroomWallID = bedroomWallID;
            BathroomFloorID = bathroomFloorID;
            BedroomFloorID = bedroomFloorID;
        }
    }
}
