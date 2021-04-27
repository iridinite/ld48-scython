/// <summary>
/// Collection of maps
/// </summary>
public static class MapList
{

    public const int F = 1; // Basic floor
    public const int Q = 2; // Barren floor (cannot build)
    public const int P = 3; // Portal Core
    public const int S = 4; // Siphon

    public static readonly int[] k_Map01 =
    {
        F, F, F, F, Q, Q, Q, F, F, F, 0,
        F, F, S, 0, 0, Q, 0, 0, S, F, F,
        F, F, 0, 0, 0, Q, 0, 0, F, F, F,
        F, F, F, F, Q, Q, Q, F, F, Q, F,
        F, F, F, F, Q, P, Q, F, F, F, F,
        F, F, F, F, Q, Q, Q, 0, F, F, F,
        Q, F, F, F, F, Q, 0, 0, F, F, F,
        F, F, S, F, F, Q, F, F, S, F, Q,
        F, F, F, F, F, Q, F, F, F, F, Q,
        0, F, Q, F, F, F, F, F, F, Q, 0,
        0, F, F, F, F, F, F, F, F, Q, 0,
    };

    public static readonly int[] k_Map02 =
    {
        0, F, F, F, F, F, Q, 0, F, F, F,
        F, F, F, S, F, F, Q, 0, F, S, F,
        F, F, F, F, F, F, Q, 0, 0, F, F,
        F, F, F, F, F, Q, Q, Q, 0, 0, F,
        F, Q, Q, Q, Q, Q, P, Q, Q, Q, Q,
        0, 0, 0, 0, 0, Q, Q, Q, 0, F, Q,
        F, F, F, F, 0, 0, Q, 0, 0, F, F,
        F, F, F, S, F, F, Q, F, F, S, F,
        0, F, F, F, F, F, Q, F, F, F, F,
        0, F, F, F, F, Q, Q, Q, F, F, F,
        0, 0, 0, F, F, F, F, F, F, F, 0,
    };

    public static readonly int[] k_Map03 =
{
        0, F, F, F, F, F, Q, F, F, F, 0,
        F, F, F, F, S, F, F, F, F, F, S,
        F, F, F, F, F, F, F, F, F, 0, 0,
        0, F, F, F, F, 0, 0, F, F, 0, 0,
        F, F, F, Q, Q, Q, Q, Q, Q, Q, 0,
        F, Q, Q, Q, Q, Q, Q, Q, P, Q, 0,
        F, F, F, Q, Q, Q, Q, Q, Q, Q, 0,
        0, F, F, 0, F, F, F, 0, 0, 0, 0,
        Q, F, F, F, F, F, Q, F, F, 0, 0,
        Q, F, F, F, S, F, F, F, F, F, S,
        0, F, F, F, F, F, F, F, F, F, 0,
    };

    public static readonly int[] k_Map04 =
{
        F, F, F, F, S, F, F, F, 0, 0, 0,
        F, 0, F, F, F, F, F, Q, Q, Q, 0,
        F, F, F, F, 0, 0, F, Q, P, Q, 0,
        F, S, F, F, F, Q, Q, Q, Q, Q, 0,
        F, F, Q, Q, Q, Q, P, Q, 0, F, F,
        F, F, F, F, F, Q, Q, Q, 0, F, F,
        F, F, F, F, Q, F, F, 0, 0, F, S,
        0, F, F, F, F, F, F, F, Q, F, F,
        0, F, S, F, F, F, F, F, F, F, F,
        0, F, F, F, Q, F, F, S, F, 0, F,
        0, 0, 0, 0, Q, F, F, F, F, F, F,
    };

    // Maps must be listed in this array to be usable from the main menu
    public static int[][] m_MapList =
    {
        k_Map01,
        k_Map02,
        k_Map03,
        k_Map04,
    };

    // The game will load the map assigned here
    public static int[] ActiveMap = k_Map01;

}