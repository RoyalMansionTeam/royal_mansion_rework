using RoyalMasion.Code.Infrastructure.Data;
using RoyalMasion.Code.Infrastructure.Services.ProjectData;
using RoyalMasion.Code.UI.Windows;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

namespace RoyalMansion.Code.UI.Windows.HUD
{
    public class MoneyFIeldUI : MonoBehaviour
    {
        private const ResourceType targetResourceType = ResourceType.SoftVallue;

        [SerializeField] private TextMeshProUGUI _textField;
        [SerializeField] private Button _addMoneyBtn;

        private IEconomyDataService _economyDataService;

        [Inject]
        public void Construct(IEconomyDataService economyDataService)
        {
            _economyDataService = economyDataService;
        }
        private void Start()
        {
            Subscribe();
            LoadData();
        }

        private void LoadData()
        {
            UpdateMoneyUI(
                resourceType: targetResourceType,
                amount: _economyDataService.GetEconomyData(targetResourceType));
        }

        private void Subscribe()
        {
            _addMoneyBtn.onClick.AddListener(OpenInAppWindow);
            _economyDataService.ResourceChanged += UpdateMoneyUI;
        }
        private void Unsubscribe()
        {
            _addMoneyBtn.onClick.RemoveListener(OpenInAppWindow);
            _economyDataService.ResourceChanged -= UpdateMoneyUI;
        }
        private void UpdateMoneyUI(ResourceType resourceType, int amount)
        {
            if (resourceType != targetResourceType)
                return;
            _textField.text = amount.ToString();
        }

        private void OpenInAppWindow()
        {
        }

        private void OnDestroy()
        {
            Unsubscribe();
        }

    }

}
