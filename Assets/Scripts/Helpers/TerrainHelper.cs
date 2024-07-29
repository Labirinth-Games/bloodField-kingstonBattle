using System;
using System.Collections.Generic;
using System.Linq;
using BloodField.Miniatures;
using BloodField.Types;

namespace BloodField.Helpers
{
    public class TerrainHelper
    {

        public static void ApplyDebufferCommon(Miniature miniature, CardSO terrain, int multiply)
        {
            int i = 0;
            var miniatureAdditionalStats = miniature.stats.additionalStats;

            foreach (var terrainStats in terrain.additionalStats)
            {
                miniatureAdditionalStats[terrainStats.Key] += terrainStats.Value * multiply; // mulyiply is used to add or remove value added
                if (terrainStats.Key == StatsType.DEF) miniature.AddHP(terrainStats.Value * multiply);

                UIHelper.AdditionalStatsUIRender($"{terrainStats.Key} {(Math.Sign(terrainStats.Value * multiply) > 0 ? "+" : "-")}{Math.Abs(terrainStats.Value)}", miniature.gameObject, i);
                i++;
            }
        }

        public static void ApplyDebufferWithConditional(Miniature miniature, CardSO terrain, int multiply)
        {
            int i = 0;
            var canExecute = new List<bool>() { };
            var miniatureAdditionalStats = miniature.stats.additionalStats;

            foreach (var conditional in terrain.conditionalStats)
            {
                var func = miniature.stats.GetType().GetMethod($"Get{conditional.Key}");
                var val = (int)func.Invoke(miniature.stats, null);

                canExecute.Add(val > conditional.Value);
            }

            if (!canExecute.All(f => f == true)) return;

            foreach (var terrainStats in terrain.additionalStats)
            {
                miniatureAdditionalStats[terrainStats.Key] += terrainStats.Value * multiply; // mulyiply is used to add or remove value added
                if (terrainStats.Key == StatsType.DEF) miniature.AddHP(terrainStats.Value * multiply);

                UIHelper.AdditionalStatsUIRender($"{terrainStats.Key} {(Math.Sign(terrainStats.Value * multiply) > 0 ? "+" : "-")}{Math.Abs(terrainStats.Value)}", miniature.gameObject, i);
                i++;
            }
        }

    }
}