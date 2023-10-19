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
        [SerializeField] private List<Miniature> miniatures = new List<Miniature>();
        [SerializeField] private Miniature currentMiniature = null;

        #region Gets/Sets
        public bool IsOtherMiniature() => currentMiniature != null;
        public void SetCurrentMiniature(Miniature miniature) => currentMiniature = miniature;
        public void AddMiniature(Miniature miniature) => miniatures.Add(miniature);
        public bool IsAllMiniaturesFinish() => miniatures.All(f => f.finishAction == true);
        #endregion
        public void MyTurn()
        {
            if(GameManager.Instance.deckManager.CanDraw())
                GameManager.Instance.deckManager.Draw();

            foreach (Miniature miniature in miniatures)
            {
                miniature.MyTurn();
            }
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
