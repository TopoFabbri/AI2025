using System;
using Game.Creeper;
using Pool;
using UnityEngine;

namespace FSM
{
    public sealed class ChaseState : State
    {
        public override Type[] OnTickParamTypes => new[]
        {
            typeof(Transform),
            typeof(Transform),
            typeof(float),
            typeof(float),
            typeof(float),
            typeof(float)
        };
        
        public override BehaviourActions GetOnTickBehaviours(params object[] parameters)
        {
            Transform agentTransform = parameters[0] as Transform;
            Transform targetTransform = parameters[1] as Transform;
            float speed = (float)parameters[2];
            float explodeDistance = (float)parameters[3];
            float lostDistance = (float)parameters[4];
            float deltaTime = (float)parameters[5];

            BehaviourActions behaviourActions = ConcurrentPool.Get<BehaviourActions>();

            behaviourActions.AddMainThreadBehaviour(0, () =>
            {
                if (!agentTransform) return;
                
                if (targetTransform)
                    agentTransform.position += (targetTransform.position - agentTransform.position).normalized * speed * deltaTime;
            });

            behaviourActions.SetTransitionBehaviour(() =>
            {
                if (agentTransform && targetTransform && Vector3.Distance(agentTransform.position, targetTransform.position) < explodeDistance)
                {
                    OnFlag.Invoke(Agent.Flags.OnTargetReach);
                }

                if (agentTransform && targetTransform && Vector3.Distance(agentTransform.position, targetTransform.position) > lostDistance)
                {
                    OnFlag.Invoke(Agent.Flags.OnTargetLost);
                }
            });

            return behaviourActions;
        }
    }
}