using RoyalMasion.Code.Infrastructure.Data;
using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using RoyalMansion.Code.UnityLogic.Catalog;

namespace RoyalMansion.Code.UI.Windows.Catalog
{
    public class CatalogItemUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _name;
        [SerializeField] private TextMeshProUGUI _price;
        [SerializeField] private Image _icon;

        public Action<CatalogItemStaticData> TryPurchase;

        private CatalogItemStaticData _itemData;
        private void Start()
        {
            Subscribe();
        }

        public void SetItemData(CatalogItemStaticData data)
        {
            _itemData = data;
            _name.text = _itemData.Name;
            _price.text = _itemData.Price.ToString();
            _icon.sprite = _itemData.Icon;
        }

        private void Subscribe()
        {
            GetComponent<Button>().onClick.AddListener(OnTryBuy);
        }
        private void Unsubscribe()
        {
            GetComponent<Button>().onClick.RemoveListener(OnTryBuy);
        }

        private void OnTryBuy()
        {
            TryPurchase?.Invoke(_itemData);
        }

        private void OnDestroy()
        {
            Unsubscribe();
        }
    }

}
