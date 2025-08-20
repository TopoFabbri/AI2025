using UnityEngine;

namespace Game.Mining
{
    public class Mine : MonoBehaviour
    {
        [SerializeField] private float goldAmount;
        [SerializeField] private float goldPerSizeMultiplier = 20f;
        
        private GoldContainer  goldContainer;

        public float GoldAmount => goldContainer.GoldAmount;
        
        private void Start()
        {
            goldAmount = transform.localScale.y * 20;

            goldContainer = new GoldContainer(goldAmount, goldAmount);
        }

        public float GetGold(float minedAmount)
        {
            goldContainer.GetGold(ref minedAmount);
            
            transform.localScale = new Vector3(transform.localScale.x, goldContainer.GoldAmount / goldPerSizeMultiplier, transform.localScale.z);
            
            goldAmount = goldContainer.GoldAmount;
            
            return minedAmount;
        }
    }
}
