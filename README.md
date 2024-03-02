# What is this?
A Windows program for Wahoo Kickr Bike Shift and Wahoor Kickr Bike gears display and button handling
Written in C# to show on top of Rouvy (Or other such software) to show your current gears, and also be able to use the basic buttons.

First connection can take a good while, but after that the ID is remembered which should speed up things a lot.

[![Youtube vidoe link](https://img.youtube.com/vi/34VhzJVuYRo/0.jpg)](https://youtu.be/34VhzJVuYRo)

# Usage
Settings can be adjusted at `%APPDATA%\WahooShift\appsettings.json`, but only after first startup.


## Bike name
By default it only scans for a Wahoo trainer with the `KICKR BIKE` name. If you have renamed it through Wahoo app you'll need to change this in the .json for it to be found properly. 

## Keybinds
Keybinds can be disabled by setting Enabled to "false" in the .json file

You can configure keybinds in the json. [SendKeys](https://learn.microsoft.com/en-us/dotnet/api/system.windows.forms.sendkeys?view=windowsdesktop-8.0) is used so only those keybinds are available right now.

# Limitiations
* I don't do any handling for lost connections right now. 
* Sometimes Rouvy opens up above the application.
* Handling of higher resolutions / other DPI might be very poor (Just threw it together for my 1080p laptop).

