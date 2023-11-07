using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
namespace Jphoooo.Tools{
    public class Function
    {
        private static GameObject monohook = null;

        public static Function Periodic(Action action,float duration){
            InitMonohook();
            Function function = new Function(action, duration);
            // monohook.GetComponent<MonoBehaviourHook>().perioidcAction = action;
            // monohook.GetComponent<MonoBehaviourHook>().duration = duration;
            monohook.GetComponent<MonoBehaviourHook>().StartFunction(action, duration);
            return function;
        }

        private static void InitMonohook(){
            if(monohook == null){
                monohook = new GameObject("FunctionMonohook", typeof(MonoBehaviourHook));
            }
        }

        Action action;
        float duration;

        public Function(Action action,float duration){
            this.action = action;
            this.duration = duration;
        }


        private class MonoBehaviourHook:MonoBehaviour{
 
            public void StartFunction(Action perioidcAction, float duration){
                StartCoroutine(Counter(perioidcAction,duration));
            }
            
             IEnumerator Counter(Action action, float duration)
            {
                action?.Invoke();
                yield return new WaitForSeconds(duration);      
                StartCoroutine(Counter(action, duration));
            }
        }

    }
}

