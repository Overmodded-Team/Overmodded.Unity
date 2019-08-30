# JEM

![JEM](https://i.imgur.com/MtJMl05.png)

### Simple C# „Just Enough Methods” Library source-code repository

A Unity Engine/Editor extension.

# Features
 - JEM.UnityEngine:
    - `JEMUnity` - Prepares all the components used by JEM.Unity
    - `JEMObjectKeepOnScene` - Just a `DontDestroyOnLoad` on Awake.
    - `JEMBounds` - Few util methods for `UnityEngine.Bounds` like `BoundsFromMeshRenderers` and `BoundsFromColliders`.
    - `JEMDestroyAfterTime` - Simple script that destroys object after amount of time.
    - `JEMObject` - Few util methods for `UnityEngine.Object` like `LiteDestroy` or `LiteInstantiate`. `Lite` methods puts your request in corontinue to prevent frames dropps.
    - `JEMOperation` - A operation methods `InvokeAction` that run given action after some time, and `StartCoroutine` that allows you to start new coroutine on global object.
    - `JEMSceneLoader` - Simple scene loader script. Uses `JEMInterfaceFadeAnimation` as loading panel.
    - `JEMSprite` - Few util methods for `UnityEngine.Sprite` like `FromTexture2D`.
    - `JEMSwitch` - Simple 'switch' script.
    - `JEMTexture2D` - Few util methods for `UnityEngine.Texture2D` like `ToSprite` or `FromBytes` `ToBytes`.
    - `JEMTimer` - Simple `timer` class.
    - `JEMTranslator/Rect` - Script that translates target from point A to point B.
    - `JEMFreeCamera` - Free camera controller.
    - `JEMThirdPersonCamera` - A thirdperson camera controller.
    - `JEMBuild` - A build/compilation number controller.
    - `JEMInterfaceFadeAnimation` - Component for UI to enable or disable UI gameObjects with fade animation.
    - `JEMInterfaceFadeElement` - Defines UI element that can Faded by `JEMInterfaceFadeAnimation`.
    - `JEMInterfaceWindow` - UI window controller that allows you with using `JEMInterfaceWindowHeader`&`Resize` to make draggable and resizable windows.
    - `JEMExtension*` - A util extension methods.
    - `JEMInterfaceSliderGradient` - Script that applays gradient(based on slider position) to target image.
    - `JEMInterfaceSuperSlider` - Simple script that interpolates two sliders to create 'damage' effect.
 - JEM.UnityEditor:
    - `JEMAssetBuilder` - Asset builder lets you quickly create and build AssetBundles.
         ![](https://i.imgur.com/QD12KTg.png)
    - `JEMBetterEditor` - Few utility methods for `EditorGUILayout`.
 - JEM.QNet.UnityEngine - A [JEM.QNet](https://github.com/TylkoDemon/JEM/tree/master/src/JEM.QNet) extenstion that lets you create multiplayer in few steps.

# Examples

Comming soon.


*JEM (c) 2019 Adam Majcherek*
