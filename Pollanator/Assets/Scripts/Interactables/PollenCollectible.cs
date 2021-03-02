using Interactables.Visitors;
using UnityEngine;

namespace Interactables
{
    public class PollenCollectible : Collectible
    {
        [SerializeField] private int pollenCount;
        
        public override void Accept(IVisitor visitor)
        {
            visitor.VisitPollenCollectible(this);
        }

        public int Harvest(int amount)
        {
            if (pollenCount >= amount)
            {
                pollenCount -= amount;
                return amount;
            }
            var harvestAmount = pollenCount;
            pollenCount = 0;
            return harvestAmount;
        }
    }
}