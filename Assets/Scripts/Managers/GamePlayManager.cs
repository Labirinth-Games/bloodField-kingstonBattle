using AYellowpaper.SerializedCollections;
using Enums;
using Generators;
using Helpers;
using Miniatures;
using Render;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

namespace Managers
{
    public class GamePlayManager : MonoBehaviour
    {
        [SerializeField] private GameObject kingPrefab;
        [SerializeField] private GameObject kingEnemyPrefab;
        [SerializeField] private Miniature currentMiniature = null;
        [SerializeField] private List<AdditionalStats> additionalStats;

        #region Gets/Sets
        public bool IsOtherMiniature(string id) => currentMiniature != null && currentMiniature?._id != id;
        public void SetCurrentMiniature(Miniature miniature) => currentMiniature = miniature;
        public Miniature GetCurrentMiniature() => currentMiniature;
        public List<AdditionalStats> GetAdditionalStats() => additionalStats;
        public void UpdateAddionalStats(ArmyTypeEnum armyType, StatsTypeEnum statsType, int value) =>
            additionalStats
                .FindAll(f => f.type == armyType)
                .ForEach(f => f.stats[statsType] += value);
        #endregion

        public void MyTurn()
        {
            if (GameManager.Instance.deckManager.CanDraw())
                GameManager.Instance.deckManager.Draw();
        }

        public void StartMatch()
        {
            // load deck to start match
            GameManager.Instance.deckManager.Load();
            GameManager.Instance.turnManager.Load();

            MiniatureRender.KingRender(GameManager.Instance.mapManager.GetKingPositions(), kingPrefab);
            //MiniatureRender.KingRender(GameManager.Instance.mapManager.GetKingEnemyPositions(), kingEnemyPrefab);

            // if debug mode active get cards defined on list
            if(GameManager.Instance.isDebug)
                GetComponent<DeckDebug>().DeckTest();

            // get hand initial
            GameManager.Instance.deckManager.Draw(GameManager.Instance.gameSettings.initialAmountInHand);

            // auto generate to create stats additional for all armys on deck
            additionalStats = new List<AdditionalStats>();
            GameManager.Instance.deckManager.GetDeck()
                .ToList()
                .FindAll(f => f.type == CardTypeEnum.Army)
                .ForEach(f =>
                {
                    if (!additionalStats.Exists(e => e.type == f.armyType))
                        additionalStats.Add(new AdditionalStats(f.armyType));
                });


            // subscribers
            GameManager.Instance.turnManager.OnStartTurnPlayer.AddListener(MyTurn);
        }
    }

    [System.Serializable]
    public class AdditionalStats
    {
        public ArmyTypeEnum type;
        public Dictionary<StatsTypeEnum, int> stats;

        public AdditionalStats(ArmyTypeEnum type)
        {
            this.type = type;
            stats = new Dictionary<StatsTypeEnum, int>();

            var enumValues = Enum.GetValues(typeof(StatsTypeEnum));
            for (var i = 0; i < enumValues.GetLength(0); i++)
            {
                stats.Add((StatsTypeEnum)enumValues.GetValue(i), 0);
            }
        }
    }
}
