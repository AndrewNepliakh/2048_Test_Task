using System;
using System.Threading.Tasks;

namespace Managers.CubesManager
{
    public interface ICubesManager : IDisposable
    {
        event Action OnCubeMergedEvent;
        int ActiveCubesCount { get; }
        Task<Cube> SpawnCube(CubeData cubeData);
        void HideCube(Cube cube);
        void ClearAllCubes();
    }
}