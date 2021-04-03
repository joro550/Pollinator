using Controllers;
using Interactables;

namespace Visitors
{
    public class EnemyVisitor : IVisitor
    {
        private readonly EnemyController _enemyController;

        public EnemyVisitor(EnemyController playerController) 
            => _enemyController = playerController;

        public void VisitBaseController(BaseController baseController)
        {
        }

        public void VisitPollenCollectible(PollenCollectible pollenCollectible)
        {
        }

        public void VisitPlayerController(PlayerController playerController)
        {
            playerController.HasBeenHit();
            _enemyController.Reset();
        }
    }
}