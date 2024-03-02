using Config.Net;
using InTheHand.Bluetooth;
using System.Diagnostics;
using WahooShift.Configuration;

namespace WahooShift;

/// <summary>
/// Container for all Bluetooth functionality with Kickr bike (Classic and Shift)
/// </summary>
internal class Bluetooth
{
    //! Service ID for both gear and button service used by Kickr bikes
    static readonly Guid serviceUid = new("a026ee0d-0a7d-4ab3-97fa-f1500f9feb8b");
    //! EH that emit connection status changes
    public event EventHandler<bool>? ConnectionEvent;

    /// <summary>
    /// Access to the virtual gears system
    /// </summary>
    public WahooGears Gears { get; init; }

    /// <summary>
    /// Access to the button system
    /// </summary>
    public WahooButtons Buttons { get; init; }

    /// <summary>
    /// Returns true if bluetooth fully connected (Service found, characteristics registered to, etc)
    /// </summary>
    public bool Connected { get; set; } = false;

    private readonly IAppSettings settings;

    public Bluetooth(IAppSettings settings)
    {

        Gears = new WahooGears();
        Buttons = new WahooButtons(settings.Keybinds);
        this.settings = settings;
    }

    /// <summary>
    /// Looks for bluetooth devices and automatically connects to it
    /// </summary>
    public async void ScanAndConnect()
    {
        // Check if we could autconnect to the last item, if we could we're all good here
        if (await AttemptLastConnection())
            return;

        // Prepare default device name. Done this way instead of default value in the settings, to write it to the json.
        settings.Bluetooth.DeviceName ??= "KICKR BIKE";

        Debug.WriteLine("Waiting for bluetooth connection to Wahoo");
        var filter = new BluetoothLEScanFilter();
        filter.NamePrefix = settings.Bluetooth.DeviceName;
        var options = new RequestDeviceOptions();
        options.Filters.Add(filter);

        while (!Connected)
        {
            Debug.WriteLine("Scanning again...");
            var tryForSeconds = new CancellationTokenSource(TimeSpan.FromSeconds(2));
            var devices = await InTheHand.Bluetooth.Bluetooth.ScanForDevicesAsync(options, tryForSeconds.Token);
            if (devices.Count > 0)
            {
                var device = devices.First();
                await ConnectToDevice(device);
            } else
            {
                Debug.WriteLine("Couldn't find suitable device");
            }
        };
    }

    /// <summary>
    /// Because Bluetooth.ScanForDevicesAsync is quite slow, we'll attempt to connect to
    /// the last used ID once first, which is a lot faster.
    /// </summary>
    /// <returns></returns>
    public async Task<bool> AttemptLastConnection()
    {
        var lastDeviceId = settings.Bluetooth.LastDeviceId;
        if (lastDeviceId != null)
        {
            Debug.WriteLine("Attemping connnection to last used id: " + lastDeviceId);
            var device = await BluetoothDevice.FromIdAsync(lastDeviceId);
            if (device != null)
            {
                return await ConnectToDevice(device);
            }
        }
        return false;
    }

    /// <summary>
    /// Connects to the found device
    /// </summary>
    /// <param name="device"></param>
    /// <returns></returns>
    private async Task<bool> ConnectToDevice(BluetoothDevice device)
    {
        // Connect to gatt server
        var gatt = device.Gatt;
        if (gatt == null)
        {
            Debug.WriteLine("Failed to setup connection with bluetooth sensor");
            return false;
        }

        // Look for the wahoo service that both gears and buttons use
        await gatt.ConnectAsync();
        var service = await gatt.GetPrimaryServiceAsync(serviceUid);
        if (service == null)
        {
            Debug.WriteLine("Failed to access correct service on bluetooth device");
            return false;
        }

        if (!await Gears.Connect(service))
            return false;
        if (!await Buttons.Connect(service))
            return false;

        Connected = true;
        ConnectionEvent?.Invoke(this, Connected);
        settings.Bluetooth.LastDeviceId = device.Id;
        return true;
    }

}
