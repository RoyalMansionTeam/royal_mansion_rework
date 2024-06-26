using UnityEngine;
using UnityEngine.Events;

namespace RoyalMasion.Code.Editor
{
    [RequireComponent(typeof(Collider))]
    public class TriggerEvent : MonoBehaviour
    {
        [System.Serializable]
        public class TriggerEnteredEvent : UnityEvent { }

        public TriggerEnteredEvent _onTriggerEntered = new();

        private void OnTriggerEnter(Collider other)
        {
            _onTriggerEntered?.Invoke(); 
        }
    }

}
