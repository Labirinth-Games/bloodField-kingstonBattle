using BloodField.Managers;

namespace Miniatures
{
    public class Command : Miniature
    {
        #region Gets/Sets
        public override bool CanAddOnBoard((int y, int x) position) => GameManager.Instance.mapManager.CanSpawnMiniatures(position);
        #endregion

        public override void AddOnBoard((int y, int x) pos)
        {
            stats.commandScript.Action(pos); // call the command specific
            Destroy(gameObject, .2f);
        }
    }
}
