using Rocket.API;
using Rocket.Core.Logging;
using Rocket.Unturned.Chat;
using Rocket.Unturned.Player;

namespace AntiAdminAbuse
{
    public class Utils
    {
        // Annoyances
        public static void Announce(string message)
        {
            Announce(message, null);
        }

        // All in one announcer
        public static void Announce(string message, UnturnedPlayer recipient)
        {
            // If private message
            if (recipient != null)
            {
                UnturnedChat.Say(recipient, message);
                return;
            }

            // If public message
            else
            {
                UnturnedChat.Say(message);
            }

            // Log it
            if (Main.Config.LogToServerLog)
            {
                Logger.Log(message);
            }
        }

        /*
            Check to see if a player is a staff member
            Using the node "antiadminabuse.bypass" will bypass this
            or by changing "IgnoreAdmins" in the config to true
        */
        public static bool IsActive(UnturnedPlayer untPlayer)
        {
            if (untPlayer?.IsAdmin ?? false)
            {
                if (Main.Config.IgnoreAdmins)
                {
                    return false;
                }
                return true;
            }
            if (untPlayer?.HasPermission("antiadminabuse.active") ?? false)
            {
                if (untPlayer.HasPermission("antiadminabuse.bypass"))
                {
                    return false;
                }
                return true;
            }
            return false;
        }
    }
}
