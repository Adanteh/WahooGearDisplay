namespace WahooShift.Configuration;

/// <summary>
/// Container for application settings. Get serialized to appsettings.json in appdata.
/// </summary>
public interface IAppSettings
{
    IBluetooth Bluetooth { get; set; }

    IKeybinds Keybinds { get; set; }
}
