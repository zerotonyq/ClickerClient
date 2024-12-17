using System;
using System.Collections;
using UnityEngine;

namespace Utils
{
    public class UpdateBehaviour : MonoBehaviour
    {
        private bool _isStopped;
        
        public void Execute(Action action)
        {
            StopAllCoroutines();
            _isStopped = false;
            StartCoroutine(Coroutine(action));
        }

        private IEnumerator Coroutine(Action action)
        {
            while (!_isStopped)
            {
                action?.Invoke();
                yield return new WaitForSeconds(0.1f);
            }
        }
        public void Stop() => _isStopped = true;
    }
}