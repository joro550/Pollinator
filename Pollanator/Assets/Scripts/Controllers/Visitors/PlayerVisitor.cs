namespace Controllers.Visitors
{
    public class PlayerVisitor : IVisitor
    {
        private readonly PlayerController _playerController;

        public PlayerVisitor(PlayerController playerController) 
            => _playerController = playerController;

        public void VisitBaseController(BaseController baseController)
        {
            var harvestSpeed = _playerController.GetDepositAmount();
            baseController.Deposit(harvestSpeed);
        }
    }
}