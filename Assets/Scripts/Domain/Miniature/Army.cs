using BloodField.Managers;

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
            GameManager.Instance.miniatureManager.GetAdditionalStats()
                .FindAll(f => f.type == stats.armyType)
                .ForEach(additionalStats =>
                {
                    foreach (var stat in additionalStats.stats)
                    {
                        stats.additionalStats[stat.Key] = stat.Value;
                        if(stat.Key == Enums.StatsTypeEnum.DEF) AddHP(stat.Value);
                    }
                });
        }
        #endregion

        public override void OnCreate(CardSO card, string ownerId, int y, int x)
        {
            base.OnCreate(card, ownerId, y, x);
            ApplyAdditionalStats();

            // GetComponent<SpriteRenderer>().sprite = SpriteColorDynamic.ChangeColorBase(GetComponent<SpriteRenderer>().sprite, new SpriteColors() { primary = stats.primaryColor, secundary = stats.secundaryColor });
        }
    }
}
