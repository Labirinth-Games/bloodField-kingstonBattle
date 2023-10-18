using System.Collections;
using System.Collections.Generic;
using UI;
using UnityEngine;

namespace Managers
{
    public class TurnManager : MonoBehaviour
    {
        private bool _isTurnPlayer = true;

        #region Gets/Sets
        public bool IsMyTurn() => _isTurnPlayer;
        #endregion

        public void EndTurn()
        {
            _isTurnPlayer = !_isTurnPlayer;
        }

        public void Load()
        {

        }
    }
}
