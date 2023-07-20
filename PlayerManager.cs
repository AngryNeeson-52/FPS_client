using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public int id;
    public string username;
    public int hp;
    public int max_hp = 100;
    public GameObject model;

    private AudioSource audioSource;
    public AudioClip fire_Sound;


    public void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }
    public void SetHealth(int _hp)
    {
        hp = _hp;

        if (hp <= 0 && model.name == "Player(Clone)")
        {
            Die();
        }
    }

    public void Die()
    {
        if (model.name == "LocalPlayer(Clone)")
        {
            UIManager.instance.Died(id);
        }
        UIManager.instance.Announce(1, username);
        model.SetActive(false);
    }

    public void Respawn()
    {
        model.SetActive(true);
        hp = max_hp;
        model.transform.position = new Vector3(50, 1, 50);
        UIManager.instance.Announce(2, username);
    }

    public void FireSound(AudioClip _clip)
    {
        audioSource.clip = _clip;
        audioSource.Play();
    }
}
