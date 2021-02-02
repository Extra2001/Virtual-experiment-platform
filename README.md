# 误差理论虚拟实验平台

### 本项目目录结构
```js
Assets 
 ├─3rd                // 第三方组件
 │  └─Plugins         // dll库
 ├─AssetsPackage      // 资源包，主要为Prefab预制体
 │  ├─Audios
 │  ├─Environment
 │  ├─Instruments     // 测量仪器资源
 │  ├─Objects         // 内置待测物体资源
 │  └─UI              // UI资源
 ├─Scenes             // 场景，只有MainScene会打包
 └─Scripts
     ├─3rd
     │  ├─Instruments // 仪器逻辑
     ├─Game           // 游戏逻辑
     ├─Managers       // 游戏管理器
     │  ├─UI          // UI逻辑
     |  └─Record
     └─Utils          // 通用工具
```