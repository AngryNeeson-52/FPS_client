using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClientSend : MonoBehaviour
{
    private static void SendTCPData(Packet _packet)
    {
        _packet.WriteLength();
        Client.instance.tcp.SendData(_packet);
    }

    private static void SendUDPData(Packet _packet)
    {
        _packet.WriteLength();
        Client.instance.udp.SendData(_packet);
    }

    #region
    public static void WelcomeReceived()
    {
        using (Packet _packet = new Packet((int)ClientPackets.welcomeReceived))
        {
            _packet.Write(Client.instance.myId);
            _packet.Write(UIManager.instance.userNameField.text);

            SendTCPData(_packet);
        }
    }

    public static void PlayerPosition()
    {
        using (Packet _packet = new Packet((int)ClientPackets.playerPosition))
        {
            _packet.Write(GameManager.players[Client.instance.myId].transform.position);

            SendUDPData(_packet);
        }
    }

    public static void PlayerRotation()
    {
        using (Packet _packet = new Packet((int)ClientPackets.playerRotation))
        {
            _packet.Write(GameManager.players[Client.instance.myId].transform.rotation);

            SendUDPData(_packet);
        }
    }

    public static void PlayerHit(int _hitid)
    {
        using (Packet _packet = new Packet((int)ClientPackets.playerHit))
        {
            _packet.Write(_hitid);

            SendUDPData(_packet);
        }
    }

    public static void PlayerRespawn(int _spawnid)
    {
        using (Packet _packet = new Packet((int)ClientPackets.playerRespawn))
        {
            _packet.Write(_spawnid);

            SendUDPData(_packet);
        }
    }
    public static void PlayerFire()
    {
        using (Packet _packet = new Packet((int)ClientPackets.playerFire))
        {
            _packet.Write(Client.instance.myId);

            SendUDPData(_packet);
        }
    }
    #endregion
}
