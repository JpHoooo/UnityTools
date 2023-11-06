public static class MathfExtension 
{
    public static float Remap(float value, float inMin,float inMax,float outMin,float outMax){
        return ((outMax - outMin)/(inMax-inMin) *(value - inMin)) + outMin ;
    }    
}
