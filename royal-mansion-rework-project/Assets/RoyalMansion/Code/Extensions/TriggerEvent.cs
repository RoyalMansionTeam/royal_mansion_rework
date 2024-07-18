using RoyalMansion.Code.UnityLogic.NPC;
using UnityEngine;
using UnityEngine.Events;

namespace RoyalMasion.Code.Editor
{
    [RequireComponent(typeof(Collider))]
    public class TriggerEvent : MonoBehaviour
    {
        [System.Serializable]
        public class TriggerEnteredEvent : UnityEvent<NpcBase> { }

        [SerializeField] public TriggerEnteredEvent _onTriggerEntered = new();

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.TryGetComponent(out NpcBase npc))
            {
                _onTriggerEntered?.Invoke(npc);
            }
        }
    }

}
