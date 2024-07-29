using AYellowpaper.SerializedCollections;
using BloodField.DTO;
using BloodField.Types;
using Render;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace BloodField.Managers
{
    public class MiniatureManager : MonoBehaviour
    {
        [SerializedDictionary("Card Type", "prefab")]
        public SerializedDictionary<CardType, GameObject> miniaturePrefabs;

        private List<BloodField.Miniatures.Miniature> _miniatures = new List<BloodField.Miniatures.Miniature>();
        private BloodField.Miniatures.Miniature _currentMiniature = null;
        private List<AdditionalStatsDTO> _additionalStats = new List<AdditionalStatsDTO>();

        #region Gets/Sets
        public void AddMiniature(BloodField.Miniatures.Miniature miniature) => _miniatures.Add(miniature);
        public void RemoveMiniature(BloodField.Miniatures.Miniature miniature) => _miniatures.Remove(miniature);
        public bool IsAllMiniaturesFinishAction() => _miniatures.All(f => f.finishAction == true);
        public void SetAllMiniaturesInactive() => _miniatures.ForEach(f => f.SetInactive());
        public void SetAllMiniaturesActive() => _miniatures.ForEach(f => f.SetActive());
        public List<BloodField.Miniatures.Miniature> GetMiniatures() => _miniatures;

        public bool IsOtherMiniature(string id) => _currentMiniature != null && _currentMiniature?._id != id;
        public void SetCurrentMiniature(BloodField.Miniatures.Miniature miniature) => _currentMiniature = miniature;
        public BloodField.Miniatures.Miniature GetCurrentMiniature() => _currentMiniature;

        /// <summary>
        /// this additional stats is used when create a new miniature, is used like references
        /// to receive same stats than the others
        /// </summary>
        public List<AdditionalStatsDTO> GetAdditionalStats() => _additionalStats;
        public void UpdateAddionalStats(ArmyType armyType, StatsType statsType, int value)
        {
            var armyStats = _additionalStats.Find(f => f.type == armyType);

            if (armyStats is null)
            {
                var dictStats = new AdditionalStatsDTO(armyType);
                dictStats.stats[statsType] += value;

                _additionalStats.Add(dictStats);
                return;
            }

            armyStats.stats[statsType] += value;
        }
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
    }
}
