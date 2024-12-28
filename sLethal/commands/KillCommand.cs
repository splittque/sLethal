using GameNetcodeStuff;
using LethalAPI.LibTerminal.Attributes;
using UnityEngine;

namespace sLethal.commands
{
    public class KillCommand
    {
        [TerminalCommand("kill"), CommandInfo("Kill target player", "[Player]")]
        public string KillClient(string player)
        {
            PlayerControllerB[] playerList = StartOfRound.Instance.allPlayerScripts;
            
            for (int i = 0; i < playerList.Length; i++)
            {
                if (playerList[i].playerUsername == player)
                {
                    playerList[i].DamagePlayerFromOtherClientServerRpc(100, Vector3.up, -1);
                    Main.logger.LogInfo("Player" + playerList[i].playerUsername + " has been killed by terminal command");
                    return "Player killed!";
                }
            }
            return "This player non-exist.";
        }
    }
}