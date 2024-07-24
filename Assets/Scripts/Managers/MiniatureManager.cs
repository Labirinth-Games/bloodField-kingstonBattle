using AYellowpaper.SerializedCollections;
using BloodField.DTO;
using Enums;
using Miniatures;
using Render;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace BloodField.Managers
{
    public class MiniatureManager : MonoBehaviour
    {
        [SerializedDictionary("Card Type", "prefab")]
        public SerializedDictionary<CardTypeEnum, GameObject> miniaturePrefabs;

        private List<Miniature> _miniatures = new List<Miniature>();
        private Miniature _currentMiniature = null;
        private List<AdditionalStatsDTO> _additionalStats = new List<AdditionalStatsDTO>();

        #region Gets/Sets
        public void AddMiniature(Miniature miniature) => _miniatures.Add(miniature);
        public void RemoveMiniature(Miniature miniature) => _miniatures.Remove(miniature);
        public bool IsAllMiniaturesFinishAction() => _miniatures.All(f => f.finishAction == true);
        public void SetAllMiniaturesInactive() => _miniatures.ForEach(f => f.SetInactive());
        public void SetAllMiniaturesActive() => _miniatures.ForEach(f => f.SetActive());
        public List<Miniature> GetMiniatures() => _miniatures;

        public bool IsOtherMiniature(string id) => _currentMiniature != null && _currentMiniature?._id != id;
        public void SetCurrentMiniature(Miniature miniature) => _currentMiniature = miniature;
        public Miniature GetCurrentMiniature() => _currentMiniature;

        public List<AdditionalStatsDTO> GetAdditionalStats() => _additionalStats;
        public void UpdateAddionalStats(ArmyTypeEnum armyType, StatsTypeEnum statsType, int value) =>
            _additionalStats
                .FindAll(f => f.type == armyType)
                .ForEach(f => f.stats[statsType] += value);
        #endregion

        public void Build(CardSO stats)
        {
            GameObject prefab;

            if (!miniaturePrefabs.TryGetValue(stats.type, out prefab))
            {
                Debug.LogWarning($"This miniature {stats.type} not found prefab", this);
                return;
            };

            MiniatureRender.Render(stats, prefab);            
        }

        // private void LoadHost()
        // {


        //     // auto generate to create stats additional for all armies on deck
        //     // additionalStats = new List<AdditionalStats>();
        //     // GameManager.Instance.deckManager.GetDeck()
        //     //     .ToList()
        //     //     .FindAll(f => f.type == CardTypeEnum.Army)
        //     //     .ForEach(f =>
        //     //     {
        //     //         if (!additionalStats.Exists(e => e.type == f.armyType))
        //     //             additionalStats.Add(new AdditionalStats(f.armyType));
        //     //     });
        // }
    }
}
