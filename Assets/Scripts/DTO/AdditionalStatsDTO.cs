using System;
using System.Collections.Generic;
using BloodField.Types;

namespace BloodField.DTO
{
    [System.Serializable]
    public class AdditionalStatsDTO
    {
        public ArmyType type;
        public Dictionary<StatsType, int> stats;

        public AdditionalStatsDTO(ArmyType type)
        {
            this.type = type;
            stats = new Dictionary<StatsType, int>();

            var enumValues = Enum.GetValues(typeof(StatsType));
            for (var i = 0; i < enumValues.GetLength(0); i++)
            {
                stats.Add((StatsType)enumValues.GetValue(i), 0);
            }
        }
    }
}