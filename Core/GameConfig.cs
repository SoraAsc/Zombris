namespace Zombris.Core;

public static class GameConfig
{
    public const int GridWidth = 50; // 50
    public const int GridHeight = 50; // 50
    public const int CellSize = 21; //14
    public const int PieceSize = 20; // 10


    public const int ScreenWidth = GridWidth * CellSize;
    public const int ScreenHeight = GridHeight * CellSize;

    public enum ZombieType { PrimeZombie, RandomWalkerZombie };
    public enum BlueType { PrimeBlue, RandomWalkerBlue };

    public enum ActorEntityType { Blue, Zombie }
}
