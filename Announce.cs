using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Announce : MonoBehaviour
{
    public Text announceMessage;

    void Start()
    {
        Destroy(this.gameObject, 5f);
    }

    public void MessageSet(int _situ, string _id)
    {
        if (_situ == 0)
        {
           announceMessage.text = $"{_id}님이 접속했습니다.";
        }
        else if (_situ == 1)
        {
            announceMessage.text = $"{_id}님이 죽었습니다.";
        }
        else if (_situ == 2)
        {
            announceMessage.text = $"{_id}님이 되살아났습니다.";
        }
    }
}
