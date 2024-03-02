
using InTheHand.Bluetooth;
using System.Diagnostics;

namespace WahooShift;

/// <summary>
/// Event triggered whenenver gear event happens. 
/// </summary>
public class WahooGearEventArgs : EventArgs
{
    /// <summary>
    /// Index of which ring is used (0 for your smallest front ring)
    /// </summary>
    public int FrontCurrent { get; set; } = 0;
    /// <summary>
    /// Totaly amount of front rings (2 for a 2-by)
    /// </summary>
    public int FrontTotal { get; set; } = 0;
    /// <summary>
    /// Index of gear used in back (0 for the largest back cog)
    /// </summary>
    public int RearCurrent { get; set; } = 0;
    /// <summary>
    /// Total amount of gears (11 for 11-speed)
    /// </summary>
    public int RearTotal { get; set; } = 0;

    public override string ToString()
    {
        return $"{FrontCurrent}/{FrontTotal} : {RearCurrent}/{RearTotal}";
    }
}

/// <summary>
/// Virtual shifting service. Used to show current gears.
/// </summary>
class WahooGears
{
    /// <summary>
    /// Event that emits whenever gear changes
    /// </summary>
    public event EventHandler<WahooGearEventArgs>? GearChange;
    //! Charasteristic ID for gear system
    private static readonly Guid characteristicUid = new("a026e03a-0a7d-4ab3-97fa-f1500f9feb8b");
    //! BLE characteristic connection
    private GattCharacteristic? characteristic;
    //! Last gear event that is emitted
    private WahooGearEventArgs lastEvent = new();

    public WahooGears() { }

    public async Task<bool> Connect(GattService service)
    {
        characteristic = await service.GetCharacteristicAsync(characteristicUid);
        if (characteristic == null)
            return false;

        characteristic.CharacteristicValueChanged += Callback;
        await characteristic.StartNotificationsAsync();
        return true;
    }

    /// <summary>
    /// Callback whenever gear notification over bluetooth is received.
    /// Will emit the GearChange EH
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void Callback(object? sender, GattCharacteristicValueChangedEventArgs e)
    {
        // Received some data not containing everything?
        if (e.Value.Length < 5)
            return;

        // 2 ... # Selected Gear in Front
        // 3 ... # Selected Gear in Rear
        // 4 ... # Gears in Front
        // 5 ... # Gears in Rear
        var newFrontGear = e.Value[2] + 1;
        var newRearGear = e.Value[3] + 1;
        if (newFrontGear != lastEvent.FrontCurrent || newRearGear != lastEvent.RearCurrent)
        {
            lastEvent = new WahooGearEventArgs
            {
                FrontCurrent = newFrontGear,
                RearCurrent = newRearGear,
                FrontTotal = e.Value[4],
                RearTotal = e.Value[5],
            };

            Debug.WriteLine($"Gear: {lastEvent}");
            GearChange?.Invoke(this, lastEvent);
        }
    }


}
