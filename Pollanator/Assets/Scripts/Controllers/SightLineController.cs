using System;
using UnityEngine;

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
        
        public void SightLineChange()
        {
            _lastSightLineChange += Time.deltaTime;
            
            if (!(_lastSightLineChange >= sightLineChangeTme)) 
                return;
            GetCurrentSightLine().Disable();
            GetNextSightLine().Enable();
            _lastSightLineChange = 0;
        }

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

        public void Disable()
        {
            bottomLeftSightLine.Disable();
            bottomRightSightLine.Disable();;
            topLeftSightLine.Disable();;
            topRightSightLine.Disable();;
        }
    }
}