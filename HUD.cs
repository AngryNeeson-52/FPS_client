using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    [SerializeField]
    private Gun theGun;
    [SerializeField]
    private PlayerManager thePlayer;
    [SerializeField]
    private GameObject BulletHUD;
    [SerializeField]
    private Text[] text_Bullet;

    void Update()
    {
        CheckBullet();
        CheckHP();
    }

    private void CheckBullet()
    {
        text_Bullet[0].text = theGun.magazineBulletCount.ToString();
        text_Bullet[1].text = theGun.magazine.ToString();
        text_Bullet[2].text = theGun.bulletCount.ToString();
    }
    private void CheckHP()
    {
        text_Bullet[3].text = thePlayer.hp.ToString();
        if (thePlayer.hp <= 0)
        {
            theGun.magazineBulletCount = theGun.magazine;
            theGun.bulletCount = theGun.maxBulletCount;
            thePlayer.Die();
        }
    }
}