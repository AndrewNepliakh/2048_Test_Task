using Zenject;
using Managers;
using Controllers;
using Managers.CubesManager;
using System.Threading.Tasks;
using Services.StateMachines;

public class SetupGameplayStateController : IGameplayStateController
{
    [Inject] private SetupGameplayState _state;
    [Inject] private ILevelManager _levelManager;
    [Inject] private ICubesManager _cubesManager;

    public async Task Init()
    {
        _state.OnStateComplete += OnStateComplete;
        
        await InitLevel();
        await InitCubes();
        
        _state.OnStateComplete.Invoke();
    }
    
    private async Task InitLevel() => 
        await _levelManager.InstantiateLevel<Level_1>(new LevelsArguments());

    private async Task InitCubes()
    {
        CubeData[] cubeDatas =
        {
            new(CubeState.Target)
        };

        foreach (var cubeData in cubeDatas)
            await _cubesManager.SpawnCube(cubeData);
    }
    
    private void OnStateComplete()
    {
        _state.OnStateComplete -= OnStateComplete;
    }
}