using System;
using Pool;
using UnityEngine;

namespace FSM
{
    public sealed class PatrolState : State
    {
        private Transform actualTarget;

        public override Type[] OnTickParamTypes =>
            new[] { typeof(Transform), typeof(Transform), typeof(Transform), typeof(Transform), typeof(float), typeof(float), typeof(float) };

        public override BehaviourActions GetOnTickBehaviours(params object[] parameters)
        {
            Transform wayPoint1 = parameters[0] as Transform;
            Transform wayPoint2 = parameters[1] as Transform;
            Transform agentTransform = parameters[2] as Transform;
            Transform targetTransform = parameters[3] as Transform;
            float speed = (float)parameters[4];
            float chaseDistance = (float)parameters[5];
            float deltaTime = (float)parameters[6];

            BehaviourActions behaviourActions = ConcurrentPool.Get<BehaviourActions>();

            behaviourActions.AddMainThreadableBehaviour(0, () =>
            {
                if (actualTarget == null)
                {
                    actualTarget = wayPoint1;
                }

                if (agentTransform != null && actualTarget != null && !(Vector3.Distance(agentTransform.position, actualTarget.position) < 0.1f)) return;

                actualTarget = actualTarget == wayPoint1 ? wayPoint2 : wayPoint1;
            });

            behaviourActions.AddMainThreadableBehaviour(1, () =>
            {
                if (agentTransform != null) agentTransform.position += (actualTarget.position - agentTransform.position).normalized * speed * deltaTime;
            });

            behaviourActions.SetTransitionBehaviour(() =>
            {
                if (agentTransform && targetTransform && Vector3.Distance(agentTransform.position, targetTransform.position) <= chaseDistance)
                {
                    OnFlag?.Invoke(Agent.Flags.OnTargetNear);
                }
            });
            return behaviourActions;
        }
    }
}