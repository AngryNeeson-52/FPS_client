using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrossHair : MonoBehaviour
{
    [SerializeField]
    private Animator anim;

    [SerializeField]
    private GameObject crossHairHUD;

    [SerializeField]
    private GunController theGunController;

    private float gunAccuracy;

    public void WalkingAnim(bool _flag)
    {
        anim.SetBool("Walking", _flag);
    }
    public void RunningAnim(bool _flag)
    {
        anim.SetBool("Running", _flag);
    }
    public void JumpingAnim(bool _flag)
    {
        anim.SetBool("Running", _flag);
    }
    public void AimingAnim(bool _flag)
    {
        anim.SetBool("Aiming", _flag);
    }
    public void FireAnim()
    {
        if (anim.GetBool("Walking"))
        {
            anim.SetTrigger("Walk_Fire");
        }
        else
        {
            anim.SetTrigger("Idle_Fire");
        }
    }

    public float GetAccuracy()
    {
        if (anim.GetBool("Walking"))
        {
            gunAccuracy = 0.1f;
        }
        else if (theGunController.GetAim())
        {
            gunAccuracy = 0.001f;
        }
        else
        {
            gunAccuracy = 0.05f;
        }

        return gunAccuracy;
    }
}
