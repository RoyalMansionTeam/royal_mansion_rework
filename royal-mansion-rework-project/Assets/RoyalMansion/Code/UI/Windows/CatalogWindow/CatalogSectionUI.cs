using RoyalMasion.Code.Infrastructure.Data;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace RoyalMansion.Code.UI.Windows.Catalog
{
    public class CatalogSectionUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _sectionName;

        public Action<CatalogSection> PickSection;

        private CatalogSection _section;
        private void Start()
        {
            Subscribe();
        }

        public void SetSection(CatalogSection targetSection)
        {
            _section = targetSection;
            _sectionName.text = targetSection.ToString();
        }

        private void Subscribe()
        {
            GetComponent<Button>().onClick.AddListener(OnPickSection);
        }
        private void Unsubscribe()
        {
            GetComponent<Button>().onClick.RemoveListener(OnPickSection);
        }

        private void OnPickSection()
        {
            PickSection?.Invoke(_section);
        }

        private void OnDestroy()
        {
            Unsubscribe();
        }

    }

}
