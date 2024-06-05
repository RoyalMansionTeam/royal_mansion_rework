using UnityEngine;

namespace RoyalMasion.Code.Infrastructure.StaticData
{
    [CreateAssetMenu(fileName = "GameStaticData", menuName = "Static Data/GameStaticData")]
    public class GameStaticData : ScriptableObject
    {
        [SerializeField] private WindowConfig[] _windowStaticData;
        [SerializeField] private MansionStaticData _mansionStaticData;

        public WindowConfig[] Windows => _windowStaticData;
        public MansionStaticData MansionStaticData => _mansionStaticData;
    }

}