# What is this?
It's a small C# program to run on Windows for usage with Wahoo Kickr Bike.
It shows both gears and has ability to handle keypresses (Because Rouvy has no button support).

The bluetooth ID of the bike is remembered so hopefully speed up connections a little bit.
Keybinds are configurable.

# Changing settings
After first startup a .json file should be available at `%APPDATA%\WahooShift\appsettings.json` which lets you change keybinds. [SendKeys](https://learn.microsoft.com/en-us/dotnet/api/system.windows.forms.sendkeys?view=windowsdesktop-8.0) is used so only those keybinds are available right now.

# Limitiations
* I don't do any handling for lost connections right now. 
* Also I've noticed that at least for Rouvy you'd need to star tthe app after launching rouvy itself, I don't use Zwift so no idea if it would work there.
* Handling of higher resolutions / other DPI might be very poor (Just threw it together for my 1080p laptop).
* Screen position is not saved between restarts, would be very welcome MR.

