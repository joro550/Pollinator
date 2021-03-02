using UnityEngine;
using Interactables;
using Interactables.Visitors;

namespace Controllers
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private int pollenLimit = 100;
        [SerializeField] private int harvestSpeed = 10;
        [SerializeField] private int depositSpeed = 10;
        [SerializeField] private float actionWaitTime = 0.5f;
        [SerializeField] private SpriteRenderer spriteRenderer;

        private bool _canHarvest;
        private bool _canDeposit;
        private int _pollenCount;
        private float _timer;

        private Collectible _collectible;
        private BaseController _currentBase;
        private Movement _movement;

        public void Awake() 
            => _movement = GetComponent<Movement>();

        public void Update()
        {
            spriteRenderer.color = _canHarvest ? Color.blue :
                _canDeposit ? Color.red : Color.white;


            Interactions();
        }

        private void Interactions()
        {
            _timer += Time.deltaTime;
            if (_timer < actionWaitTime)
                return;

            if (!Input.GetKey(KeyCode.E)) 
                return;
            
            if (_canHarvest)
                _collectible.Accept(new PlayerVisitor(this));
            else if (_canDeposit)
                _currentBase.Accept(new Controllers.Visitors.PlayerVisitor(this));
            _timer = 0;
        }

        public void HasEnteredHarvestRange(Collectible collectible)
        {
            _canHarvest = true;
            _collectible = collectible;
        }
        
        public void SetCanHarvest(bool canHarvest) 
            => _canHarvest = canHarvest;

        public void SetDeposit(bool deposit) 
            => _canDeposit = deposit;

        public void HasEnteredDepositRange(BaseController baseController)
        {
            _canDeposit = true;
            _currentBase = baseController;
        }
        
        public int GetPollenCount()
            => _pollenCount;
        
        public int GetHarvestSpeed()
        {
            var limit = pollenLimit - _pollenCount;
            return limit < harvestSpeed ? limit : harvestSpeed;
        }

        public void Harvest(int harvestAmount)
        {
            _pollenCount += harvestAmount;
        }

        public int GetDepositAmount()
        {
            if (_pollenCount < depositSpeed)
            {
                var pollenToReturn = _pollenCount;
                _pollenCount = 0;
                return pollenToReturn;
            }

            _pollenCount -= depositSpeed;
            return depositSpeed;
        }
    }
}