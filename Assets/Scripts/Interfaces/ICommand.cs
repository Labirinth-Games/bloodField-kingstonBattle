using System.Collections.Generic;

public interface ICommand
{
    public void Action((int y, int x) pos);
    public void Action(List<(int y, int x)> pos);
}