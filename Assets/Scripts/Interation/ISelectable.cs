namespace CAVS.ProjectOrganizer.Interation
{
    public interface ISelectable 
    {
        /// <summary>
        /// Called whenever the user aims their pointer at and object and pulls
        /// the trigger.
        /// </summary>
        /// <param name="caller"></param>
        void SelectPress(UnityEngine.GameObject caller);

        /// <summary>
        /// Called when the user has released the trigger after previously
        /// pointing at the object and pulling the trigger.
        /// </summary>
        /// <param name="caller"></param>
        void SelectUnpress(UnityEngine.GameObject caller);

        /// <summary>
        /// Called whenever the user begins to point at the object
        /// </summary>
        /// <param name="caller"></param>
        void Hover(UnityEngine.GameObject caller);

        /// <summary>
        /// Called whenever the user is no longer pointing to the object.
        /// </summary>
        /// <param name="caller"></param>
        void UnHover(UnityEngine.GameObject caller);

        /// <summary>
        /// Called whenever the user pulls the trigger and the pointer is not
        /// over this object.
        /// </summary>
        /// <param name="caller"></param>
        void UnSelect(UnityEngine.GameObject caller);
    }
}