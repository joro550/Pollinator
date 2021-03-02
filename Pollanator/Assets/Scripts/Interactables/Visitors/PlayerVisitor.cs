using Controllers;

namespace Interactables.Visitors
{
    public class PlayerVisitor : IVisitor
    {
        private readonly PlayerController _playerController;

        public PlayerVisitor(PlayerController playerController) 
            => _playerController = playerController;

        public void VisitPollenCollectible(PollenCollectible pollenCollectible)
        {
            var harvestSpeed = _playerController.GetHarvestSpeed();
            var harvestAmount = pollenCollectible.Harvest(harvestSpeed);
            _playerController.Harvest(harvestAmount);
        }
    }
}