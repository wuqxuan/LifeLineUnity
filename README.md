<!--![](http://upload-images.jianshu.io/upload_images/266748-635b8f94a4500b23.jpg?imageMogr2/auto-orient/strip%7CimageView2/2/w/1240/imageView/2/w/619/q/90)-->
<img src="http://upload-images.jianshu.io/upload_images/266748-635b8f94a4500b23.jpg?imageMogr2/auto-orient/strip%7CimageView2/2/w/1240" width = "850" height = "500" alt="图片名称" align=center />
## Lifeline made with Unity
  
Unity(5.4.2f2) 版 **Lifeline**， 设计参考了 [生命线：静夜 on Telegram](http://www.jianshu.com/p/5a480d2d5dc6)。实现思路很简单，主要是文本处理，先把拿到的 txt 格式剧本解析成 json，在 Unity 中用 UGUI 加一些控制逻辑就可以了。Unity 中使用 [SimpleJSON](http://wiki.unity3d.com/index.php/SimpleJSON) 解析 json 文本，经测试 Android 和 iOS 正常运行。  
 
剧本是 2015 年大热的[这一版](https://itunes.apple.com/cn/app/lifeline-sheng-ming-xian/id982354972?mt=8)，使用 python3 写了 txt 剧本解析成 json 的脚本: 

``` 
 cd JsonData   
 
 python3 ExtractJson.py   
 
``` 


也可以在终端运行游戏( Python2 ):  

``` 
cd JsonData  
 
python lifeline_on_terminal.py 

```


### Project   
  
- Open Assets/Scenes/Level_Mobile  
- Set Game scene resolution to 9:16     
  

<img src="https://github.com/wuqxuan/LifeLineUnity/raw/master/image/Lifeline.png" width = "300" height = "500" alt="图片名称" align=center />


