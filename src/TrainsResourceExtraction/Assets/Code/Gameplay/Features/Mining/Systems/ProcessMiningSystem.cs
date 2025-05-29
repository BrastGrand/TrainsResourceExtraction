using Entitas;
using Code.Gameplay.Common.Time;
using UnityEngine;

namespace Code.Gameplay.Features.Mining.Systems
{
    public class ProcessMiningSystem : IExecuteSystem
    {
        private readonly IGroup<GameEntity> _miningEntities;
        private readonly ITimeService _timeService;

        public ProcessMiningSystem(GameContext context, ITimeService timeService)
        {
            _timeService = timeService;
            _miningEntities = context.GetGroup(GameMatcher.AllOf(
                GameMatcher.Train,
                GameMatcher.Mining,
                GameMatcher.MiningTime
            ));
        }

        public void Execute()
        {
            foreach (var entity in _miningEntities.GetEntities())
            {
                var miningTime = entity.MiningTime;
                miningTime -= _timeService.DeltaTime;

                if (miningTime <= 0)
                {
                    entity.isMiningCompleted = true;
                    entity.RemoveMiningTime();
                }
                else
                {
                    entity.ReplaceMiningTime(miningTime);
                }
            }
        }
    }
} 