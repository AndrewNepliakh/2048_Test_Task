using Zenject;
using UnityEngine;

[CreateAssetMenu(fileName = "SOInstaller", menuName = "Installers/SOInstaller")]
public class SOInstaller : ScriptableObjectInstaller<SOInstaller>
{
    [SerializeField] private GameConfig _gameConfig;

    public override void InstallBindings()
    {
        Container.Bind<GameConfig>().FromInstance(_gameConfig).AsSingle().NonLazy();
    }
}