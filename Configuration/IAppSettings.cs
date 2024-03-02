using Config.Net;

namespace WahooShift.Configuration;

/// <summary>
/// Container for application settings. Get serialized to appsettings.json in appdata.
/// </summary>
public interface IAppSettings
{
    IBluetooth Bluetooth { get; set; }

    IKeybinds Keybinds { get; set; }

    IUserInterface UserInterface { get; set; }

    [Option(DefaultValue = true)]
    bool ButtonsEnabled { get; set; }
}
