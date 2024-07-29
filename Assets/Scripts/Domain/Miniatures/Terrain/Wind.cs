using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BloodField.Domain.Commands;
using BloodField.Managers;
using BloodField.Miniatures;
using UnityEngine;

namespace BloodField.Miniature.Terrains
{
    public class Wind : ActionCommand
    {
        public override async void Action(List<(int y, int x)> positions, Action finishCallback)
        {
            GameManager.Instance.mapManager.FindByPosition(positions)
                .FindAll(e => e.IsArmy())
                .ForEach(army =>
                {
                    army.gameObject.GetComponent<BloodField.Miniatures.Miniature>().self.MoveBack(2);
                });

            await Task.Delay(500);

            finishCallback();
        }
    }
}