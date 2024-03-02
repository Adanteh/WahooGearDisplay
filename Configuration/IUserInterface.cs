using Config.Net;
namespace WahooShift.Configuration;

public interface IUserInterface
{
    /// <summary>
    /// xPos of UI when opening / closing
    /// </summary>
    int? xPos { get; set; }

    /// <summary>
    /// yPos of UI when opening / closing
    /// </summary>
    int? yPos { get; set; }

    /// <summary>
    /// UI height
    /// </summary>
    int? width {  get; set; }

    /// <summary>
    /// UI width
    /// </summary>
    int? height { get; set; }
}
