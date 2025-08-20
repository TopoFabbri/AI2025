using System;
using FSM;
using Game.Mining;
using Pool;
using UnityEngine;

namespace Game.Miner.States
{
    public class GoToMineState : State
    {
        public override Type[] OnTickParamTypes => new[] { typeof(Mine), typeof(Transform), typeof(float), typeof(float), typeof(float) };

        public override BehaviourActions GetOnTickBehaviours(params object[] parameters)
        {
            Mine mine = parameters[0] as Mine;
            Transform minerTransform = parameters[1] as Transform;
            float speed = (float)parameters[2];
            float delta = (float)parameters[3];
            float mineDistance = (float)parameters[4];
            
            BehaviourActions behaviourActions = ConcurrentPool.Get<BehaviourActions>();
            
            behaviourActions.AddMainThreadBehaviour(0, () => { WalkToMine(mine, minerTransform, speed, delta); });
            behaviourActions.AddMainThreadBehaviour(1, () => { CheckDistance(minerTransform, mine, mineDistance); });
            
            return behaviourActions;
        }

        private static void WalkToMine(Mine mine, Transform minerTransform, float speed, float delta)
        {
            if (!mine || !minerTransform) return;
            
            Vector3 direction = (mine.transform.position - minerTransform.position).normalized;
            minerTransform.Translate(direction * speed * delta);
        }

        private void CheckDistance(Transform minerTransform, Mine mine, float mineDistance)
        {
            float dis = Vector3.Distance(mine.transform.position, minerTransform.position);
            
            if (dis < mineDistance)
                OnFlag?.Invoke(Miner.Flags.MiningDistance);
        }
    }
}