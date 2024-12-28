using GameNetcodeStuff;
using LethalAPI.LibTerminal.Attributes;
using Unity.Netcode;
using UnityEngine;

namespace sLethal.commands
{
    public class TeleportCommand : NetworkBehaviour // don't working right now
    {
        [TerminalCommand("teleport"), CommandInfo("teleport player to coords", "[Player] [x] [y] [z]")]
        public string TeleportServer(string player, int x, int y, int z)
        {
            Vector3 vector = new Vector3(x, y, z);
            PlayerControllerB[] playerList = StartOfRound.Instance.allPlayerScripts;

            for (int i = 0; i < playerList.Length; i++)
            {
                if (playerList[i].playerUsername == player)
                {
                    NetworkManager networkManager = playerList[i].NetworkManager;
                    if (networkManager == null || !networkManager.IsListening) return "error";
                    if (this.__rpc_exec_stage != NetworkBehaviour.__RpcExecStage.Client && (networkManager.IsServer || networkManager.IsHost))
                    {
                        ServerRpcParams serverRpcParams = new ServerRpcParams();
                        FastBufferWriter bufferWriter = this.__beginSendServerRpc(2770415134U, serverRpcParams, RpcDelivery.Reliable);
                        BytePacker.WriteValueBitPacked(bufferWriter, x);
                        BytePacker.WriteValueBitPacked(bufferWriter, y);
                        BytePacker.WriteValueBitPacked(bufferWriter, z);
                        this.__endSendServerRpc(ref bufferWriter, 2770415134U, serverRpcParams, RpcDelivery.Reliable);
                    }
                    return TeleportClient(playerList[i], x, y, z);
                }
            }
            return "This player non-exist.";
        }
        
        [TerminalCommand("teleport"), CommandInfo("teleport player to coords", "[Player] [x] [y] [z]")]
        public string TeleportClient(PlayerControllerB player, int x, int y, int z)
        { 
            NetworkManager networkManager = player.NetworkManager;
            if (networkManager == null || !networkManager.IsListening) return "error";
            if (this.__rpc_exec_stage != NetworkBehaviour.__RpcExecStage.Client && (networkManager.IsServer || networkManager.IsHost))
            {
                ClientRpcParams clientRpcParams = new ClientRpcParams();
                FastBufferWriter bufferWriter = this.__beginSendClientRpc(2770415134U, clientRpcParams, RpcDelivery.Reliable);
                BytePacker.WriteValueBitPacked(bufferWriter, x);
                BytePacker.WriteValueBitPacked(bufferWriter, y);
                BytePacker.WriteValueBitPacked(bufferWriter, z);
                this.__endSendClientRpc(ref bufferWriter, 2770415134U, clientRpcParams, RpcDelivery.Reliable);
            }
            player.TeleportPlayer(new Vector3(x, y, z));
            return "Player teleported!";
        }
    }
}
