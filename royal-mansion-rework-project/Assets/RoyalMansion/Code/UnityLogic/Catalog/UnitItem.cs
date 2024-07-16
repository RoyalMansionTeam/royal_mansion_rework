using RoyalMasion.Code.Extensions;
using RoyalMasion.Code.Extensions.Utils;
using RoyalMasion.Code.Infrastructure.Data;
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

        private CatalogSection _itemSection;
        private string _unitID;
        private string _assetReference;

        private void Start()
        {
            if (SaveableID == null)
                SaveableID = GetComponent<UniqueId>().GenerateId();
        }

        public void SetItemData(string unitID, string assetReference, CatalogSection itemSection)
        {
            _unitID = unitID;
            _assetReference = assetReference;
            _itemSection = itemSection;
        }

        public void Rotate()
        {
            transform.rotation *= Quaternion.Euler(0f, 90f, 0f);
        }

        public void LoadProgress(GameProgress progress)
        {

        }

        public void SaveProgress(GameProgress progress)
        {
            if (this == null)
                return;
            progress.MansionProgress.TryAddItem(
                new CatalogItemSaveData(
                    position: transform.localPosition.AsVectorData(),
                    unitID: _unitID,
                    assetGUID: _assetReference,
                    uSID: SaveableID,
                    catalogSectionID: _itemSection.GetHashCode(),
                    rotation: transform.rotation.eulerAngles.AsVectorData()
                    )
                );
        }
    }
}
