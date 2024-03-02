
using InTheHand.Bluetooth;
using System.Diagnostics;
using WahooShift.Configuration;

namespace WahooShift;

/// <summary>
/// Some lazy representation of one of the buttons on the handlebars.
/// </summary>
class KickrButton
{
    private byte Val1 { get; init; }
    private byte Val2 { get; init; }
    //! Slug for this key
    private string Label { get; init; }
    // Text representation of key
    public string Key { get; init; }
    //! Value above this should be considered keypress, value below is release
    public static byte PRESS = 0x50;

    public KickrButton(byte val1, byte val2, string label, string key)
    {
        Val1 = val1;
        Val2 = val2;
        Label = label;
        Key = key;
    }

    /// <summary>
    /// Lazy meh code to check if this button is pressed, if so fetch which keybind it is and execute it
    /// </summary>
    /// <param name="bytes"></param>
    /// <returns></returns>
    public bool ButtonCheck(byte[] bytes)
    {
        if (bytes.Length < 2)
            return false;
        return bytes[0] == Val1 && bytes[1] == Val2;
    }

    /// <summary>
    /// Returns all known buttons on Wahoo Kickr Bike shift, and inits their keybinds.
    /// Keybinds can be configured in the appsettings.json file.
    /// </summary>
    /// <returns></returns>
    public static List<KickrButton> PrepareButtons(IKeybinds keybinds)
    {
        return new()
        {
            new KickrButton(0x00, 0x01, "right front", keybinds.RightFront),
            new KickrButton(0x80, 0x00, "right back", keybinds.RightBack),
            new KickrButton(0x00, 0x08, "right inside", keybinds.RightInside),
            new KickrButton(0x00, 0x04, "right large", keybinds.RightLarge),
            new KickrButton(0x00, 0x02, "right small", keybinds.RightSmall),
            new KickrButton(0x40, 0x00, "right brake", keybinds.RightBrake),
            new KickrButton(0x02, 0x00, "left front", keybinds.LeftFront),
            new KickrButton(0x04, 0x00, "left back", keybinds.LeftBack),
            new KickrButton(0x20, 0x00, "left inside", keybinds.LeftInside),
            new KickrButton(0x10, 0x04, "left large", keybinds.LeftLarge),
            new KickrButton(0x08, 0x02, "left small", keybinds.LeftSmall),
            new KickrButton(0x01, 0x00, "left brake", keybinds.LeftBrake),
        };
    }

}

/// <summary>
/// Handles button presses by wahoo bike received in bluetooth
/// </summary>
class WahooButtons
{
    /// <summary>
    /// Event whenever actual key is forwarded to computer
    /// </summary>
    public event EventHandler<string>? KeyPress;
    //! Characteristic ID for button system
    private static readonly Guid characteristicId = new("a026e03c-0a7d-4ab3-97fa-f1500f9feb8b");
    //! Connected characteristic
    private GattCharacteristic? characteristic;
    /// <summary>
    /// True if buttons should actually be handled.
    /// Bluetooth characteristic will still be subscribed too for easy on-the-fly enabling/diabling
    /// </summary>
    public bool Enabled { get; set; } = true;

    private List<KickrButton> Buttons { get; init; }


    public WahooButtons(IKeybinds keybinds, bool buttonsEnabled)
    {
        Buttons = KickrButton.PrepareButtons(keybinds);
        this.Enabled = buttonsEnabled;
    }

    /// <summary>
    /// Sets up notification handler for the characteristic
    /// </summary>
    /// <param name="service"></param>
    /// <returns>True if connected</returns>
    public async Task<bool> Connect(GattService service)
    {
        characteristic = await service.GetCharacteristicAsync(characteristicId);
        if (characteristic == null)
            return false;

        characteristic.CharacteristicValueChanged += Callback;
        await characteristic.StartNotificationsAsync();
        return true;
    }

    /// <summary>
    /// Callback whenever we receive a NOTIFY event from bluetooth on the button characteristic
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void Callback(object? sender, GattCharacteristicValueChangedEventArgs e)
    {
        // Don't handle if buttons are disabled
        if (!Enabled) return;

        if (e.Value.Length < 3)
            return;

        bool pressed = e.Value[2] > KickrButton.PRESS;
        if (pressed)
            UseButton(e.Value);
    }

    /// <summary>
    /// Handle wahoo bike button
    /// </summary>
    /// <param name="button"></param>
    private void UseButton(byte[] button)
    {
        foreach (var kickrButton in Buttons)
        {
            if (kickrButton.ButtonCheck(button))
            {
                var key = kickrButton.Key;
                if (key == "")
                    return;

                Debug.WriteLine($"Pressing: {key}");
                SendKeys.SendWait("{" + key + "}");
                KeyPress?.Invoke(this, key);
                return;
            }
        }

    }
}