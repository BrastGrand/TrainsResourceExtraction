using Cysharp.Threading.Tasks;

namespace Code.Infrastructure.States.StateInfrastructure
{
    public interface IExitableState
    {
        UniTask BeginExit();
        void EndExit();
    }
}