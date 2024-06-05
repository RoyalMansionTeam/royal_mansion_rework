using RoyalMasion.Code.Infrastructure.Data;
using System.Collections.Generic;
using UnityEngine;

namespace RoyalMansion.Code.UnityLogic.Catalog
{
    [CreateAssetMenu(fileName = "UnitCatalogConfig", menuName = "Static Data/Catalog/CatalogCondig")]
    public class CatalogConfig : ScriptableObject
    {
        [SerializeField] public UnitType UnitType;
        [SerializeField] private List<CatalogDataType> _catalogData;

        public Dictionary<CatalogSection, List<CatalogItemStaticData>> CatalogData;
        public Dictionary<CatalogSection, List<CatalogItemStaticData>> BathroomCatalogData;

        private Dictionary<CatalogSection, List<CatalogItemStaticData>> _targetDict;

        private void OnValidate()
        {
            CatalogData = new();
            BathroomCatalogData = new();
            foreach (var sectionData in _catalogData)
            {
                if (sectionData.ApartmentArea!=ApartmentAreaType.Bathroom)
                    _targetDict = CatalogData;
                else
                    _targetDict = BathroomCatalogData;

                if (_targetDict.ContainsKey(sectionData.Section))
                    _targetDict[sectionData.Section].AddRange(sectionData.Items);
                else
                    _targetDict.Add(sectionData.Section, 
                        new List<CatalogItemStaticData>(sectionData.Items));
            }
        }
    }

    [System.Serializable]
    public class CatalogDataType
    {
        public CatalogSection Section;
        public ApartmentAreaType ApartmentArea;
        public List<CatalogItemStaticData> Items;
    }

}
