using MainControllers;
using UnityEngine;
using Zenject;

public class SceneContext : MonoInstaller
{
    [SerializeField] 
    private MainMenupr _mainMenupr;

    public override void InstallBindings()
    {
        Container.Bind<MainMenupr>().FromInstance(_mainMenupr).AsSingle().NonLazy();
    }
}
