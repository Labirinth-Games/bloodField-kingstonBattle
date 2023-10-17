using Render;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Managers
{
    public class GamePlayManager : MonoBehaviour
    {
        [SerializeField] private GameObject kingsPrefab;

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
