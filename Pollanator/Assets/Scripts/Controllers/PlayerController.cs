using Visitors;
using UnityEngine;
using Interactables;

namespace Controllers
{
    public class PlayerController : MonoBehaviour
    {
        [Header("Interactable")]
        [SerializeField] private float pollenLimit = 100;
        [SerializeField] private float harvestSpeed = 10;
        [SerializeField] private float depositSpeed = 10;
        [SerializeField] private float actionWaitTime = 0.5f;
        
        [Header("Sprite")]
        [SerializeField] private SpriteRenderer spriteRenderer;

        [Header("Player")] 
        [SerializeField] private int lives = 3;
        [SerializeField] private Transform baseLocation;

        private float _timer;
        private bool _canHarvest;
        private bool _canDeposit;
        private float _pollenCount;
        private Vector3 _originalScale;

        private Collectible _collectible;
        private BaseController _currentBase;

        public void Awake()
        {
            GetComponent<Movement>();
            _originalScale = transform.localScale;
        }

        public void Update()
        {
            Interactions();
        }

        private void Interactions()
        {
            _timer += Time.deltaTime;
            
            if (_timer < actionWaitTime)
                return;

            if (!Input.GetKey(KeyCode.E))
            {
                transform.localScale = _originalScale;
                return;
            }
            
            if (_canHarvest)
                _collectible.Accept(new InteractionVisitor(this));
            else if (_canDeposit)
                _currentBase.Accept(new InteractionVisitor(this));
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
        
        public float GetPollenCount()
            => _pollenCount;

        public float GetPollenLimit() 
            => pollenLimit;

        public float GetHarvestSpeed()
        {
            var limit = pollenLimit - _pollenCount;
            return limit < harvestSpeed ? limit : harvestSpeed;
        }

        public int GetLives()
            => lives;

        public void Harvest(float harvestAmount)
        {
            _pollenCount += harvestAmount;
            transform.localScale = _originalScale / 2;
        }

        public float GetDepositAmount()
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

        public void Accept(IVisitor visitor) 
            => visitor.VisitPlayerController(this);

        public void HasBeenHit()
        {
            lives -= 1;
            transform.position = baseLocation.position;
        }
    }
}