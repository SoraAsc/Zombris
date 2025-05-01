using Microsoft.Xna.Framework;

namespace Zombris.GridSystem;

public class Cell
{
    public Point Position { get; }
    public object Occupant { get; set; }

}