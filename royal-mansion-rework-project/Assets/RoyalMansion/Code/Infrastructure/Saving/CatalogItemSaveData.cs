using RoyalMasion.Code.Extensions;

namespace RoyalMasion.Code.Infrastructure.Saving
{
    [System.Serializable]
    public class CatalogItemSaveData : MansionSaveableCategory
    {
        public string AssignedUnitID;
        public Vector3Data Position;
        public Vector3Data Rotation;
        public string AssetGUID;
        public int CatalogSectionID;

        public CatalogItemSaveData(Vector3Data position, string unitID,
            string assetGUID, string uSID, int catalogSectionID, Vector3Data rotation)
        {
            Position = position;
            AssignedUnitID = unitID;
            AssetGUID = assetGUID;
            UniqueSaveID = uSID;
            CatalogSectionID = catalogSectionID;
            Rotation = rotation;
        }
    }
}
