using FSM;
using Game.Miner.States;
using Game.Mining;
using UnityEngine;

namespace Game.Miner
{
    public class Miner : MonoBehaviour
    {
        #region - Enums -

        public enum States
        {
            Idle,
            GoToMine,
            Mine,
            ReturnGold
        }

        public enum Flags
        {
            MineFound,
            MiningDistance,
            MineLost,
            BagFull,
            GoldReturned
        }

        #endregion

        private FSM<States, Flags> fsm;

        [Header("DebugInfo")] [SerializeField] private float goldAmount;

        [Header("MineSettings")] [SerializeField]
        private Mine targetedMine;

        [SerializeField] private GoldStorage goldStorage;
        [SerializeField] private float mineDistance;
        [SerializeField] private float mineSpeed;

        [Header("WalkSettings")] [SerializeField]
        private float walkSpeed;

        [Header("GoldContainerSettings")] [SerializeField]
        private float maxGold = 10f;

        private GoldContainer goldContainer;

        private void Start()
        {
            goldContainer = new GoldContainer(0, maxGold);

            fsm = new FSM<States, Flags>(States.Idle);

            fsm.AddState<IdleState>(States.Idle, onTickParameters: () => new object[] { targetedMine });
            fsm.AddState<GoToMineState>(States.GoToMine, onTickParameters: () => new object[] { targetedMine, transform, walkSpeed, Time.deltaTime, mineDistance });
            fsm.AddState<MineState>(States.Mine, onTickParameters: () => new object[] { targetedMine, goldContainer, mineSpeed, Time.deltaTime });
            fsm.AddState<ReturnGoldState>(States.ReturnGold,
                onTickParameters: () => new object[] { goldStorage, transform, goldContainer, walkSpeed, Time.deltaTime, mineDistance });

            fsm.SetTransition(States.Idle, Flags.MineFound, States.GoToMine);

            fsm.SetTransition(States.GoToMine, Flags.MiningDistance, States.Mine);

            fsm.SetTransition(States.Mine, Flags.MineLost, States.ReturnGold);
            fsm.SetTransition(States.Mine, Flags.BagFull, States.ReturnGold);

            fsm.SetTransition(States.ReturnGold, Flags.GoldReturned, States.Idle);
        }

        private void Update()
        {
            fsm.Tick();

            goldAmount = goldContainer.GoldAmount;
        }
    }
}