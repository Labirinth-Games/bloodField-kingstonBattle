using Helpers;
using Managers;
using Render;
using System.Collections;
using System.Collections.Generic;
using Tiles;
using UnityEngine;

namespace Miniatures
{
    public class Command : Miniature
    {
        #region Gets/Sets
        public override bool CanAddOnBoard((int y, int x) position) => GameManager.Instance.mapManager.CanSpawnUntilMiddleMiniatures(position);
        #endregion

        public override void AddOnBoard((int y, int x) pos)
        {
            stats.commandScript.Action(pos); // call the command specific
            Destroy(gameObject, .2f);
        }
    }
}
