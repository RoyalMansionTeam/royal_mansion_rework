using UnityEngine.Events;

namespace RoyalMasion.Code.Extensions
{
    public class TriggerEvent
    {
        [System.Serializable]
        public class MyEvent : UnityEvent<int> { }

        public MyEvent onClick;
    }

}
