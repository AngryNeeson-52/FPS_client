using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public string gunName;
    public float accuracy;
    public float fireRate;
    public float reloadTime;

    public int damage;

    public int magazine;
    public int magazineBulletCount;
    public int bulletCount;
    public int maxBulletCount;

    public float reboundNormalForce;
    public float reboundAimForce;

    public Vector3 AimPos;

    public Animator anim;

    public ParticleSystem muzzleFlash;
}
