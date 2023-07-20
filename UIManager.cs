using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    public GameObject startMenu;
    public GameObject restartMenu;
    public GameObject systemAnnounce;
    public InputField userNameField;
    public InputField ip;
    public int id;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Debug.Log("Instance already exsists, destroying object!");
            Destroy(this);
        }
    }

    public void ConnectToServer()
    {
        Client.instance.ip = instance.ip.text;
        startMenu.SetActive(false);
        userNameField.interactable = false;
        Client.instance.ConnectToServer();
    }

    public void Died(int _id)
    {
        id = _id;
        restartMenu.SetActive(true);
    }

    public void Restart()
    {
        GameManager.players[id].Respawn();
        ClientSend.PlayerRespawn(id);
        restartMenu.SetActive(false);
    }

    public void Announce(int _situ, string _id)
    {
        GameObject _message = Instantiate(systemAnnounce);
        _message.transform.SetSiblingIndex(0);
        Announce announceFunc = _message.GetComponent<Announce>();
        announceFunc.MessageSet(_situ, _id);
    }
}