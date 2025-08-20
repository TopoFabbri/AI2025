namespace Game.Mining
{
    public class GoldContainer
    {
        private readonly float maxGold;

        public float GoldAmount { get; private set; }

        public GoldContainer(float goldAmount, float maxGold)
        {
            this.GoldAmount = goldAmount;
            this.maxGold = maxGold;
        }
        
        public bool AddGold(float amount)
        {
            if (maxGold > 0)
            {
                if (GoldAmount + amount > maxGold)
                    return false;
            }
            
            GoldAmount += amount;
            return true;
        }

        public bool GetGold(ref float amount)
        {
            if (GoldAmount < amount)
            {
                amount = GoldAmount;
                GoldAmount = 0;
                return false;
            }
            
            GoldAmount -= amount;
            return true;
        }
        
        public float GetAllGold()
        {
            float amount = GoldAmount;
            GoldAmount = 0f;
            return amount;
        }

        public void ReplenishGold()
        {
            GoldAmount = maxGold;
        }
    }
}