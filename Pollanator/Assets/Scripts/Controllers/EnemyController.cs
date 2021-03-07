using Visitors;
using UnityEngine;

namespace Controllers
{
    [RequireComponent(typeof(SightLineController))]
    public class EnemyController : MonoBehaviour
    {
        [SerializeField] private float speed = 10.0f;
        [SerializeField] private float waitTime = 3.0f;
        [SerializeField] private Transform startingPosition;
        [SerializeField] private GameObject spriteContainer;
        
        private Animator _animator;
        private float _lastSeenPlayer;
        private Vector2 _targetPosition;
        private float _lastSightLineChange;
        private SightLineController _sightLineController;


        private void Awake()
        {
            _animator = GetComponentInChildren<Animator>();
        }

        public void Start()
        {
            _sightLineController = GetComponent<SightLineController>();
            _sightLineController.Disable();

            var current = _sightLineController.GetCurrentSightLine();
            current.Enable();
        }

        private void Update()
        {
            Movement();

            if (CanSeePlayer()) return;

            // close old sight line
            var current = _sightLineController.GetCurrentSightLine();
            if(!string.IsNullOrEmpty(current.AnimationFlag()))
                _animator.SetBool(current.AnimationFlag(), false);

            // new sight line
            _sightLineController.SightLineChange();
            current = _sightLineController.GetCurrentSightLine();

            if(!string.IsNullOrEmpty(current.AnimationFlag()))
                _animator.SetBool(current.AnimationFlag(), true);

            spriteContainer.transform.rotation = Quaternion.Euler(0, current.isInverted ? 180 : 0, 0);
        }

        private bool CanSeePlayer() 
            => _targetPosition != Vector2.zero;

        private void Movement()
        {
            var step = speed * Time.deltaTime;

            if (!CanSeePlayer())
            {
                _lastSeenPlayer += Time.deltaTime;
                if (_lastSeenPlayer > waitTime)
                    transform.position = Vector2.MoveTowards(transform.position, startingPosition.position, step);

                return;
            }

            transform.position = Vector2.MoveTowards(transform.position, _targetPosition, step);
        }



        public void OnCollisionEnter2D(Collision2D other)
        {
            if (other.collider.CompareTag("Player"))
            {
                var player = other.collider.GetComponent<PlayerController>();
                player.Accept(new EnemyVisitor(this));
            }
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                _lastSeenPlayer = 0;
                _targetPosition = other.transform.position;
            }
        }

        private void OnTriggerStay2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                _targetPosition = other.transform.position;
            }
        }
    
        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                _targetPosition = Vector2.zero;
            }
        }

        public void Reset()
        {
            transform.position = startingPosition.position;
            _targetPosition = Vector2.zero;
            _lastSeenPlayer = 0;
        }
    }
}
