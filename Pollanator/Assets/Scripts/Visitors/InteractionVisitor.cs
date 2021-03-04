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
    }
}