using UnityEngine;

namespace RoyalMasion.Code.UnityLogic.MasionManagement
{
    public class MansionUnitTimer : MonoBehaviour
    {
        private bool _isTimeUp;

        public bool IsTimeUp => _isTimeUp;
        private void Start()
        {
            //start timer. update on Update
        }
    }
}
