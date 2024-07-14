using RoyalMasion.Code.Extensions;
using RoyalMasion.Code.Extensions.Utils;
using RoyalMasion.Code.Infrastructure.Saving;
using RoyalMasion.Code.Infrastructure.Services.SaveLoadService;
using System;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace RoyalMansion.Code.UnityLogic.Catalog
{
    public class UnitItem : MonoBehaviour, ISaveWriter, ISaveReader
    {
        public string SaveableID { get; set; }

        private string _unitID;
        private string _assetReference;

        private void Start()
        {
            if (SaveableID == null)
                SaveableID = GetComponent<UniqueId>().GenerateId();
        }

        public void SetItemData(string unitID, string assetReference)
        {
            _unitID = unitID;
            _assetReference = assetReference;
        }

        public void LoadProgress(GameProgress progress)
        {

        }

        public void SaveProgress(GameProgress progress)
        {
            progress.MansionProgress.TryAddItem(
                new CatalogItemSaveData(
                    position: transform.localPosition.AsVectorData(),
                    unitID: _unitID,
                    assetGUID: _assetReference,
                    uSID: SaveableID
                    )
                );
        }
    }
}
