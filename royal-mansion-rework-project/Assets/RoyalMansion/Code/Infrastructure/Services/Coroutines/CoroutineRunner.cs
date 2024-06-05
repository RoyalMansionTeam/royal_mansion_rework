using UnityEngine;

namespace RoyalMasion.Code.UnityLogic.Coroutines
{
    public class CoroutineRunner : MonoBehaviour, ICoroutineRunner
    {
        public void StopCoroutineSafe(Coroutine coroutine)
        {
            if (this != null && coroutine is not null) StopCoroutine(coroutine);
        }
    }
}