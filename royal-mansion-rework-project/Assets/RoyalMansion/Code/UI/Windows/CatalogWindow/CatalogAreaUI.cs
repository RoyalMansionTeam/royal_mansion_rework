using RoyalMasion.Code.Infrastructure.Data;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace RoyalMansion.Code.UI.Windows.Catalog
{
    public class CatalogAreaUI : MonoBehaviour
    {
        [SerializeField] private ApartmentAreaType _area;

        public Action<ApartmentAreaType> PickArea;

        private void Start()
        {
            Subscribe();
        }

        private void Subscribe()
        {
            GetComponent<Button>().onClick.AddListener(OnPickArea);
        }
        private void Unsubscribe()
        {
            GetComponent<Button>().onClick.RemoveListener(OnPickArea);
        }

        private void OnPickArea()
        {
            PickArea?.Invoke(_area);
        }

        private void OnDestroy()
        {
            Unsubscribe();
        }
    }

}
