using AYellowpaper.SerializedCollections;
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

        #region Gets/Sets
        public void AddMiniature(Miniature miniature) => _miniatures.Add(miniature);
        public void RemoveMiniature(Miniature miniature) => _miniatures.Remove(miniature);
        public bool IsAllMiniaturesFinishAction() => _miniatures.All(f => { 
            Debug.Log($"info {f.name} foi? {f.finishAction}");
            return f.finishAction == true; });
        public void SetAllMiniaturesInactive() => _miniatures.ForEach(f => f.SetInactive());
        public void SetAllMiniaturesActive() => _miniatures.ForEach(f => f.SetActive());
        public List<Miniature> GetMiniatures() => _miniatures;
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
