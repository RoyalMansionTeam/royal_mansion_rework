using RoyalMasion.Code.Extensions;

namespace RoyalMasion.Code.Infrastructure.Saving
{
    [System.Serializable]
    public class CatalogItemSaveData : MansionSaveableCategory
    {
        public string AssignedUnitID;
        public Vector3Data Position;
        public string AssetGUID;

        public CatalogItemSaveData(Vector3Data position, string unitID, string assetGUID, string uSID)
        {
            Position = position;
            AssignedUnitID = unitID;
            AssetGUID = assetGUID;
            UniqueSaveID = uSID;
        }
    }
}
