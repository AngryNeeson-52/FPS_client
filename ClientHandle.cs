using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;

public class ClientHandle : MonoBehaviour
{
    public static void Welcome(Packet _packet)
    {
        string _msg = _packet.ReadString();
        int _myId = _packet.ReadInt();

        Debug.Log($"Message from server: {_msg}");
        Client.instance.myId = _myId;
        ClientSend.WelcomeReceived();

        Client.instance.udp.Connect(((IPEndPoint)Client.instance.tcp.socket.Client.LocalEndPoint).Port);
    }

    public static void SpawnPlayer(Packet _packet)
    {
        int _id = _packet.ReadInt();
        string _username = _packet.ReadString();
        Vector3 _position = _packet.ReadVector3();
        Quaternion _rotation = _packet.ReadQuaternion();
        int _hp = _packet.ReadInt();

        GameManager.instance.SpawnPlayer(_id, _username, _position, _rotation, _hp);
    }

    public static void PlayerPosition(Packet _packet)
    {
        int _id = _packet.ReadInt();
        Vector3 _position = _packet.ReadVector3();

        if (GameManager.players.TryGetValue(_id, out PlayerManager _player))
        {
            Debug.Log($"id : {_id},realposition : {_player.transform.position}, position : {_position}");
            _player.transform.position = _position;
        }
    }

    public static void PlayerRotation(Packet _packet)
    {
        int _id = _packet.ReadInt();
        Quaternion _rotation = _packet.ReadQuaternion();

        if (GameManager.players.TryGetValue(_id, out PlayerManager _player))
        {
            _player.transform.rotation = _rotation;
        }
    }

    public static void PlayerHP(Packet _packet)
    {
        int _id = _packet.ReadInt();
        int _hp = _packet.ReadInt();

        GameManager.players[_id].SetHealth(_hp);
    }

    public static void PlayerRespawn(Packet _packet)
    {
        int _id = _packet.ReadInt();

        GameManager.players[_id].Respawn();
    }
    public static void PlayerFire(Packet _packet)
    {
        int _id = _packet.ReadInt();

        GameManager.players[_id].FireSound(GameManager.players[_id].fire_Sound);
    }

}
