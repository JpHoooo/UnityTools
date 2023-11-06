using System;
using System.Collections;
using UnityEngine;
using Jphoooo.DataVisualization;
public class Tester : MonoBehaviour
{
    private GraphDrawer drawer ;
    float data;

    private void Start() {

        //GraphConfig config = new GraphConfig()
        //{
        //    MaxVisiableCount = 20,
        //    graphScaleMultiplier = 1f,
        //    dotSizeMultiplier = 1.2f,
        //    alignment = Alignment.MiddleCenter,
        //    YMinMax = new Vector2(2,-2)           
        //};


        GraphConfig config = new GraphConfig();
        drawer =new GraphDrawer(config);
        FunctionPeriodic(InputData, 0.1f);
    }

    void InputData()
    {
        data += Mathf.Deg2Rad * 20;
        drawer.AddData(Mathf.Sin(data));
    }

    void FunctionPeriodic(Action action, float duration)
    {
        StartCoroutine(Counter(action, duration));
    }

    IEnumerator Counter(Action action, float duration)
    {
        action?.Invoke();
        yield return new WaitForSeconds(duration);      
        StartCoroutine(Counter(action, duration));
    }
}
