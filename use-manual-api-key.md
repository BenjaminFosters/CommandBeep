# Add your API Key manually

In case you're unable to use OAuth 2.0, or you're (for some reason) uses remote endpoints, you can add your API key manually instead.

1. Grab your Beeper Desktop API key.

   Go to your Beeper Desktop > Settings (`Ctrl+,`) > Integrations > Beeper Desktop API > Approved Connections > Plus Icon

   | Options                 | Value             |
   | ----------------------- | ----------------- |
   | Name                    | Anything You Want |
   | Expires In              | Never             |
   | Allow sensitive actions | True              |

   Then copy your API key

   ![Get your API Key](assets/apikey/get.gif)

2. Open Command Palette, type `CommandBeep` and press `Ctrl + Enter`. This opens a settings page, in there, paste your API Key and click on **Save**.

   ![Add your API Key](assets/apikey/add.gif)

> [!TIP]
> Even though you can use `Ctrl + Enter` to open the settings page, the settings page is also accessible through the Command Palette settings page, or by clicking **Update in Settings** when you get invalid API key error.
