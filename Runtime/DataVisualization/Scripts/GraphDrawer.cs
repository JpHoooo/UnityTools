using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace Jphoooo.Tools
{
    public class GraphDrawer : MonoBehaviour
    {

        [SerializeField] Sprite dotSprite;

        private Transform parent;

        RectTransform windowGraph, graphContainer;

        // 图标宽高
        float graphHeight, graphWidth;

        float xPositionNormalized;

        YInfo yInfo = new YInfo(0,0);

        GameObject templateX,templateY;

    
        int maxVisiableCount = 20;
        int xIndex = 0;

        float dotSizeMultiplier, dotConnectionSizeMultiplier = 1 ;
 
        private Queue<float> valueQueue = new Queue<float>();
        private List<TemplateX> templateXList;
        private List<TemplateY> templateYList;

        GraphConfig graphConfig;

        bool refreshY = true;


        public GraphDrawer(GraphConfig graphConfig)
        {

            this.graphConfig = graphConfig;

          

            maxVisiableCount = graphConfig.MaxVisiableCount < 1 ? 20: graphConfig.MaxVisiableCount;
            parent = Instantiate((GameObject)Resources.Load("GraphCanvas")).transform;
            parent.name = "GraphCanvas";           


            windowGraph = parent.Find("Window_graph") as RectTransform;
            windowGraph.localScale *= graphConfig.graphScaleMultiplier;

            graphConfig.SetAlignment(windowGraph);


            dotSizeMultiplier = graphConfig.dotSizeMultiplier;
            dotConnectionSizeMultiplier = graphConfig.dotConnectionSizeMultiplier;


            graphContainer = windowGraph.Find("graphContainer") as RectTransform;
            templateX = graphContainer.Find("TemplateX").gameObject;
            templateY = graphContainer.Find("TemplateY").gameObject;

            graphHeight = graphContainer.sizeDelta.y;
            graphWidth = graphContainer.sizeDelta.x;

            xPositionNormalized = graphWidth / (maxVisiableCount + 1) < 60 ? graphWidth / (maxVisiableCount + 1) : 60;

            templateYList =  DrawTemplateY();
            templateXList = DrawTemplateX();

            refreshY = graphConfig.YMinMax == Vector2.zero;
            if (!refreshY)
            {
                Vector2 minmax = graphConfig.YMinMax.x > graphConfig.YMinMax.y ? new Vector2(graphConfig.YMinMax.y, graphConfig.YMinMax.x) : graphConfig.YMinMax;
                yInfo.minimum = minmax.x;
                yInfo.maximum = minmax.y;
                RefreshLabelY(yInfo);
            }
        }

        public void AddData(float value)
        {
            valueQueue.Enqueue(value);

            if (valueQueue.Count > maxVisiableCount)
            {
                valueQueue.Dequeue();
            }

    
            Refresh(value);             

            int i = 0;

            Vector2 lastPosition = Vector2.zero;
        
            foreach( var _value in valueQueue)
            {
                templateXList[i].dot.SetActive(true);
                Vector2 currentPosition = templateXList[i].dot.GetComponent<RectTransform>().anchoredPosition = GetDotPositionByValue(i,_value);

                if (i > 0)
                {
                    templateXList[i - 1].dotConnection.SetActive(true);
                    float dist = Vector2.Distance(lastPosition, currentPosition);
                    Vector2 direc = (currentPosition - lastPosition).normalized;
                    RectTransform rectTransform = templateXList[i - 1].dotConnection.GetComponent<RectTransform>();

                    rectTransform.anchorMin = Vector2.zero;
                    rectTransform.anchorMax = Vector2.zero;
                    rectTransform.sizeDelta = new Vector2(dist, 5* dotConnectionSizeMultiplier);

                    // float angle = Mathf.Atan2(currentPosition.y - lastPosition.y, currentPosition.x - lastPosition.x);                    

                    // angle = Mathf.Rad2Deg * angle;
                    rectTransform.anchoredPosition = lastPosition + (direc * dist * .5f);
                    rectTransform.localEulerAngles = new Vector3(0, 0, MathfExtension.GetDegrees(currentPosition,lastPosition));
                }

                lastPosition = currentPosition;
                //Debug.Log($"valueQueue.Count:{valueQueue.Count} templateXList.Count:{templateXList.Count} i:{i} value:{_value}");
                i++;
            }
            //Debug.Log("value:" + value + " index:" + xIndex);

            xIndex++;
        }

        Vector2 GetDotPositionByValue(int index,float value)
        {
            return new Vector2
                (xPositionNormalized + index * xPositionNormalized,
                 MathfExtension.Remap(value, yInfo.displayMinimum, yInfo.displayMaximum, 0, graphHeight));
        }

        List<TemplateX> DrawTemplateX()
        {

            List <TemplateX> templateXes = new List<TemplateX>();
            templateXes.Clear();

            for (int i = 0; i <= maxVisiableCount; i++)
            {
                TemplateX templateX = new TemplateX(Instantiate(this.templateX));
                templateX.parent.transform.SetParent(graphContainer, false);
                templateX.parent.name = "TemplateX " + i;
                templateX.parent.SetActive(true);


                templateXes.Add(templateX);

                
                float xPosition = xPositionNormalized + i * xPositionNormalized;

                // Dash X
                RectTransform dashX = templateX.dash.GetComponent<RectTransform>();
                dashX.name = $"dashX - {i}";
                dashX.gameObject.SetActive(true);
                dashX.anchoredPosition = new Vector2(xPosition, 0);

                // Label X
                RectTransform labelX = templateX.label.GetComponent<RectTransform>();
                labelX.name = $"label X - {i}";
                labelX.gameObject.SetActive(true);
                labelX.anchoredPosition = new Vector2(xPosition, -25);
                labelX.sizeDelta = new Vector2((graphWidth-(2 * xPositionNormalized))/(maxVisiableCount+1),labelX.sizeDelta.y);
                labelX.GetComponent<Text>().text = i.ToString();

                // Dot & DotConnection
                templateX.dot.SetActive(false);
                templateX.dot.GetComponent<RectTransform>().sizeDelta *= dotSizeMultiplier;
                templateX.dotConnection.SetActive(false);

            }

            return templateXes;
        }




        List<TemplateY> DrawTemplateY()
        {

            List<TemplateY> templateYs = new List<TemplateY>();
            templateYs.Clear();

            int separatorCount = 10;
            for (int i = 0; i <= separatorCount; i++)
            {
                TemplateY templateY = new TemplateY(Instantiate(this.templateY));
                templateY.parent.transform.SetParent(graphContainer, false);
                templateY.parent.name = "TemplateY " + i;
                templateY.parent.gameObject.SetActive(true);
                templateYs.Add(templateY);
                float normalizedValue = 1.0f / separatorCount * i;

                // Dash Y
                RectTransform dashY = templateY.dash.GetComponent<RectTransform>();
                dashY.name = $"dashY - {i}";
                dashY.gameObject.SetActive(true);
                dashY.anchoredPosition = new Vector2(0, normalizedValue * graphHeight);

                float alpha = MathfExtension.Remap(i, 0, separatorCount, 30, 150);

                dashY.GetComponent<Image>().color = new Color(0, 1, 0, Mathf.Sin(alpha* Mathf.Deg2Rad) - 0.4f);



                // Label Y
                RectTransform labelY = templateY.label.GetComponent<RectTransform>();
                labelY.name = $"labelY - {i}";
                labelY.gameObject.SetActive(true);
                labelY.anchoredPosition = new Vector2(-30f, normalizedValue * graphHeight);
            }

            //templateYs[5].dash.GetComponent<Image>().color =Color.green;

            return templateYs;
        }


        #region When graph get a new data, Refresh.
        void Refresh(float value)
        {
            if(refreshY)
                RefreshLabelY( RefreshYInfoByValue(value));

            RefreshLabelX();
        }

        void RefreshLabelY(YInfo yInfo)
        {
            // Debug.Log($"yInfo.different:{ yInfo.different} yInfo.displayMinimum:{yInfo.displayMinimum}  yInfo.displayMaximum:{yInfo.displayMaximum} ");

            for (int i = 0; i < templateYList.Count; i++)
            {
                string str = MathfExtension.Remap(i, 0, templateYList.Count - 1, yInfo.displayMinimum, yInfo.displayMaximum).ToString("0.0");

                // Debug.Log($"str: {str}");

                templateYList[i].label.GetComponent<Text>().text = str;
            }
        }

        YInfo RefreshYInfoByValue(float value)
        {

            if (valueQueue.Count == 1)
            {
                yInfo.maximum = yInfo.minimum = valueQueue.First();
            }
            else
            {
                if (value > yInfo.maximum)
                {
                    yInfo.maximum = value;
                }
                if (value < yInfo.minimum)
                {
                    yInfo.minimum = value;
                }
            }
            return yInfo;
        }


        void RefreshLabelX()
        {
            if (xIndex > maxVisiableCount)

                for (int i = 0; i <= maxVisiableCount; i++)
                {
                    templateXList[i].label.GetComponent<Text>().text = (xIndex - (maxVisiableCount - i)).ToString();
                }
        }

        #endregion

        /*
        private class BarGraphVisual
        {
            private RectTransform graphContainer;
            private Color barColor;
            private float barWidthMultiplier;
            public BarGraphVisual(RectTransform graphContainer, Color barColor, float barWidthMultiplier)
            {
                this.graphContainer = graphContainer;
                this.barColor = barColor;
                this.barWidthMultiplier = barWidthMultiplier;
            }

            public List<GameObject> AddGraphVisual(Vector2 graphPosition)
            {
                return new List<GameObject> { CreatBar(graphPosition) };
            }

            private GameObject CreatBar(Vector2 graphPosition)
            {

                Debug.Log(graphPosition.y);
                GameObject gameObject = new GameObject("bar", typeof(Image));
                gameObject.transform.SetParent(graphContainer, false);
                gameObject.GetComponent<Image>().color = barColor;
                RectTransform rectTransform = gameObject.GetComponent<RectTransform>();
                rectTransform.pivot = new Vector2(0.5f, 0f);
                rectTransform.anchoredPosition = new Vector2(graphPosition.x, 0);
                rectTransform.sizeDelta = new Vector2(50.0f * barWidthMultiplier, graphPosition.y);
                rectTransform.anchorMin = Vector2.zero;
                rectTransform.anchorMax = Vector2.zero;

                return gameObject;
            }
        }
        */

        /*
        private class DotGraphVisual
        {
            private RectTransform graphContainer;
            private float dotSizeMultiplier;
            private Sprite dotSprite;
            private GameObject lastDot = null;

            public DotGraphVisual(RectTransform graphContainer, Sprite dotSprite, float dotSizeMultiplier)
            {
                this.graphContainer = graphContainer;
                this.dotSizeMultiplier = dotSizeMultiplier;
                this.dotSprite = dotSprite;
            }
            public List<GameObject> AddGraphVisual(Vector2 graphPosition)
            {

                List<GameObject> gameObjectsList = new List<GameObject>();
                GameObject dot = CreateDot(graphPosition);
                gameObjectsList.Add(dot);
                if (lastDot != null)
                {
                    GameObject dotConnect = CreateDotConnection(lastDot.GetComponent<RectTransform>().anchoredPosition,
                            dot.GetComponent<RectTransform>().anchoredPosition);
                    gameObjectsList.Add(dotConnect);
                }
                lastDot = dot;
                return gameObjectsList;
            }



            GameObject CreateDot(Vector2 anchoredPosition,bool activeSelf = true)
            {
                GameObject gameObject = new GameObject("circle", typeof(Image));
                gameObject.transform.SetParent(graphContainer, false);
                gameObject.GetComponent<Image>().sprite = dotSprite;
                RectTransform rectTransform = gameObject.GetComponent<RectTransform>();
                rectTransform.anchoredPosition = anchoredPosition;

                gameObject.SetActive(activeSelf);

                rectTransform.sizeDelta = new Vector2(32, 32) * dotSizeMultiplier;
                rectTransform.anchorMin = Vector2.zero;
                rectTransform.anchorMax = Vector2.zero;

                return gameObject;
            }

            private GameObject CreateDotConnection(Vector2 dotPositionA, Vector2 dotPositionB)
            {
                GameObject gameObject = new GameObject("dotConnection", typeof(Image));
                gameObject.transform.SetParent(graphContainer, false);
                gameObject.GetComponent<Image>().color = new Color(1, 1, 1, 0.5f);


                float dist = Vector2.Distance(dotPositionA, dotPositionB);
                Vector2 direc = (dotPositionB - dotPositionA).normalized;
                RectTransform rectTransform = gameObject.GetComponent<RectTransform>();

                rectTransform.anchorMin = Vector2.zero;
                rectTransform.anchorMax = Vector2.zero;
                rectTransform.sizeDelta = new Vector2(dist, 5);


                float angle = Mathf.Atan2(dotPositionB.y - dotPositionA.y, dotPositionB.x - dotPositionA.x);
                angle = Mathf.Rad2Deg * angle;

                rectTransform.anchoredPosition = dotPositionA + (direc * dist * .5f);
                rectTransform.localEulerAngles = new Vector3(0, 0, angle);

                return gameObject;
            }
        }
        */
        

        public class TemplateX
        {
            public GameObject parent;
            public GameObject dash;
            public GameObject label;
            public GameObject dot;
            public GameObject dotConnection;

            public TemplateX(GameObject templateX)
            {
                parent = templateX;
                dash = templateX.transform.Find("dashTemplateX").gameObject;
                label = templateX.transform.Find("labelTemplateX").gameObject;
                dot = templateX.transform.Find("dot").gameObject;
                dotConnection = templateX.transform.Find("dotConnection").gameObject;
            }


        }
        public class TemplateY
        {
            public GameObject parent;
            public GameObject dash;
            public GameObject label;

            public TemplateY(GameObject templateY)
            {
                parent = templateY;
                dash = templateY.transform.Find("dashTemplateY").gameObject;
                label = templateY.transform.Find("labelTemplateY").gameObject;
            }
        }

        public class YInfo
        {
            public float maximum, minimum;

            public float displayMaximum
            {
                get
                {
                    return  maximum + different * .1f;
                }
            }
            public float displayMinimum
            {
                get
                {
                    return minimum - different * .1f;
                }
            }
            public float different
            {
                get {
                    float _different = maximum - minimum;
                    if (_different <= 0) return 10;
                    return _different;
                }
            }

            public YInfo( float maximum, float minimum)
            {
                this.maximum = maximum;
                this.minimum = minimum;
            }
        }


    }
    public class GraphConfig
    {
        [Tooltip("图表缩放值")]
        public float graphScaleMultiplier = 1;
        [Tooltip("点icon缩放值")]
        public float dotSizeMultiplier = 1;
        [Tooltip("线段缩放值")]
        public float dotConnectionSizeMultiplier = 1;
        [Tooltip("最大显示数量")]
        public int MaxVisiableCount = 20;
        [Tooltip("对齐方式")]
        public Alignment alignment = Alignment.MiddleCenter;

        public Vector2 YMinMax = new Vector2(-10,10);

        public RectTransform SetAlignment(RectTransform rectTransform)
        {           
            switch (alignment)
            {
                case Alignment.UpperLeft:
                    rectTransform.anchoredPosition = Vector2.zero;
                    rectTransform.anchorMax = rectTransform.anchorMin = rectTransform.pivot = new Vector2(0, 1);              
                    break;

                case Alignment.UpperCenter:
                    rectTransform.anchorMax = rectTransform.anchorMin = rectTransform.pivot = new Vector2(.5f, 1);
                    rectTransform.anchoredPosition = Vector2.zero;
                    break;

                case Alignment.UpperRight:
                    rectTransform.anchorMax = rectTransform.anchorMin = rectTransform.pivot = new Vector2(1, 1);
                    rectTransform.anchoredPosition = Vector2.zero;
                    break;
                case Alignment.MiddleLeft:
                    rectTransform.anchoredPosition = Vector2.zero;
                    rectTransform.anchorMax = rectTransform.anchorMin = rectTransform.pivot = new Vector2(0, .5f);
                    break;

                case Alignment.MiddleCenter:
                    rectTransform.anchorMax = rectTransform.anchorMin = rectTransform.pivot = new Vector2(.5f, .5f);
                    rectTransform.anchoredPosition = Vector2.zero;
                    break;

                case Alignment.MiddleRight:
                    rectTransform.anchorMax = rectTransform.anchorMin = rectTransform.pivot = new Vector2(1, .5f);
                    rectTransform.anchoredPosition = Vector2.zero;
                    break;
                case Alignment.LowerLeft:
                    rectTransform.anchoredPosition = Vector2.zero;
                    rectTransform.anchorMax = rectTransform.anchorMin = rectTransform.pivot = new Vector2(0, 0);
                    break;

                case Alignment.LowerCenter:
                    rectTransform.anchorMax = rectTransform.anchorMin = rectTransform.pivot = new Vector2(.5f, 0);
                    rectTransform.anchoredPosition = Vector2.zero;
                    break;

                case Alignment.LowerRight:
                    rectTransform.anchorMax = rectTransform.anchorMin = rectTransform.pivot = new Vector2(1, 0);
                    rectTransform.anchoredPosition = Vector2.zero;
                    break;
            }

            return rectTransform;
        }
    }

    public enum Alignment
    {
        UpperLeft, UpperCenter,UpperRight,
        MiddleLeft, MiddleCenter,MiddleRight,
        LowerLeft,LowerCenter,LowerRight
    }
}
