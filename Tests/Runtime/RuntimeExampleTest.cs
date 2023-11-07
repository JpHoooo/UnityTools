using System;
using System.Collections;
using Jphoooo.Tools;
using UnityEngine;
public class RuntimeExampleTest : MonoBehaviour
{   
    private GraphDrawer drawer ;
    float data;

    private void Start() {

        GraphConfig config = new GraphConfig()
        {
           MaxVisiableCount = 20,
           graphScaleMultiplier = 1f,
           dotSizeMultiplier = 1.2f,
           alignment = Alignment.MiddleCenter,
           YMinMax = new Vector2(2,-2)           
        };

        drawer =new GraphDrawer(config);
        Function.Periodic(InputData,0.1f) ;
        Debug.Log("Fibonacci:"+MathfExtension.Fibonacci(5));
        Debug.Log("RichText Test".WithColor(Color.red));
     
    }

    void InputData()
    {
        data += Mathf.Deg2Rad * 20;
        drawer.AddData(Mathf.Sin(data));
    }
}
