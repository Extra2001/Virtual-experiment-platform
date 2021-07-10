# 误差理论虚拟实验平台

### 本项目目录结构
```
Assets
├─3rd                           // 第三方插件目录
│  ├─Computer Classroom         // 基础场景插件
│  ├─Loading screen package     // 加载屏幕动画插件
│  ├─OBJImport                  // OBJ模型导入插件
│  ├─OwnRuntimeTransformGizmos  // 移动物体插件
│  ├─Plugins                    // 开源nuget package，包含数学计算库
│  ├─Standard Assets            // Unity标准库，包含一人称控制器
│  └─unity-ui-extensions        // UGUI扩展插件
├─Editor                        // 编辑器脚本，主要来自第三方
├─HTFramework                   // 框架主体
├─Resources                     // 资源包，包含UI预制体和3D模型、音效和图像文件
│  ├─Audios
│  ├─Instruments                // 仪器资源，包含3D模型、图像
│  │  ├─Ammeter
│  │  ├─Caliper
│  │  ├─ElectronicScales
│  │  ├─Micrometer
│  │  ├─Ruler
│  │  ├─Thermometer
│  │  └─Voltmeter
│  └─UI                         // UI预制体
│      ├─Bag                    // 仪器和被测物体抽屉UI
│      ├─DataProcess            // 数据处理UI
│      │  └─Pictures
│      ├─Formula                // 公式编辑器UI
│      │  ├─Cells
│      │  └─Images
│      ├─Indicator              // 信息指示器UI
│      ├─Measurment             // 测量时UI
│      │  └─DataTable
│      ├─PreExperiment          // 实验前UI
│      ├─Record                 // 存档UI
│      ├─Resources              // UI公共图片、字体资源
│      │  ├─Common
│      │  │  └─Fonts
│      │  └─Indicator
│      ├─Select_instruments
│      ├─Settings               // 设置UI
│      └─ValidNumber            // 有效数字UI
├─Scenes                        // 场景
│  ├─BaseEnv
│  └─MainScene
├─Scripts                       // 脚本，代码文件都在这
│  ├─Game                       // 主要的游戏逻辑脚本
│  │  ├─EventHandlers           // 程序内部事件载体
│  │  │  └─PreExperiment
│  │  ├─Instruments             // 仪器属性
│  │  ├─Procedures              // 流程载体
│  │  │  ├─DataProcess
│  │  │  └─PreExperiment
│  │  ├─Trigger                 // 触发器
│  │  └─UI                      // UI逻辑
│  │      ├─Bag                 // 仪器和被测物体抽屉
│  │      │  ├─SelectInstrument
│  │      │  └─SelectObject
│  │      ├─DataProcess         // 数据处理交互逻辑
│  │      │  ├─ComplexDataProcess
│  │      │  ├─MeasuredDataProcess
│  │      │  ├─ProcessExplain
│  │      │  └─ProcessResult
│  │      ├─Formula             // 公式编辑器交互逻辑核心
│  │      ├─Indicator           // 提示器交互
│  │      ├─Meaurement          // 测量时UI交互逻辑
│  │      │  └─UILogic
│  │      ├─PausePanel          // 暂停面板UI交互逻辑
│  │      ├─PreExperiment       // 实验配置UI交互逻辑
│  │      │  └─UILogic
│  │      ├─Records             // 存档交互逻辑
│  │      ├─Settings            // 游戏设置UI交互逻辑
│  │      ├─UIAPIs              // UI公共API接口
│  │      └─ValidNumber         // 有效数字传递计算UI逻辑
│  ├─Managers                   // 公共核心，包含启动器
│  ├─Models                     // 数据交互模型
│  ├─Object                     // 3D交互逻辑
│  └─Utils                      // 通用工具
│      ├─Calculate              // 计算程序核心
│      ├─FlexibleMenu
│      ├─Record                 // 存档服务核心
│      └─Server                 // 远程交互核心
└─StreamingAssets               // 预置被测物体的obj
    ├─Objects
    └─PreviewImages
```