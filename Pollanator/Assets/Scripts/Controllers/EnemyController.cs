using System;
using UnityEngine;

namespace Controllers
{
    public class EnemyController : MonoBehaviour
    {
        [SerializeField] private float speed = 10.0f;
        [SerializeField] private float waitTime = 3.0f;
        [SerializeField] private float sightLineChangeTme;
        [SerializeField] private Transform startingPosition;
        
        [SerializeField] private SighLine bottomLeftSightLine;
        [SerializeField] private SighLine bottomRightSightLine;
        [SerializeField] private SighLine topLeftSightLine;
        [SerializeField] private SighLine topRightSightLine;

        private float _lastSeenPlayer;
        private Vector2 _targetPosition;
        private float _lastSightLineChange;
        private SightLine _currentSightLine = SightLine.BottomRight;
    
        public void Start()
        {
            bottomLeftSightLine.Disable();
            bottomRightSightLine.Disable();
            topLeftSightLine.Disable();
            topRightSightLine.Disable();

            var current = GetCurrentSightLine();
            current.Enable();
        }

        private void Update()
        {
            Movement();
            
            if (!CanSeePlayer())
                SightLineChange();
        }

        private bool CanSeePlayer()
        {
            return _targetPosition != Vector2.zero;
        }

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

        private void SightLineChange()
        {
            _lastSightLineChange += Time.deltaTime;
            
            if (!(_lastSightLineChange >= sightLineChangeTme)) 
                return;
            GetCurrentSightLine().Disable();
            GetNextSightLine().Enable();
            _lastSightLineChange = 0;
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

        private SighLine GetCurrentSightLine()
        {

            switch (_currentSightLine)
            {
                case SightLine.BottomRight:
                    return bottomRightSightLine;
                case  SightLine.BottomLeft:
                    return bottomLeftSightLine;
                case SightLine.TopLeft:
                    return topLeftSightLine;
                case SightLine.TopRight:
                    return topRightSightLine;
                default:
                    throw new ArgumentException();
            };
        }

        private SighLine GetNextSightLine()
        {
            switch (_currentSightLine)
            {
                case SightLine.BottomRight:
                {
                    _currentSightLine = SightLine.BottomLeft;
                    return bottomLeftSightLine;
                    break;
                }
                case SightLine.BottomLeft:
                {
                    _currentSightLine = SightLine.TopLeft;
                    return topLeftSightLine;
                    break;
                }
                case SightLine.TopLeft:
                {
                    _currentSightLine = SightLine.TopRight;
                    return topRightSightLine;
                    break;
                }
                case SightLine.TopRight:
                {
                    _currentSightLine = SightLine.BottomRight;
                    return bottomRightSightLine;
                    break;
                }
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private enum SightLine
        {
            BottomLeft,
            BottomRight,
            TopLeft,
            TopRight
        }

    }
}
