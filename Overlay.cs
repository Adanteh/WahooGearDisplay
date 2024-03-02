using WahooShift.Utils;
namespace WahooShift;

/// <summary>
/// UI class. Will need to handle all the events from our bluetooth class in here for proper threaded handling.
/// </summary>
public partial class Overlay : Form
{
    private readonly Bluetooth bluetooth = new();

    /// <summary>
    /// Cleanup the key that was pressed after debounce time. This means that if you press any buttons, 2 seconds after the last press the label will hide again
    /// </summary>
    private readonly Debounce<string> debounceKeyInfo = new(TimeSpan.FromSeconds(2));

    /// <summary>
    /// Current gear data received over bluetooth
    /// </summary>
    private WahooGearEventArgs gears;

    /// <summary>
    /// Init, setup our bluetooth connection and UI signals.
    /// </summary>
    public Overlay()
    {
        InitializeComponent();

        // Run our bluetooth stuff
        RegisterBluetooth();

        // Register bluetooth events to be handled by this UI.
        bluetooth.Gears.GearChange += Gears_GearChange;
        bluetooth.Buttons.KeyPress += ShowKeybindLabel;
        bluetooth.ConnectionEvent += Wahoo_ConnectionEvent;

        this.gears = new WahooGearEventArgs { FrontCurrent = 1, FrontTotal = 2, RearCurrent = 5, RearTotal = 11 };
        this.MouseDown += Form1_MouseDown;
        this.Paint += Repaint;
    }


    // Settings to handle dragging borderless window
    public const int WM_NCLBUTTONDOWN = 0xA1;
    public const int HT_CAPTION = 0x2;

    [System.Runtime.InteropServices.DllImport("user32.dll")]
    public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
    [System.Runtime.InteropServices.DllImport("user32.dll")]
    public static extern bool ReleaseCapture();

    /// <summary>
    /// Trigger the mousedown EH like if it were on the title bar, allows dragging the window
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void Form1_MouseDown(object? sender, System.Windows.Forms.MouseEventArgs e)
    {
        if (e.Button == MouseButtons.Left)
        {
            ReleaseCapture();
            SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
        }
    }

    /// <summary>
    /// Load event for the form
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void Form1_Load(object sender, EventArgs e)
    {
        BottomField.Visible = false;
        debounceKeyInfo.Elapsed += HideKeybindLabel;

        SetAsOverlay();
        
    }

    /// <summary>
    /// Sets best settings I could find as 'overlay' like app in forms
    /// </summary>
    private void SetAsOverlay()
    {
        TopMost = true;
        TopLevel = true;
        Text = "";
        FormBorderStyle = FormBorderStyle.SizableToolWindow;
        Focus();
    }

    /// <summary>
    /// Start our bluetooth functionality
    /// </summary>
    private void RegisterBluetooth()
    {
        var blueThread = new Thread(new ThreadStart(bluetooth.ScanAndConnect));
        blueThread.Start();
    }

    /// <summary>
    /// Connected to bluetooth fully (Inc services, characteristics etc)
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="connected"></param>
    private void Wahoo_ConnectionEvent(object? sender, bool connected)
    {
        this.BeginInvoke((Action)delegate
        {
            ConnectionLabel.Visible = false;
        });
    }

    /// <summary>
    /// Handler for gear change event.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="newGears"></param>
    private void Gears_GearChange(object? sender, WahooGearEventArgs newGears)
    {
        gears = newGears;
        this.BeginInvoke((Action)delegate
        {
            RenderGears();
        });
    }

    /// <summary>
    /// UI drawing event, need to redraw gear display on each frame to correctly show
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void Repaint(object? sender, PaintEventArgs e)
    {
        RenderGears();
    }

    /// <summary>
    /// Main logic to render the gear display
    /// </summary>
    private void RenderGears()
    {
        var brushFill = new SolidBrush(Color.Orange);
        var brushStroke = new Pen(Color.White);

        Graphics formGraphics = this.CreateGraphics();
        formGraphics.Clear(BackColor);

        const int MARGIN = 10;
        var availableWidth = this.Size.Width - (5 * MARGIN);  // 10PX margin on each side, plus 20PX margin between front/rear
        var gearHeight = this.Size.Height - (2 * MARGIN);
        var yCenter = MARGIN + (gearHeight / 2);

        // TODO: Maybe allow some config, just for the sake of it?
        var showFrontRings = gears.FrontTotal > 1;
        var gearWidth = Convert.ToInt32(availableWidth / (showFrontRings ? gears.FrontTotal + gears.RearTotal : gears.RearTotal));

        var x = MARGIN;

        // Only draw front rings if there's more than 1. Apparently some people really hate front mechs.
        if (showFrontRings)
        {
            for (int i = 0; i < gears.FrontTotal; i++)
            {
                var heightOffset = (gears.FrontTotal - 1 - i) * 20;
                var rect = new Rectangle(x, MARGIN + heightOffset, gearWidth, gearHeight - (heightOffset * 2));
                if ((i + 1) == gears.FrontCurrent)
                    formGraphics.FillRectangle(brushFill, rect);

                formGraphics.DrawRectangle(brushStroke, rect);
                x += gearWidth;
            }
        }

        // Draw rear rings
        // TODO: Overall better resizing would be cool for fancier drawing, but this works for now
        if (showFrontRings)
            x += (2 * MARGIN);

        for (int i = 0; i < gears.RearTotal; i++)
        {
            var height = Math.Max(gearHeight - (i * 10), 10);
            var rect = new Rectangle(x, yCenter - height / 2, gearWidth, height);
            if ((i + 1) == gears.RearCurrent)
                formGraphics.FillRectangle(brushFill, rect);

            formGraphics.DrawRectangle(brushStroke, rect);
            x += gearWidth;
        }

        brushStroke.Dispose();
        brushFill.Dispose();
        formGraphics.Dispose();
    }

    /// <summary>
    /// Shows given text within the bottom text field. Will debounce to automatically hide it.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void ShowKeybindLabel(object? sender, string? e)
    {
        this.BeginInvoke((Action)delegate
        {
            BottomField.Text = e;
            BottomField.Visible = true;
            debounceKeyInfo.TriggerEvent(e);
        });
    }

    /// <summary>
    /// Hides the info label at the bottom
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void HideKeybindLabel(object? sender, string? e)
    {
        this.BeginInvoke((Action)delegate
        {
            BottomField.Visible = false;
        });
    }

    /// <summary>
    /// Small close button in top right
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void button1_Click(object sender, EventArgs e)
    {
        Close();
    }
}