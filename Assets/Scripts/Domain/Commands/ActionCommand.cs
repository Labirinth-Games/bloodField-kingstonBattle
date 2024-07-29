using System;
using System.Collections.Generic;
using UnityEngine;

namespace BloodField.Domain.Commands
{
    public class ActionCommand : MonoBehaviour, ICommand
    {
        public virtual void Action((int y, int x) pos) { }
        public virtual void Action(List<(int y, int x)> pos) { }
        public virtual void Action(List<(int y, int x)> pos, Action finishCallback) { }
        public virtual void Action(List<(int y, int x)> pos, CardSO stats, Action finishCallback) { }
    }
}
