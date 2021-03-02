namespace Controllers.Visitors
{
    public interface IVisitor
    {
        void VisitBaseController(BaseController baseController);
    }
}