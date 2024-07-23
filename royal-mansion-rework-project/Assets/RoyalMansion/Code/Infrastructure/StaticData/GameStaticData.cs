using UnityEngine;
using UnityEngine.AddressableAssets;

namespace RoyalMasion.Code.Infrastructure.StaticData
{
    [CreateAssetMenu(fileName = "GameStaticData", menuName = "Static Data/GameStaticData")]
    public class GameStaticData : ScriptableObject
    {
        [SerializeField] private WindowConfig[] _windowStaticData;
        [SerializeField] private MansionStaticData _mansionStaticData;
        [SerializeField] private PlaytestStaticData _playtestStaticData;
        [SerializeField] private EconomyStaticData _economyStaticData;

        public WindowConfig[] Windows => _windowStaticData;
        public MansionStaticData MansionStaticData => _mansionStaticData;
        public PlaytestStaticData PlaytestStaticData => _playtestStaticData;
        public EconomyStaticData EconomyStaticData => _economyStaticData;
    }
}