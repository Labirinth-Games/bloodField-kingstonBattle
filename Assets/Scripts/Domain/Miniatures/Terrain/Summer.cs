using BloodField.Domain.Commands;
using BloodField.Managers;

namespace BloodField.Miniatures.Terrains
{
    public class Summer : ActionCommand
    {
        public override void Action() => GameManager.Instance.matchManager.RemoveTerrainPermanent();
    }
}