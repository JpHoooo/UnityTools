using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Jphoooo.Tools{
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

        /// <summary>
        /// Returns the angle in Degrees whose Tan is posA/posB
        /// </summary>
        /// <param name="posA"></param>
        /// <param name="posB"></param>
        /// <returns></returns>
        public static float GetDegrees(Vector2 posA,Vector2 posB){

            return Mathf.Atan2(posA.y - posB.y, posA.x - posB.x) * Mathf.Rad2Deg;  
        }

        public static Vector2 SameVector2(float value){
            return new Vector2(value, value);
        }
        public static Vector3 SameVector3(float value){
            return new Vector3(value, value, value);
        }
        public static Vector4 SameVector4(float value){
            return new Vector4(value, value,value,value);
        }
    }
}

