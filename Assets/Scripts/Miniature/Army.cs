using Managers;
using Render;
using Tiles;
using UnityEngine;

namespace Miniatures
{
    public class Army : Miniature
    {
        #region Gets/Sets
        public override bool CanAddOnBoard((int y, int x) position) => GameManager.Instance.mapManager.CanSpawnMiniatures(position);
        #endregion

        #region Utils
        protected void ApplyAdditionalStats()
        {
            GameManager.Instance.gamePlayManager.GetAdditionalStats()
                .FindAll(f => f.type == stats.armyType)
                .ForEach(additionalStats =>
                {
                    foreach (var stat in additionalStats.stats)
                        stats.additionalStats[stat.Key] = stat.Value;
                });
        }
        #endregion

        public override void OnCreate(MiniatureCreateMessage miniature)
        {
            base.OnCreate(miniature);
            ApplyAdditionalStats();
        }
    }
}
