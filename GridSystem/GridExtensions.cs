using System;
using Microsoft.Xna.Framework;
using Zombris.Entities;

namespace Zombris.GridSystem;

public static class GridExtensions
{
    public static bool TryPlaceCheck(this Grid grid, int x, int y)
    {
        try
        {
            var dummy = new ActorEntity("check", new Point(x, y), Core.GameConfig.ActorEntityType.Blue);
            return grid.TryMove(dummy, x, y);
        }
        catch { return false; }
    }
}