using UnityEngine;
using Controllers.Visitors;

namespace Controllers
{
    public class BaseController : MonoBehaviour
    {
        private int _pollenCount;
        
        public void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.CompareTag("Player")) return;
            
            var controller = other.GetComponent<PlayerController>();
            controller.HasEnteredDepositRange(this);
        }
        
        public void OnTriggerExit2D(Collider2D other)
        {
            if (!other.CompareTag("Player")) return;
            
            var controller = other.GetComponent<PlayerController>();
            controller.SetDeposit(false);
        }
        
        public int GetPollenCount() 
            => _pollenCount;

        public void Deposit(int pollen) 
            => _pollenCount += pollen;

        public void Accept(IVisitor visitor) 
            => visitor.VisitBaseController(this);
    }
}