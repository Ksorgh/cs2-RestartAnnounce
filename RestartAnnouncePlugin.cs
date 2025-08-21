using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Core.Attributes;
using CounterStrikeSharp.API.Modules.Admin;
using CounterStrikeSharp.API.Modules.Commands;
using CounterStrikeSharp.API.Modules.Utils;

namespace RestartAnnouncePlugin;

[MinimumApiVersion(80)]
public class RestartAnnouncePlugin : BasePlugin
{
    public override string ModuleName => "Restart Announce Plugin";
    public override string ModuleVersion => "1.0.0";
    public override string ModuleAuthor => "ksor";

    public override void Load(bool hotReload)
    {
        AddCommand("css_restartannounce", "Announce server restart in chat", OnRestartCommand);
        Console.WriteLine("[RestartAnnouncePlugin] Plugin loaded successfully!");
    }

    private void OnRestartCommand(CCSPlayerController? player, CommandInfo command)
    {
        if (player != null && !AdminManager.PlayerHasPermissions(player, "@css/rcon"))
        {
            command.ReplyToCommand("You don't have permission to use this command!");
            return;
        }

        Console.WriteLine("[RestartAnnouncePlugin] Starting restart announcement...");
        _ = SendAnnouncementMessagesAsync();
        
        command.ReplyToCommand("Restart announcement started!");
    }

    private async Task SendAnnouncementMessagesAsync()
    {
        for (int i = 0; i < 5; i++)
        {
            await Task.Run(() =>
            {
                Server.NextFrame(() =>
                {
                    string message = "В СКОРОМ ВРЕМЕНИ БУДЕТ РЕСТАРТ";
                    string coloredMessage = $" {ChatColors.Red}{message}";
                    
                    Server.PrintToChatAll(coloredMessage);
                    Console.WriteLine($"[RestartAnnouncePlugin] Sending message {i + 1}/5");
                });
            });

            if (i < 4)
            {
                await Task.Delay(1000);
            }
        }
        
        Console.WriteLine("[RestartAnnouncePlugin] Announcement completed!");
    }
}