using Controllers;
using Interactables;

namespace Visitors
{
    public class InteractionVisitor : IVisitor
    {
        private readonly PlayerController _playerController;

        public InteractionVisitor(PlayerController playerController) 
            => _playerController = playerController;

        public void VisitBaseController(BaseController baseController)
        {
            var harvestSpeed = _playerController.GetDepositAmount();
            baseController.Deposit(harvestSpeed);
        }

        public void VisitPollenCollectible(PollenCollectible pollenCollectible)
        {
            var harvestSpeed = _playerController.GetHarvestSpeed();
            var harvestAmount = pollenCollectible.Harvest(harvestSpeed);
            _playerController.Harvest(harvestAmount);
        }

        public void VisitPlayerController(PlayerController playerController)
        {
            
        }
    }
    
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