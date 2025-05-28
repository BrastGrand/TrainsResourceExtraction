using Cysharp.Threading.Tasks;

namespace Code.Infrastructure.States.StateInfrastructure
{
    public class SimplePayloadState<TPayload> : IPayloadState<TPayload>
    {
        public virtual UniTask Enter(TPayload payload) => UniTask.CompletedTask;

        protected virtual void Exit() { }

        async UniTask IExitableState.BeginExit()
        {
            Exit();
            await UniTask.CompletedTask;
        }

        void IExitableState.EndExit() { }
    }
}