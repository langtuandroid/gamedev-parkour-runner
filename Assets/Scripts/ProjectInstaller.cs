using Integration;
using UnityEngine;
using Zenject;

public class ProjectInstaller : MonoInstaller
{
    [SerializeField] 
    private AdMobController _adMobController;

    public override void InstallBindings()
    {
        Container.Bind<AdMobController>().FromInstance(_adMobController).AsSingle().NonLazy();

        Container.Bind<BannerViewController>().AsSingle().NonLazy();
        Container.Bind<InterstitialAdController>().AsSingle().NonLazy();
        Container.Bind<RewardedAdController>().AsSingle().NonLazy();
    }
}
