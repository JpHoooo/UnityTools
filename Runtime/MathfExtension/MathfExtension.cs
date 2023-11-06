using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Jphoooo.Tools.MathfExtension{
    public static class MathfExtension 
    {
        public static float Remap(this float value,float inMin,float inMax,float outMin,float outMax){
            value = Mathf.Clamp(value, inMin, inMax);
            return (outMax - outMin) / (inMax - inMin) * (value - inMin) +outMin;
        }
        public static Vector2 Remap(this Vector2 value,Vector2 inMin,Vector2 inMax,Vector2 outMin,Vector2 outMax){
            return new Vector2(Remap(value.x, inMin.x, inMax.x, outMin.x, outMax.x), Remap(value.y, inMin.y, inMax.y, outMin.y, outMax.y));
         }
        public static Vector3 Remap(this Vector3 value,Vector3 inMin,Vector3 inMax,Vector3 outMin,Vector3 outMax){
            return new Vector3(Remap(value.x, inMin.x, inMax.x, outMin.x, outMax.x), Remap(value.y, inMin.y, inMax.y, outMin.y, outMax.y),Remap(value.z, inMin.z, inMax.z, outMin.z, outMax.z));
        }
    }
}
