using System;
using System.Threading.Tasks;

namespace Managers
{
    public interface ILevelManager : IDisposable
    {
        Level CurrentLevel { get; }
        Task InstantiateLevel<T>(LevelsArguments levelArgs) where T : Level;
        void ClearAllLevels();
    }
}