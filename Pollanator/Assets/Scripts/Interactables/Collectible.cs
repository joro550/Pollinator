using Controllers;
using Interactables.Visitors;
using UnityEngine;

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
        }

        public void OnTriggerExit2D(Collider2D other)
        {
            if (!other.CompareTag("Player")) 
                return;
            
            var controller = other.GetComponent<PlayerController>();
            controller.SetCanHarvest(false);
        }

        public abstract void Accept(IVisitor visitor);
    }
}