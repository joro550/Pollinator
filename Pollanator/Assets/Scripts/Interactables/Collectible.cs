using Controllers;
using UnityEngine;
using Visitors;

namespace Interactables
{
    public abstract class Collectible : MonoBehaviour
    {
        public void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.CompareTag("Player")) 
                return;

            var controller = other.GetComponent<PlayerController>();
            controller.HasEnteredHarvestRange(this);
            Accept(new InteractionCollideVisitor());
        }

        public void OnTriggerExit2D(Collider2D other)
        {
            if (!other.CompareTag("Player")) 
                return;
            
            var controller = other.GetComponent<PlayerController>();
            controller.SetCanHarvest(false);
            Accept(new InteractionCollideVisitor(true));
        }

        public abstract void Accept(IVisitor visitor);
    }
}