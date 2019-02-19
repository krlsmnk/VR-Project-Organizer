namespace CAVS.ProjectOrganizer.Interation
{
    public interface ISelectable 
    {
        void Select(UnityEngine.GameObject caller);

        void UnSelect(UnityEngine.GameObject caller);
    }
}