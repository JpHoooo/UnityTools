# Unity 开发工具

### How to Use

在Package Manager窗口中寻找 `+` 号按钮，选中并点击 `add package from git URL`，将本repo的Git URL复制输入框后点击 `Add` 按钮。

--- 



#### 数据可视化

##### Usage

```csharp
using UnityEngine;
using Jphoooo.Tools;
public class Tester : MonoBehaviour
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
        // or
        GraphConfig config = new GraphConfig();
        drawer =new GraphDrawer(config);
        FunctionPeriodic(InputData, 0.1f);
    }

    void InputData()
    {
        data += Mathf.Deg2Rad * 20;
        drawer.AddData(Mathf.Sin(data));
    }
}
```

##### Preview

![DataVisualization](https://github.com/JpHoooo/UnityTools/assets/42137140/f9570da9-65b6-4ce0-863a-f7cdc0f45d2a)
