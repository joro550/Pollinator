using Controllers;
using Interactables;

namespace Visitors
{
    public interface IVisitor
    {
        void VisitBaseController(BaseController baseController);
        void VisitPollenCollectible(PollenCollectible pollenCollectible);
    }
}