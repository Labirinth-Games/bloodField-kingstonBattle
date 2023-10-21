using Miniatures;
using Render;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace Managers
{
    public class GamePlayManager : MonoBehaviour
    {
        [SerializeField] private GameObject kingsPrefab;
        [SerializeField] private Miniature currentMiniature = null;

        #region Gets/Sets
        public bool IsOtherMiniature() => currentMiniature != null;
        public void SetCurrentMiniature(Miniature miniature) => currentMiniature = miniature;
        #endregion

        public void MyTurn()
        {
            if(GameManager.Instance.deckManager.CanDraw())
                GameManager.Instance.deckManager.Draw();

           GameManager.Instance.miniatureManager.InitTurnForAll();
        }

        public void StartMatch()
        {
            // load deck to start match
            GameManager.Instance.deckManager.Load();
            MiniatureRender.KingRender(GameManager.Instance.mapManager.GetKingPositions(), kingsPrefab);


            // get hand initial
            GameManager.Instance.deckManager.Draw(GameManager.Instance.matchConfig.initialAmountInHand);
        }
    }
}
