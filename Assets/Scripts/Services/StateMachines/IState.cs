using System;
using System.Threading.Tasks;

namespace Services.StateMachines
{
    public interface IState<T> where T : Enum 
    {
        T State { get; }
        Task Enter(ChangeStateData changeStateData = null);
        void Exit();
        void Update(float deltaTime);
    }
}
