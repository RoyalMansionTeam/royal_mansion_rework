using RoyalMansion.Code.UI.WorldspaceUI;
using RoyalMasion.Code.Infrastructure.Data;
using RoyalMasion.Code.Infrastructure.Services.ProjectData;
using RoyalMasion.Code.Infrastructure.StaticData;
using RoyalMasion.Code.UnityLogic.MasionManagement.StaffRecruitmentLogic;
using System;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

namespace RoyalMasion.Code.UI.Windows
{
    public class StaffRecruitmentWindow : WindowBase
    {
        public Action<NpcType> StaffMemberRecruited;

        [SerializeField] private Image _staffMemberIcon;
        [SerializeField] private Button _recruitBtn;
        [SerializeField] private TextUIHandler _messageText;
        [SerializeField] private TextUIHandler _priceText;

        private IEconomyDataService _economyData;

        private RecruitmentPriceData _priceData;

        [Inject]
        public void Construct(IEconomyDataService economyData)
        {
            _economyData = economyData;
        }

        private void Start()
        {
            Subscribe();
        }

        private void Subscribe()
        {
            _recruitBtn.onClick.AddListener(TryRecruit);
        }


        public void SetWindowData(RecruitmentMessageData messageData, RecruitmentPriceData priceData)
        {
            _priceData = priceData;
            _staffMemberIcon.sprite = messageData.Icon;
            _messageText.SetTextField(messageData.Message);
            _priceText.SetTextField(priceData.Amount.ToString());
        }
        private void TryRecruit()
        {
            if (_economyData.GetEconomyData(_priceData.ValueType) < _priceData.Amount)
                return;
            _economyData.SetEconomyData(_priceData.ValueType, (-1)*_priceData.Amount);
            StaffMemberRecruited?.Invoke(_priceData.StaffType);
            CloseWindow();
        }
        private void Unsubscribe()
        {
            _recruitBtn.onClick.RemoveListener(TryRecruit);
        }

        private void OnDestroy()
        {
            Unsubscribe();
        }

    }
}
