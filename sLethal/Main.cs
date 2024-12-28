using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using LethalAPI.LibTerminal.Models;
using sLethal.commands;

namespace sLethal
{
    [BepInPlugin(modID, modName, modVersion)]
    public class Main : BaseUnityPlugin
    {
        private const string modID = "me.splitque.lethal";
        private const string modName = "sLethal";
        private const string modVersion = "0.0.1";

        private Harmony harmony;
        private TerminalModRegistry commands;
        public static ManualLogSource logger;

        public void Awake()
        {
            harmony = new Harmony(modID);
            commands = new TerminalModRegistry();
            logger = BepInEx.Logging.Logger.CreateLogSource(modName);

            harmony.PatchAll(typeof(Main));
            commands.RegisterFrom(new KillCommand());

            logger.LogInfo(modName + " loaded!"); 
        }

    }
}