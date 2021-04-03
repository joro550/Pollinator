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
        private float _lastSightLineChange;
        private SpriteRenderer _spriteRenderer;
        private SightLineController _sightLineController;

        private static readonly int IsFlyingHorizontally = Animator.StringToHash("IsFlyingHorizontally");

        private void Awake()
        {
            _animator = GetComponentInChildren<Animator>();
            _spriteRenderer = spriteContainer.GetComponent<SpriteRenderer>();
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
            // Set things back to how they were before we touched them
            spriteContainer.transform.rotation = Quaternion.identity;
            _spriteRenderer.flipY = false;
            
            Movement();
            
            var currentSightLine = _sightLineController.GetCurrentSightLine();
            if (_sightLineController.CanSeePlayer())
            {
                SetCurrentSightLineAnimation(currentSightLine, false);
                return;
            }
            
            _animator.SetBool(IsFlyingHorizontally, false);

            // close old sight line
            SetCurrentSightLineAnimation(currentSightLine, false);

            // new sight line
            currentSightLine = _sightLineController.SightLineChange();

            SetCurrentSightLineAnimation(currentSightLine, true);
            spriteContainer.transform.rotation = Quaternion.Euler(0, currentSightLine.isInverted ? 180 : 0, 0);
        }

        private void SetCurrentSightLineAnimation(SightLine currentSightLine, bool isEnabled)
        {
            if (!string.IsNullOrEmpty(currentSightLine.AnimationFlag()))
                _animator.SetBool(currentSightLine.AnimationFlag(), isEnabled);
        }

        private void Movement()
        {
            var step = speed * Time.deltaTime;

            if (!_sightLineController.CanSeePlayer())
            {
                if (_sightLineController.LastSeenTarget() > waitTime)
                    transform.position = Vector2.MoveTowards(transform.position, startingPosition.position, step);
                return;
            }

            var targetPosition = _sightLineController.GetTargetPosition();
            var dir = targetPosition - spriteContainer.transform.position;
            var angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            
            var rotation = Quaternion.AngleAxis(angle, Vector3.forward);
            spriteContainer.transform.rotation = rotation;
            _animator.SetBool(IsFlyingHorizontally, true);

            _spriteRenderer.flipY = _sightLineController.IsLeftSide();
            transform.position = Vector2.MoveTowards(transform.position, targetPosition, step);
        }

        public void OnCollisionEnter2D(Collision2D other)
        {
            if (other.collider.CompareTag("Player"))
            {
                var player = other.collider.GetComponent<PlayerController>();
                player.Accept(new EnemyVisitor(this));
            }
        }

        public void Reset()
        {
            transform.position = startingPosition.position;
            _sightLineController.Reset();
        }
    }
}
