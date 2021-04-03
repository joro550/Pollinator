using Interactables;
using Visitors;
using UnityEngine;
using UnityEngine.UI;

namespace Controllers
{
    public class BaseController : MonoBehaviour, IInteractable
    {
        [SerializeField] private int beeAddThreshold;
        [SerializeField] private Sprite sliderImage;
        
        private float _pollenCount;
        
        public void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.CompareTag("Player")) return;
            
            var controller = other.GetComponent<PlayerController>();
            controller.HasEnteredDepositRange(this);
            Accept(new InteractionCollideVisitor());
        }
        
        public void OnTriggerExit2D(Collider2D other)
        {
            if (!other.CompareTag("Player")) return;
            
            var controller = other.GetComponent<PlayerController>();
            controller.SetDeposit(false);
            Accept(new InteractionCollideVisitor(true));
        }
        
        public float GetPollenCount() 
            => _pollenCount;

        public void Deposit(float pollen) 
            => _pollenCount += pollen;

        public void Accept(IVisitor visitor) 
            => visitor.VisitBaseController(this);

        public float Max => beeAddThreshold;
        public float Current => _pollenCount;
        public Sprite Image => sliderImage;
    }
}