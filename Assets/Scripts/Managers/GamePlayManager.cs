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
        [SerializeField] private GameObject kingPrefab;
        [SerializeField] private GameObject kingEnemyPrefab;
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
            GameManager.Instance.turnManager.Load();

            MiniatureRender.KingRender(GameManager.Instance.mapManager.GetKingPositions(), kingPrefab);
            //MiniatureRender.KingRender(GameManager.Instance.mapManager.GetKingEnemyPositions(), kingEnemyPrefab);

            // get hand initial
            GameManager.Instance.deckManager.Draw(GameManager.Instance.matchConfig.initialAmountInHand);
        }
    }
}
