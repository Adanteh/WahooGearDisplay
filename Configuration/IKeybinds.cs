using Config.Net;

namespace WahooShift.Configuration;

/// <summary>
/// Container to be able to remap buttons
/// </summary>
public interface IKeybinds
{
    [Option(DefaultValue = "UP")]
    string RightFront { get; }

    [Option(DefaultValue = "DOWN")]
    string RightBack { get; }

    [Option(DefaultValue = "")]
    string RightInside { get; }

    [Option(DefaultValue = "")]
    string RightLarge { get; }

    [Option(DefaultValue = "")]
    string RightSmall { get; }

    [Option(DefaultValue = "ENTER")]
    string RightBrake { get; }

    [Option(DefaultValue = "RIGHT")]
    string LeftFront { get; }

    [Option(DefaultValue = "LEFT")]
    string LeftBack { get; }

    [Option(DefaultValue = "")]
    string LeftInside { get; }

    [Option(DefaultValue = "")]
    string LeftLarge { get; }

    [Option(DefaultValue = "")]
    string LeftSmall { get; }

    [Option(DefaultValue = "ESC")]
    string LeftBrake { get; }
}
