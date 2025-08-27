# UnityCommonSolution-MultimediaPlayer

![GitHub](https://img.shields.io/badge/Unity-2021.3%2B-blue)
![GitHub](https://img.shields.io/badge/license-MIT-green)
![GitHub](https://img.shields.io/badge/Platform-Windows-red)

这是一个基于Unity中最火热的视频播放插件开发而来的，结合[TouchSocket](https://github.com/RRQM/TouchSocket)实现远程控制，支持TCP、UDP等协议的字符串消息控制视频的播控功能。
## 依赖
   需要安装[TouchSocket的Unity版本](https://github.com/RRQM/TouchSocket/tree/master/examples/Unity3d/UnityPackage) 支持，请下载其中的*.unitypackage包导入到Unity中后再通过下方的方法安装本仓库。

## 安装

1. **通过克隆仓库安装**

   将本仓库克隆到您的 Unity 项目的 `Assets` 目录下：

   ```bash
   git clone https://github.com/Pixelsmao/UnityCommonSolution-ScriptPersistence.git
   ```

2. **使用UPM进行安装：**

   在 Unity 编辑器中，点击顶部菜单栏,打开 Package Manager 窗口.

   ```
   Window > Package Manager
   ```

   在 Package Manager 窗口的左上角，点击 **+** 按钮，然后选择 **Add package from git URL...**。
   在弹出的输入框中，粘贴本仓库的 Git URL：

   ```
   https://github.com/Pixelsmao/UnityCommonSolution-ScriptPersistence.git
   ```

   然后点击 **Add**。

## 使用方法

1.使用`PersistenceScript`属性标记脚本，然后运行程序，会在程序目录中生成`Applocation.ini`配置文件。