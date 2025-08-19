using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using UnityEngine;

namespace FSM
{
    public struct BehaviourActions : IEquatable<BehaviourActions>
    {
        public Dictionary<int, List<Action>> MainThreadBehaviours { get; private set; }

        public ConcurrentDictionary<int, List<Action>> MultiThreadableBehaviours { get; private set; }

        public Action TransitionBehaviour { get; private set; }

        public void AddMainThreadableBehaviour(int executionOrder, Action behaviour)
        {
            MainThreadBehaviours ??= new Dictionary<int, List<Action>>();
            
            if (!MainThreadBehaviours.ContainsKey(executionOrder))
                MainThreadBehaviours.Add(executionOrder, new List<Action>());

            MainThreadBehaviours[executionOrder].Add(behaviour);
        }

        public void AddMultiThreadableBehaviour(int executionOrder, Action behaviour)
        {
            MultiThreadableBehaviours ??= new ConcurrentDictionary<int, List<Action>>();
            
            if (!MultiThreadableBehaviours.ContainsKey(executionOrder))
                MultiThreadableBehaviours.TryAdd(executionOrder, new List<Action>());

            MultiThreadableBehaviours[executionOrder].Add(behaviour);
        }

        public void SetTransitionBehaviour(Action behaviour)
        {
            TransitionBehaviour = behaviour;
        }

        public bool Equals(BehaviourActions other)
        {
            return Equals(MainThreadBehaviours, other.MainThreadBehaviours) && Equals(MultiThreadableBehaviours, other.MultiThreadableBehaviours) && Equals(TransitionBehaviour, other.TransitionBehaviour);
        }

        public override bool Equals(object obj)
        {
            return obj is BehaviourActions other && Equals(other);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(MainThreadBehaviours, MultiThreadableBehaviours, TransitionBehaviour);
        }
    }

    public abstract class State
    {
        public Action<Enum> OnFlag;
        public abstract BehaviourActions GetOnEnterBehaviours(params object[] parameters);
        public abstract BehaviourActions GetOnTickBehaviours(params object[] parameters);
        public abstract BehaviourActions GetOnExitBehaviour(params object[] parameters);
    }

    public sealed class PatrolState : State
    {
        private Transform actualTarget;

        public override BehaviourActions GetOnEnterBehaviours(params object[] parameters)
        {
            return default;
        }

        public override BehaviourActions GetOnExitBehaviour(params object[] parameters)
        {
            return default;
        }

        public override BehaviourActions GetOnTickBehaviours(params object[] parameters)
        {
            Transform wayPoint1 = parameters[0] as Transform;
            Transform wayPoint2 = parameters[1] as Transform;
            Transform agentTransform = parameters[2] as Transform;
            Transform targetTransform = parameters[3] as Transform;
            float speed = (float)parameters[4];
            float chaseDistance = (float)parameters[5];
            float deltaTime = (float)parameters[6];

            BehaviourActions behaviourActions = new BehaviourActions();

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

    public sealed class ChaseState : State
    {
        public override BehaviourActions GetOnEnterBehaviours(params object[] parameters)
        {
            return default;
        }

        public override BehaviourActions GetOnExitBehaviour(params object[] parameters)
        {
            return default;
        }

        public override BehaviourActions GetOnTickBehaviours(params object[] parameters)
        {
            Transform agentTransform = parameters[0] as Transform;
            Transform targetTransform = parameters[1] as Transform;
            float speed = (float)parameters[2];
            float explodeDistance = (float)parameters[3];
            float lostDistance = (float)parameters[4];
            float deltaTime = (float)parameters[5];

            BehaviourActions behaviourActions = new BehaviourActions();

            behaviourActions.AddMainThreadableBehaviour(0, () =>
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

    public sealed class ExplodeState : State
    {
        public override BehaviourActions GetOnEnterBehaviours(params object[] parameters)
        {
            BehaviourActions behaviourActions = new BehaviourActions();
            behaviourActions.AddMultiThreadableBehaviour(0, () => { Debug.Log("Boom"); });
            return behaviourActions;
        }

        public override BehaviourActions GetOnExitBehaviour(params object[] parameters)
        {
            return default;
        }

        public override BehaviourActions GetOnTickBehaviours(params object[] parameters)
        {
            BehaviourActions behaviourActions = new BehaviourActions();
            behaviourActions.AddMultiThreadableBehaviour(0, () => { Debug.Log("F"); });
            return behaviourActions;
        }
    }
}