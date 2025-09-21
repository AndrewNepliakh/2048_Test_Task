using Zenject;
using UnityEngine;
using Implementations.Game.Controllers;

public class GameplayInstaller : MonoInstaller
{
    [SerializeField] private GameplayController _gameplayController;
    
    public override void InstallBindings()
    {
        Container.Bind<GameplayController>().FromInstance(_gameplayController).AsSingle().NonLazy();
    }
}