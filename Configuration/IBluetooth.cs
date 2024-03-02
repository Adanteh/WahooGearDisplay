using Config.Net;
namespace WahooShift.Configuration;

public interface IBluetooth
{
    /// <summary>
    /// Device ID of last successful connection. Helps to speed up repeated connections.
    /// </summary>
    string? LastDeviceId { get; set; }

    /// <summary>
    /// Name of kickr bike. Does partial match, needs to be updated if someone were to change the name
    /// </summary>
    [Option(DefaultValue = "KICKR BIKE")]
    string DeviceName { get; set; }
}
