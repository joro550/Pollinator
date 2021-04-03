using System;
using UnityEngine;
using Visitors;

namespace Controllers
{
    public class SightLineController : MonoBehaviour
    {
        [SerializeField] private float sightLineChangeTme;
        
        [SerializeField] private Controllers.SightLine bottomLeftSightLine;
        [SerializeField] private Controllers.SightLine bottomRightSightLine;
        [SerializeField] private Controllers.SightLine topLeftSightLine;
        [SerializeField] private Controllers.SightLine topRightSightLine;
        
        private float _lastSightLineChange;
        private SightLine _currentSightLine = SightLine.BottomRight;

        private float _lastSeenTarget;
        private Vector3 _targetPosition;

        public void Update()
        {
            if (!CanSeePlayer())
            {
                _lastSeenTarget += Time.deltaTime;
            }
        }

        public float LastSeenTarget() 
            => _lastSeenTarget;

        public Vector3 GetTargetPosition() 
            => _targetPosition;

        public Controllers.SightLine SightLineChange()
        {
            _lastSightLineChange += Time.deltaTime;

            if (!(_lastSightLineChange >= sightLineChangeTme))
                return GetCurrentSightLine();
            
            GetCurrentSightLine().Disable();
            GetNextSightLine().Enable();
            _lastSightLineChange = 0;
            return GetCurrentSightLine();
        }
        
        public bool CanSeePlayer() 
            => _targetPosition != Vector3.zero;

        public Controllers.SightLine GetCurrentSightLine()
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
        
        private Controllers.SightLine GetNextSightLine()
        {
            switch (_currentSightLine)
            {
                case SightLine.BottomRight:
                {
                    _currentSightLine = SightLine.BottomLeft;
                    return bottomLeftSightLine;
                }
                case SightLine.BottomLeft:
                {
                    _currentSightLine = SightLine.TopLeft;
                    return topLeftSightLine;
                }
                case SightLine.TopLeft:
                {
                    _currentSightLine = SightLine.TopRight;
                    return topRightSightLine;
                }
                case SightLine.TopRight:
                {
                    _currentSightLine = SightLine.BottomRight;
                    return bottomRightSightLine;
                }
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public bool IsLeftSide() 
            => _currentSightLine == SightLine.BottomLeft || _currentSightLine == SightLine.TopLeft;

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                _lastSeenTarget = 0;
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
        

        private enum SightLine
        {
            BottomLeft,
            BottomRight,
            TopLeft,
            TopRight
        }

        public void Disable()
        {
            bottomLeftSightLine.Disable();
            bottomRightSightLine.Disable();;
            topLeftSightLine.Disable();;
            topRightSightLine.Disable();;
        }

        public void Reset()
        {
            _targetPosition = Vector2.zero;
            _lastSeenTarget = 0;
        }
    }
}