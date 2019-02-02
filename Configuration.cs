using Rocket.API;

namespace AntiAdminAbuse
{
    public class Configuration : IRocketPluginConfiguration
    {
        public bool AnnounceGod;
        public bool AnnounceVanish;
        public bool AnnounceHeal;
        public bool AnnounceTp;
        public bool AnnounceTphere;
        public bool AnnounceKick;
        public bool AnnounceBan;
        public bool AnnounceSlay;
        public bool AnnounceSpy;
        public bool AnnounceAdmin;
        public bool AnnounceAirdrop;
        public bool AnnounceItem;
        public bool AnnounceVehicle;

        public bool PlayerDamagePrivateWarn;
        public bool PlayerDamagePublicWarn;
        public bool BlockPlayerDamage;

        public bool DontAnnounceCommandsInVanish;

        public bool LogToServerLog;
        public bool IgnoreAdmins;

        public void LoadDefaults()
        {
            AnnounceGod = true;
            AnnounceVanish = true;
            AnnounceHeal = true;
            AnnounceTp = true;
            AnnounceTphere = true;
            AnnounceKick = true;
            AnnounceBan = true;
            AnnounceSlay = true;
            AnnounceSpy = false;
            AnnounceAdmin = true;
            AnnounceAirdrop = true;
            AnnounceItem = true;
            AnnounceVehicle = true;

            PlayerDamagePrivateWarn = true;
            PlayerDamagePublicWarn = false;
            BlockPlayerDamage = true;

            DontAnnounceCommandsInVanish = false;

            LogToServerLog = true;
            IgnoreAdmins = false;
        }
    }
}