using Controllers;
using Interactables;

namespace Visitors
{
    public class InteractionCollideVisitor : IVisitor
    {
        private readonly bool _isExit;

        public InteractionCollideVisitor(bool isExit = false)
        {
            _isExit = isExit;
        }
        
        public void VisitBaseController(BaseController baseController)
        {
            GameManager.Instance.SetInteractable(_isExit ? null : baseController);
        }

        public void VisitPollenCollectible(PollenCollectible pollenCollectible)
        {
            GameManager.Instance.SetInteractable(_isExit ? null : pollenCollectible);
        }

        public void VisitPlayerController(PlayerController playerController)
        {
            
        }
    }
}