using UI;
using Zenject;
using Managers;
using Factories;
using Controllers;
using Managers.SaveManager;
using Managers.UserManager;
using Managers.CubesManager;
using Services.StateMachines;
using Implementations.Game.Signals;

public class BootstrapInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        SignalBusInstaller.Install(Container);
        
        Container.DeclareSignalWithInterfaces<CubeCollidedSignal>();
        Container.DeclareSignalWithInterfaces<NextLevelSignal>();
        
        Container.Bind<IObjectFactory<Cube>>().To<ObjectFactory<Cube>>().AsSingle().NonLazy();

        Container.Bind<ICubesManager>().To<CubesManager>().AsSingle().NonLazy();
        
        Container.Bind<IAssetsManager>().To<AssetsManager>().AsSingle().NonLazy();
        Container.Bind<IUIManager>().To<UIManager>().AsSingle().NonLazy();
        Container.Bind<IUserManager>().To<UserManager>().AsSingle().NonLazy();
        Container.Bind<ISaveManager>().To<SaveManager>().AsSingle().NonLazy();
        
        Container.Bind(typeof(IGameManager), typeof(IInitializable)).To<GameManager>().AsSingle().NonLazy();
        Container.Bind<ILevelManager>().To<LevelManager>().AsSingle().NonLazy();
        Container.Bind<GameplayStateMachine<GameplayStates>>().AsSingle().NonLazy();

        Container.Bind<SetupGameplayState>().AsSingle().NonLazy();
        Container.Bind<AimGameplayState>().AsSingle().NonLazy();
        Container.Bind<ResultGameplayState>().AsSingle().NonLazy();
        Container.Bind<WinGameplayState>().AsSingle().NonLazy();

        Container.Bind<SetupGameplayStateController>().AsSingle().NonLazy();
        Container.Bind<AimGameplayStateController>().AsSingle().NonLazy();
        Container.Bind<ResultGameplayStateController>().AsSingle().NonLazy();
        Container.Bind<WinGameplayStateController>().AsSingle().NonLazy();
    }
}