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
            if (self is not null) return;
            Debug.Log($"criando army {netId}");

            // create tile config
            self = GameManager.Instance.mapManager.Register(new Tile(miniature.card.type, gameObject), miniature.position);

            GetComponent<SpriteRenderer>().sprite = miniature.card.sprite;

            // setting stats
            stats = Instantiate(miniature.card);
            _hp = stats.GetDEF();

            ApplyAdditionalStats();
            Subscribers();

            // attachment the army on mouse to set position
            if (isOwned)
                GameManager.Instance.miniatureMouseHelper.Attachment(gameObject);
        }
    }
}
