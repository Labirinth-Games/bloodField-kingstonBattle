using Miniatures;
using UnityEngine;

public interface ICommand
{
    public void Action((int y, int x) pos);
}