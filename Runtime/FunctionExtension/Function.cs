using System;
using System.Collections;
using UnityEngine;
namespace Jphoooo.Tools{
    public static class Function
    {
        public static void Periodic(this MonoBehaviour mono,Action action,float duration){
            mono.StartCoroutine(Counter(mono, action,duration));
       }
        static IEnumerator Counter(this MonoBehaviour mono,Action action, float duration)
        {
            action?.Invoke();
            yield return new WaitForSeconds(duration);      
            mono.StartCoroutine(Counter(mono,action, duration));
        }
    }
}

