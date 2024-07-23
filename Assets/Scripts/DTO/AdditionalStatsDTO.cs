using System;
using System.Collections.Generic;
using Enums;

namespace BloodField.DTO
{
    [System.Serializable]
    public class AdditionalStatsDTO
    {
        public ArmyTypeEnum type;
        public Dictionary<StatsTypeEnum, int> stats;

        public AdditionalStatsDTO(ArmyTypeEnum type)
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