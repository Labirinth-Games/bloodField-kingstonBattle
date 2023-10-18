using Render;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        #endregion

        public void StartMatch()
        {
            // load deck to start match
            GameManager.Instance.deckManager.Load();
            MapRender.KingRender(GameManager.Instance.mapManager.GetKingPositions(), kingsPrefab);


            // get hand initial
            var cards = GameManager.Instance.deckManager.Draw(GameManager.Instance.matchConfig.initialAmountInHand);
            GameManager.Instance.cardManager.Create(cards);
            GameManager.Instance.player.SetCardHand(cards);
        }
    }
}
