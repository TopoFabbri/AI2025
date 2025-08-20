using UnityEngine;

namespace Game.Mining
{
    public class GoldStorage : MonoBehaviour
    {
        [SerializeField] private float goldAmount;
        
        private GoldContainer goldContainer;

        private void Start()
        {
            goldContainer = new GoldContainer(0, 0);
        }

        public void AddGold(float amount)
        {
            goldContainer.AddGold(amount);

            goldAmount = goldContainer.GoldAmount;
        }
    }
}