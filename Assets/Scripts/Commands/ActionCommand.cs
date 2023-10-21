using Miniatures;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Commands
{
    public class ActionCommand : MonoBehaviour, ICommand
    {
        public virtual void Action((int y, int x) pos) { }
    }
}
