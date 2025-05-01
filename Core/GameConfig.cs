namespace Zombris.Core;

public static class GameConfig
{
    public const int GridWidth = 10;
    public const int GridHeight = 10;
    public const int CellSize = 64;
    public const int PieceSize = 32;


    public const int ScreenWidth = GridWidth * CellSize;
    public const int ScreenHeight = GridHeight * CellSize;
}
