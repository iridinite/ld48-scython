/// <summary>
/// Balance variables
/// </summary>
public static class Tuning
{

    // Size of the map in tiles per axis
    public const int k_MapSize = 11;

    // Starting money value
    public const int k_StartingMoney = 2000;

    // Percentage (0-1) of the current layer's "remaining" siphon output that will be instantly awarded when doing a manual descent
    public const float k_ManualAdvanceBonusFactor = 0.66f;

    // Time in seconds between corruption spread events
    public const float k_CorruptionSpreadInterval = 1.0f;

    // If the difference in corruption values between two tiles is smaller than this value, corruption cannot spread between them
    public const float k_CorruptionEquilibriumThreshold = 1.0f;

    // Corruption value at which a tile can spread to other tiles
    public const float k_CorruptionSpreadThreshold = 70.0f;

    // Percentage (0-1) of corruption that is removed from the source, and added to neighbor tiles
    public const float k_CorruptionSpreadFactor = 0.15f;

    // If corruption value is lower than this, all visual logic on a tile is disabled (for performance)
    public const float k_CorruptionVisualThreshold = 5.0f;

    // Corruption value at which the tile is destroyed
    public const float k_CorruptionMax = 100.0f;

    // Time in seconds between all automatic Energy events (income & upkeep)
    public const float k_UpkeepInterval = 0.25f;

}
