using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Eraslank.Util
{
    public static class SuperInvokeUtil
    {
        public static Coroutine SuperInvoke(this MonoBehaviour m, UnityAction action)
        {
            return m.StartCoroutine(Countdown(action, 0));
        }
        public static Coroutine SuperInvoke(this MonoBehaviour m, UnityAction action, float delay)
        {
            return m.StartCoroutine(Countdown(action, delay));
        }
        public static Coroutine SuperInvoke(this MonoBehaviour m, UnityAction action, float delay, float repeatEvery)
        {
            return m.StartCoroutine(CountdownRepeatable((_)=> { action?.Invoke(); }, delay, repeatEvery, null));
        }

        public static Coroutine SuperInvoke(this MonoBehaviour m, UnityAction<int> action, float delay, float repeatEvery, int? repeatCount = null)
        {
            return m.StartCoroutine(CountdownRepeatable(action, delay, repeatEvery, repeatCount));
        }

        private static IEnumerator Countdown(UnityAction action, float delay)
        {
            yield return new WaitForSeconds(delay);
            action?.Invoke();
        }
        private static IEnumerator CountdownRepeatable(UnityAction<int> action, float delay, float repeatEvery, int? repeatCount)
        {
            yield return new WaitForSeconds(delay);

            var waiter = new WaitForSeconds(repeatEvery);
            int repeatedFor = 0;
            while(true)
            {
                if(repeatCount != null && repeatedFor>=repeatCount)
                {
                    yield break;
                }
                repeatedFor++;
                action?.Invoke(repeatedFor);
                yield return waiter;
            }
        }
    }

}
