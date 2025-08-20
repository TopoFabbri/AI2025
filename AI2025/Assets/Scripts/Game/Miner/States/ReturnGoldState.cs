using System;
using FSM;
using Game.Mining;
using Pool;
using UnityEngine;

namespace Game.Miner.States
{
    public class ReturnGoldState : State
    {
        public override Type[] OnTickParamTypes => new[] { typeof(GoldStorage), typeof(Transform), typeof(GoldContainer), typeof(float), typeof(float), typeof(float) };

        public override BehaviourActions GetOnTickBehaviours(params object[] parameters)
        {
            GoldStorage goldStorage = parameters[0] as GoldStorage;
            Transform minerTransform = parameters[1] as Transform;
            GoldContainer goldContainer = parameters[2] as GoldContainer;
            float speed = (float)parameters[3];
            float delta = (float)parameters[4];
            float mineDistance = (float)parameters[5];

            BehaviourActions behaviourActions = ConcurrentPool.Get<BehaviourActions>();

            behaviourActions.AddMainThreadBehaviour(0, () => { WalkToStorage(goldStorage, minerTransform, speed, delta); });
            behaviourActions.AddMainThreadBehaviour(1, () => { CheckAndLeaveGold(minerTransform, goldStorage, goldContainer, mineDistance); });

            return behaviourActions;
        }

        private static void WalkToStorage(GoldStorage goldStorage, Transform minerTransform, float speed, float delta)
        {
            if (!goldStorage || !minerTransform) return;

            Vector3 direction = (goldStorage.transform.position - minerTransform.position).normalized;
            minerTransform.Translate(direction * speed * delta);
        }

        private void CheckAndLeaveGold(Transform minerTransform, GoldStorage goldStorage, GoldContainer goldContainer, float mineDistance)
        {
            float dis = Vector3.Distance(goldStorage.transform.position, minerTransform.position);

            if (dis < mineDistance)
            {
                goldStorage.AddGold(goldContainer.GetAllGold());
                OnFlag?.Invoke(Miner.Flags.GoldReturned);
            }
        }
    }
}