using BloodField.Managers;
using UnityEngine;

namespace BloodField.Miniatures
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

        public override void OnCreate(CardSO card, string ownerId, int y, int x, bool isAttachment)
        {
            base.OnCreate(card, ownerId, y, x, isAttachment);

            GetComponent<SpriteRenderer>().sprite = card.sprite;
        }
    }
}
