using Pool;
using UnityEngine;

namespace FSM
{
    public sealed class ExplodeState : State
    {
        public override BehaviourActions GetOnEnterBehaviours(params object[] parameters)
        {
            BehaviourActions behaviourActions = ConcurrentPool.Get<BehaviourActions>();
            behaviourActions.AddMultiThreadableBehaviour(0, () => { Debug.Log("Boom"); });
            return behaviourActions;
        }

        public override BehaviourActions GetOnExitBehaviour(params object[] parameters)
        {
            return null;
        }

        public override BehaviourActions GetOnTickBehaviours(params object[] parameters)
        {
            BehaviourActions behaviourActions = ConcurrentPool.Get<BehaviourActions>();
            behaviourActions.AddMultiThreadableBehaviour(0, () => { Debug.Log("F"); });
            return behaviourActions;
        }
    }
}