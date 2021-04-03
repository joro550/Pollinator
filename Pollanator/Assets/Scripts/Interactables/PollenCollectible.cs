using Visitors;
using UnityEngine;

namespace Interactables
{
    public class PollenCollectible : Collectible, IInteractable
    {
        [SerializeField] private float maxPollen;
        [SerializeField] private float pollenCount;
        [SerializeField] private Sprite sliderImage;
        [SerializeField] private ParticleSystem particleSystem;
        
        public override void Accept(IVisitor visitor)
        {
            visitor.VisitPollenCollectible(this);
        }

        public void Update()
        {
            if (pollenCount <= 0)
                particleSystem.Stop();
        }

        public float Harvest(float amount)
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

        public float Max => maxPollen;
        public float Current => pollenCount;
        public Sprite Image => sliderImage;
    }
}