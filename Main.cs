using System.Linq;
using Rocket.API;
using Rocket.API.Collections;
using Rocket.Core.Plugins;
using Rocket.Unturned.Events;
using Rocket.Unturned.Player;
using SDG.Unturned;
using Steamworks;
using UnityEngine;

namespace AntiAdminAbuse
{
    public class Main : RocketPlugin<Configuration>
    {
        public static Main Instance;
        public static Configuration Config;
        public static Utils Utils;

        protected override void Load()
        {
            Config = Configuration.Instance;
            Instance = this;

            UnturnedPlayerEvents.OnPlayerChatted += OnPlayerChatted;
            UnturnedPlayerEvents.OnPlayerDeath += OnPlayerDeath;
            DamageTool.playerDamaged += OnPlayerDamaged;

        }

        protected override void Unload()
        {
            UnturnedPlayerEvents.OnPlayerChatted -= OnPlayerChatted;
            DamageTool.playerDamaged -= OnPlayerDamaged;
        }

        void OnPlayerChatted(UnturnedPlayer player, ref Color color, string message, EChatMode chatMode, ref bool cancel)
        {
            if (Utils.IsActive(player))
            {
                if (message == null) return;

                string[] messageSplit = message.Split(' ');
                string command = messageSplit[0];

                if (Config.DontAnnounceCommandsInVanish && player.VanishMode)
                {
                    return;
                }

                if (command.Equals("/god") && player.HasPermission("god") && Config.AnnounceGod)
                {   
                    if (player.GodMode)
                    {
                        Utils.Announce(Translate("announce_disable_god", player.DisplayName));
                    }
                    else
                    {
                        Utils.Announce(Translate("announce_enable_god", player.DisplayName));
                    }
                }

                else if (command.Equals("/vanish") && player.HasPermission("vanish") && Config.AnnounceVanish)
                {
                    if (player.VanishMode)
                    {
                        Utils.Announce(Translate("announce_disable_vanish", player.DisplayName));
                    }
                    else
                    {
                        Utils.Announce(Translate("announce_enable_vanish", player.DisplayName));
                    }
                }

                else if (command.Equals("/heal") && player.HasPermission("heal") && Config.AnnounceHeal)
                {
                    if (messageSplit.Length > 1)
                    {
                        UnturnedPlayer target = UnturnedPlayer.FromName(messageSplit[1]);

                        if (target != null)
                        {
                            Utils.Announce(Translate("announce_heal_other", player.DisplayName, target.DisplayName));
                        }
                    }
                    else
                    {
                        Utils.Announce(Translate("announce_heal_self", player.DisplayName));
                    }

                }

                else if (command.Equals("/tp") && player.HasPermission("tp") && Config.AnnounceTp)
                {
                    if (messageSplit.Length < 1) return;

                    UnturnedPlayer target = UnturnedPlayer.FromName(messageSplit[1]);

                    if (target != null)
                    {
                        Utils.Announce(Translate("announce_tp", player.DisplayName, target.DisplayName));
                    }

                    else
                    {
                        foreach (Node node in LevelNodes.nodes)
                        {
                            if (node.type == ENodeType.LOCATION)
                            {
                                if (((LocationNode)node).name.ToLower().Contains(messageSplit[1].Replace(" ", "").Replace("\"", "")))
                                {
                                    Utils.Announce(Translate("announce_tp", player.DisplayName, ((LocationNode)node).name));
                                }
                            }
                        }
                    }
                }

                else if (command.Equals("/tphere") && player.HasPermission("tphere") && Config.AnnounceTphere)
                {
                    if (messageSplit.Length < 1) return;

                    UnturnedPlayer target = UnturnedPlayer.FromName(messageSplit[1]);

                    if (target != null)
                    {
                        Utils.Announce(Translate("announce_tphere", player.DisplayName, target.DisplayName));
                    }
                }

                else if (command.Equals("/kick") && player.HasPermission("kick") && Config.AnnounceKick)
                {
                    if (messageSplit.Length < 1) return;

                    UnturnedPlayer target = UnturnedPlayer.FromName(messageSplit[1]);

                    if (target != null)
                    {
                        Utils.Announce(Translate("announce_kick", player.DisplayName, target.DisplayName));
                    }
                }

                else if (command.Equals("/ban") && player.HasPermission("ban") && Config.AnnounceBan)
                {
                    if (messageSplit.Length < 1) return;

                    UnturnedPlayer target = UnturnedPlayer.FromName(messageSplit[1]);

                    if (target != null)
                    {
                        Utils.Announce(Translate("announce_ban", player.DisplayName, target.DisplayName));
                    }
                }

                else if (command.Equals("/slay") && player.HasPermission("slay") && Config.AnnounceSlay)
                {
                    if (messageSplit.Length < 1) return;

                    UnturnedPlayer target = UnturnedPlayer.FromName(messageSplit[1]);

                    if (target != null)
                    {
                        Utils.Announce(Translate("announce_slay", player.DisplayName, target.DisplayName));
                    }
                }

                else if (command.Equals("/spy") && player.HasPermission("spy") && Config.AnnounceSpy)
                {
                    if (messageSplit.Length < 1) return;

                    UnturnedPlayer target = UnturnedPlayer.FromName(messageSplit[1]);

                    if (target != null)
                    {
                        Utils.Announce(Translate("announce_spy", player.DisplayName, target.DisplayName));
                    }
                }

                else if (command.Equals("/admin") && player.HasPermission("admin") && Config.AnnounceAdmin)
                {
                    if (messageSplit.Length < 1) return;

                    UnturnedPlayer target = UnturnedPlayer.FromName(messageSplit[1]);

                    if (target != null)
                    {
                        Utils.Announce(Translate("announce_admin", player.DisplayName, target.DisplayName));
                    }
                }

                else if (command.Equals("/airdrop") && player.HasPermission("airdrop") && Config.AnnounceAirdrop)
                {
                    if (messageSplit.Length < 1) return;
                    Utils.Announce(Translate("announce_airdrop", player.DisplayName));
                }

                else if (command.Equals("/i") && player.HasPermission("i") && Config.AnnounceItem)
                {
                    if (messageSplit.Length < 2) return;

                    int AssetCount = 1;

                    if (messageSplit.Length > 2)
                    {
                        int.TryParse(messageSplit[2].Replace("\"", ""), out AssetCount);
                    }

                    try
                    {
                        ushort.TryParse(messageSplit[1], out ushort AssetID);
                        Asset iAssetID = Assets.find(EAssetType.ITEM, AssetID);

                        Utils.Announce(Translate("announce_item", player.DisplayName, AssetCount, ((ItemAsset)iAssetID).itemName, iAssetID.id));
                    }
                    catch
                    {
                        ItemAsset AssetName = (Assets.find(EAssetType.ITEM).Cast<ItemAsset>().Where(i => i.itemName != null).OrderBy(i => i.itemName.Length).FirstOrDefault(i => i.itemName.ToLower().Contains(messageSplit[1].Replace("\"", "").ToLower())));

                        if (AssetName == null) return;

                        Utils.Announce(Translate("announce_item", player.DisplayName, AssetCount, AssetName.itemName, AssetName.id));
                    }
                }

                else if (command.Equals("/v") && player.HasPermission("v") && Config.AnnounceVehicle)
                {
                    if (messageSplit.Length < 2) return;

                    try
                    {
                        ushort.TryParse(messageSplit[1], out ushort AssetID);
                        Asset vAssetID = Assets.find(EAssetType.VEHICLE, AssetID);

                        Utils.Announce(Translate("announce_vehicle", player.DisplayName, ((VehicleAsset)vAssetID).vehicleName, vAssetID.id.ToString()));
                    }
                    catch
                    {
                        VehicleAsset AssetName = (Assets.find(EAssetType.VEHICLE).Cast<VehicleAsset>().Where(v => v.vehicleName != null).OrderBy(v => v.vehicleName.Length).FirstOrDefault(v => v.vehicleName.ToLower().Contains(messageSplit[1].Replace("\"", "").ToLower())));

                        if (AssetName == null) return;

                        Utils.Announce(Translate("announce_vehicle", player.DisplayName, AssetName.vehicleName, AssetName.id.ToString()));
                    }
                }
            }
        }

        void OnPlayerDeath(UnturnedPlayer player, EDeathCause cause, ELimb limb, CSteamID murderer)
        {
            // Remove any of those pesky errors
            if (player?.Player != null)
            {
                UnturnedPlayer untKiller = UnturnedPlayer.FromCSteamID(murderer);

                if (Utils.IsActive(untKiller))
                {
                    if (untKiller.GodMode)
                    {
                        if (Config.PlayerDamagePublicWarn)
                        {
                            Utils.Announce(Translate("public_killed_in_god", untKiller.DisplayName));
                        }
                    }
                    if (untKiller.VanishMode)
                    {
                        if (Config.PlayerDamagePublicWarn)
                        {
                            Utils.Announce(Translate("public_killed_in_vanish", untKiller.DisplayName, player.DisplayName));
                        }
                    }
                }
            }


        }

        void OnPlayerDamaged(Player player, ref EDeathCause cause, ref ELimb limb, ref CSteamID killer, ref Vector3 direction, ref float damage, ref float times, ref bool canDamage)
        {
            UnturnedPlayer untKiller = UnturnedPlayer.FromCSteamID(killer);
            UnturnedPlayer untVictim = UnturnedPlayer.FromPlayer(player);

            // Remove any of those pesky errors
            if (untKiller?.Player != null)
            {
                if (Utils.IsActive(untKiller))
                {
                    if (untKiller.GodMode)
                    {
                        if (Config.PlayerDamagePublicWarn)
                        {
                            Utils.Announce(Translate("public_damaged_in_god", untKiller.DisplayName));
                        }
                        if (Config.PlayerDamagePrivateWarn)
                        {
                            Utils.Announce(Translate("private_damaged_in_god", untVictim.DisplayName, untKiller), untKiller);
                        }
                        if (Config.BlockPlayerDamage)
                        {
                            canDamage = false;
                        }
                    }
                    if (untKiller.VanishMode)
                    {
                        if (Config.PlayerDamagePublicWarn)
                        {
                            Utils.Announce(Translate("public_damaged_in_vanish", untKiller.DisplayName, untVictim.DisplayName));
                        }
                        if (Config.PlayerDamagePrivateWarn)
                        {
                            Utils.Announce(Translate("private_damaged_in_vanish", untVictim.DisplayName), untKiller);
                        }
                        if (Config.BlockPlayerDamage)
                        {
                            canDamage = false;
                        }
                    }
                }
            }
        }

        public override TranslationList DefaultTranslations => new TranslationList()
        {
            {"announce_enable_god", "{0} has enabled god mode!"},
            {"announce_disable_god", "{0} has disabled god mode!"},
            {"announce_enable_vanish", "{0} has enabled vanish!"},
            {"announce_enable_vanish", "{0} has disabled vanish!"},
            {"announce_heal_other", "{0} has healed {1}!"},
            {"announce_heal_self", "{0} has healed themselves!"},
            {"announce_tp", "{0} has teleported to {1}"},
            {"announce_tphere", "{0} has teleported {1} to themselves!"},
            {"announce_kick", "{0} has kicked {1}!"},
            {"announce_ban", "{0} has banned {1}!"},
            {"annoucne_slay", "{0} has slayed {1}!"},
            {"announce_spy", "{0} has spied on {1}!"},
            {"announce_admin", "{0} has admined {1}!"},
            {"announce_airdrop", "{0} has called in an airdrop!"},
            {"announce_item", "{0} has spawned in {1} {2} ({3})!"},
            {"announce_vehicle", "{0} has spawned in a {1} ({2})!"},

            {"public_killed_in_god", "{0} has killed {1} while in god mode!"},
            {"public_killed_in_vanish", "{0} has killed {1} while in vanish!"},

            {"public_damaged_in_god", "{0} has damaged {1} while in god mode!"},
            {"private_damaged_in_god", "You damaged {0} in god mode!"},
            {"public_damaged_in_vanish", "{0} has damaged {1} while in vanish!"},
            {"private_damaged_in_vanish", "You damaged {0} in vanish!"}
        };
    }
}