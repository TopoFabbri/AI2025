using System;
using FSM;
using Game.Mining;
using Pool;
using UnityEngine;

namespace Game.Miner.States
{
    public class MineState : State
    {
        public override Type[] OnTickParamTypes => new[] { typeof(Mine), typeof(GoldContainer), typeof(float), typeof(float) };

        public override BehaviourActions GetOnTickBehaviours(params object[] parameters)
        {
            Mine mine = parameters[0] as Mine;
            GoldContainer goldContainer = parameters[1] as GoldContainer;
            float mineSpeed = (float)parameters[2];
            float delta = (float)parameters[3];

            BehaviourActions behaviourActions = ConcurrentPool.Get<BehaviourActions>();

            behaviourActions.AddMainThreadBehaviour(0, () => { MineGold(mine, goldContainer, mineSpeed, delta); });

            return behaviourActions;
        }

        private void MineGold(Mine mine, GoldContainer goldContainer, float mineSpeed, float delta)
        {
            if (!mine || goldContainer == null) return;

            float mineAmount = mineSpeed * delta;
            float minedAmount = mine.GetGold(mineAmount);

            if (!Mathf.Approximately(minedAmount, mineAmount))
            {
                minedAmount += mine.GetGold(mine.GoldAmount);
                OnFlag?.Invoke(Miner.Flags.MineLost);
            }


            if (!goldContainer.AddGold(minedAmount))
                OnFlag?.Invoke(Miner.Flags.BagFull);
        }
    }
}