using RoyalMasion.Code.Infrastructure.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace RoyalMansion.Code.UnityLogic.Catalog
{
    [CreateAssetMenu(fileName = "Unit_item_0", menuName = "Static Data/Catalog/CatalogItemStaticData")]
    public class CatalogItemStaticData : ScriptableObject
    {
        [SerializeField] public CatalogSection ItemSection;
        [SerializeField] public string Name;
        [SerializeField] public int Price;
        [SerializeField] public AssetReference PrefabAssetReference;
        [SerializeField] public Sprite Icon;
    }

}
