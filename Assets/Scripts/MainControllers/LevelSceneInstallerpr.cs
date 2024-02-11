using Game;
using UI;
using UnityEngine;
using Zenject;

public class LevelSceneInstallerpr : MonoInstaller
{
   
    [SerializeField]
    private  PlayerScript _playerScriptpr;
    [SerializeField]
    private CameraControlspr _cameraControlspr;
    [SerializeField]
    private UIManagerpr _uiManagerpr;
    
    public override void InstallBindings()
    {
        Container.Bind<PlayerScript>().FromInstance(_playerScriptpr).AsSingle().NonLazy();
        Container.Bind<CameraControlspr>().FromInstance(_cameraControlspr).AsSingle().NonLazy();
        Container.Bind<UIManagerpr>().FromInstance(_uiManagerpr).AsSingle().NonLazy();
        //Container.Bind<SettingsData>().AsSingle();
    }
}
