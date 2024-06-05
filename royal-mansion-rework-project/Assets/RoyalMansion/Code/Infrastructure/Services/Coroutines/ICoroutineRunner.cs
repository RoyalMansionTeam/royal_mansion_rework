using System.Collections;
using UnityEngine;

namespace RoyalMasion.Code.UnityLogic.Coroutines
{
    public interface ICoroutineRunner
    {
        Coroutine StartCoroutine(IEnumerator coroutine);
        void StopCoroutineSafe(Coroutine coroutine);
    }
}