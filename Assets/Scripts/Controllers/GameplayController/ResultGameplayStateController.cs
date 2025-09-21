using System;
using System.Threading.Tasks;

namespace Controllers
{
    public class ResultGameplayStateController : IGameplayStateController
    {
        public event Action OnStateComplete;
        
        public async Task Init()
        {
            await Task.Delay(TimeSpan.FromSeconds(1));
            
            OnStateComplete?.Invoke();
        }
    }
}