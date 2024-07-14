using RoyalMasion.Code.Extensions;

namespace RoyalMasion.Code.Infrastructure.Saving
{
    [System.Serializable]
    public class NpcSaveData : MansionSaveableCategory
    {
        public Vector3Data Position;
        public string AssignedUnitID;

        public NpcSaveData(string uniqueSaveID, Vector3Data position, string assignedUnitID)
        {
            UniqueSaveID = uniqueSaveID;
            Position = position;
            AssignedUnitID = assignedUnitID;
        }

    }
}