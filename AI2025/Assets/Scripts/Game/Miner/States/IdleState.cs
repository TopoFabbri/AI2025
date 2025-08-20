using System;
using FSM;
using Game.Mining;
using Pool;

namespace Game.Miner.States
{
    public class IdleState : State
    {
        public override Type[] OnTickParamTypes => new[] { typeof(Mine) };

        public override BehaviourActions GetOnTickBehaviours(params object[] parameters)
        {
            Mine mine = parameters[0] as Mine;
            
            BehaviourActions behaviourActions = ConcurrentPool.Get<BehaviourActions>();
            
            behaviourActions.AddMultiThreadableBehaviour(0, () =>
            {
                if (mine && mine.GoldAmount > 0)
                    OnFlag?.Invoke(Miner.Flags.MineFound);
            });

            return behaviourActions;
        }
    }
}
