using Game;
using MainControllers;
using UI;
using UnityEngine;
using Zenject;

public class LevelSceneInstallerpr : MonoInstaller
{
   
    [SerializeField]
    private  PlayerScriptpr _playerScriptpr;
    [SerializeField]
    private CameraControlspr _cameraControlspr;
    [SerializeField]
    private UIManagerpr _uiManagerpr;
    
    public override void InstallBindings()
    {
        Container.Bind<PlayerScriptpr>().FromInstance(_playerScriptpr).AsSingle().NonLazy();
        Container.Bind<CameraControlspr>().FromInstance(_cameraControlspr).AsSingle().NonLazy();
        Container.Bind<UIManagerpr>().FromInstance(_uiManagerpr).AsSingle().NonLazy();
    }
}
