# CommandBeep (Alpha Stage)

Send messages on [Beeper](https://beeper.com) from [PowerToys' Command Palette](https://learn.microsoft.com/en-us/windows/powertoys/command-palette/overview) window directly.

![A poster of CommandBeep GitHub Header, with text "CommandBeep: Send Beeper Messages Faster, Powered by Command Palette" with an image of Command Palette with CommandBeep window open.](materials/CommandBeep.jpg)

> [!IMPORTANT]
> This is still in proof of concept stage. If you want to try it, there's a guide below.

# How It Works?

More information on diagram planning here: [FigJam Link](https://www.figma.com/board/q8LBTIiQcOWvAJ525bE2kS/CommandBeep?node-id=0-1&t=G6Jccg5lCIMLM6ME-1) or [FigJam file attached in this repo.](materials/CommandBeep.jam)

# Development

## Pre-requisites

1. [**Visual Studio** with **Windows App SDK** & **WinUI workload** installed](https://learn.microsoft.com/en-us/windows/apps/get-started/start-here?tabs=wingetconfig).[^1]
2. Windows 11 with **PowerToys** installed and **Command Palette** enabled.[^1]
3. [Enable Developer Mode on Windows](https://learn.microsoft.com/en-us/windows/advanced-settings/developer-mode).[^1]
4. **Beeper Desktop** installed with **Desktop API enabled**.[^2]

### Setting up

1. Clone the repository (`git clone https://github.com/BenjaminFosters/CommandBeep`).
2. Open the solution file `CommandBeep.sln` in Visual Studio.
3. In the Solution Explorer, go to `CommandBeep\Pages\CommandBeepPage.cs`<br>Replace `_beeperSrv = new BeeperSrv("http://localhost:23373/", "bdapi_6mxSA1itq8ntUXKE1FvhpVkNOAuOvK3fWR8lKkTo8_w");` with your own API key.[^3]
   - To get API Key, go to Beeper Desktop, Settings (`Ctrl+,`) > Integrations > Approved connections > Plus icon > Give a name and allow sensitive permissions > Copy the API key.

### Installing the extension

4. Build the Solution. Build > Build Solution (`F6`).
5. Then deploy the extension. Build > Deploy Solution.
6. Open PowerToys Command Palette, type `reload` and reload Command Palette, then it should be (after loading) on the very bottom of the list.<br>or find "CommandBeep", it should be on the list.

### Debugging

To debug, use (`F5`) instead and follow the 6th instruction above.

# Feature List (and To do List)

Do keep in mind, I will add necessary features, specifically to prevent [feature creep](https://en.wikipedia.org/wiki/Feature_creep). Also these features are designed for user experience (in terms of intuitiveness and overall performance) and general aesthetics.

- [x] Querying Chats
- [x] Sending Message
- [x] User Feedback
- [ ] Ability to change API key
- [ ] OAuth 2.0 authorization support
- [ ] Faster querying speed through caching
- [ ] Icons for Accessibility & Aesthetics
- [ ] Photo Profile with circle & 1:1 ratio.

# Footnotes

[^1]: Source: [Microsoft Learn](https://learn.microsoft.com/en-us/windows/powertoys/command-palette/creating-an-extension#overview)

[^2]: To enable Desktop API, open Beeper Desktop, go to Settings (`Ctrl+,`) > Integrations > Beeper Desktop API > Enable **Allow connections**. It should be available on `https://127.0.0.1:23373`. (**Not to be confused with `localhost` which can have some issues with OAuth 2.0 authorization.**)

[^3]: Yes, I know I hard coded the key (again, this is still in PoC), but it will change soon with implementation of fillable API key and OAuth 2.0 authorization. Since the key works on localhost connection, it should be fine for now. However, there's a risk of sharing the key, **if you tunneled the connection or enables remote access**.<br>That said, if you ever get your key through Git, please revoke it immediately. (If it leaks, then assume somebody already got the key.)
