using Config.Net;

namespace WahooShift.Configuration;

/// <summary>
/// Container to be able to remap buttons.
/// All keybinds are set in WahooButtons.PrepareButtons
/// </summary>
public interface IKeybinds
{
    string? RightFront { get; set;}

    string? RightBack { get; set;}

    string? RightInside { get; set;}

    string? RightLarge { get; set;}

    string? RightSmall { get; set;}

    string? RightBrake { get; set;}

    string? LeftFront { get; set;}

    string? LeftBack { get; set;}

    string? LeftInside { get; set;}

    string? LeftLarge { get; set;}

    string? LeftSmall { get; set;}

    string? LeftBrake { get; set;  }
}
