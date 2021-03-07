using Visitors;
using UnityEngine;
using Interactables;

namespace Controllers
{
    public class PlayerController : MonoBehaviour
    {
        [Header("Interactable")]
        [SerializeField] private int pollenLimit = 100;
        [SerializeField] private int harvestSpeed = 10;
        [SerializeField] private int depositSpeed = 10;
        [SerializeField] private float actionWaitTime = 0.5f;
        
        [Header("Sprite")]
        [SerializeField] private SpriteRenderer spriteRenderer;

        [Header("Player")] 
        [SerializeField] private int lives = 3;
        [SerializeField] private Transform baseLocation;

        private float _timer;
        private bool _canHarvest;
        private bool _canDeposit;
        private int _pollenCount;
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
        
        public int GetPollenCount()
            => _pollenCount;

        public int GetPollenLimit() 
            => pollenLimit;

        public int GetHarvestSpeed()
        {
            var limit = pollenLimit - _pollenCount;
            return limit < harvestSpeed ? limit : harvestSpeed;
        }

        public int GetLives()
            => lives;

        public void Harvest(int harvestAmount)
        {
            _pollenCount += harvestAmount;
            transform.localScale = _originalScale / 2;
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

        public void Accept(IVisitor visitor) 
            => visitor.VisitPlayerController(this);

        public void HasBeenHit()
        {
            lives -= 1;
            transform.position = baseLocation.position;
        }

        public void Reset()
        {
            
        }
    }
}