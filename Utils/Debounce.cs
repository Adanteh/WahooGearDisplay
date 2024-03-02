using System.Timers;

namespace WahooShift.Utils;

/// <summary>
/// Debouncer like in javascript.
/// Will only use invoke after `delay` time has passed. Invoke is called with the last data this debouncer is triggered with.
/// </summary>
/// <typeparam name="T"></typeparam>
internal class Debounce<T>
{
    public event EventHandler<T?>? Elapsed;
    private readonly System.Timers.Timer _timer;
    private T? lastData;

    public Debounce(TimeSpan delay)
    {
        _timer = new()
        {
            Interval = delay.TotalMilliseconds,
            AutoReset = false
        };
        _timer.Elapsed += OnTimerElapsed;
    }

    public void TriggerEvent(T? data)
    {
        lastData = data;
        _timer.Stop();
        _timer.Start();
    }

    private void OnTimerElapsed(object? sender, EventArgs e)
    {
        Elapsed?.Invoke(this, lastData);
    }
}
