using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunController : MonoBehaviour
{
    [SerializeField]
    private Gun holdingGun;
    private CrossHair theCH;

    private float fireRate;

    private bool reloading = false;
    public bool aiming = false;
    private bool firing = false;

    private float aimingSmooth = 0;

    [SerializeField]
    private Vector3 notAimPos;

    private RaycastHit hitInfo;

    [SerializeField]
    private Camera theCam;

    [SerializeField]
    private GameObject hit_effect;

    [SerializeField]
    private PlayerController theplayer;

    private void Start()
    {
        notAimPos = Vector3.zero;
        theCH = FindObjectOfType<CrossHair>();
        theplayer = FindObjectOfType<PlayerController>();
    }

    void Update()
    {
        if (theplayer.running == false)
        {
            FireRateCalc();
            Reload();
            Launch();
            Aim();
        }
    }

    private void FireRateCalc()
    {
        if (fireRate > 0)
            fireRate -= Time.deltaTime;
    }

    private void Launch()
    {
        if (Input.GetButton("Fire1") && fireRate <= 0)
        {
            Ignite();
        }
    }

    private void Ignite()
    {
        if (!reloading && aimingSmooth == 0)
        {
            if (holdingGun.magazineBulletCount > 0)
                Fire();
            else
                StartCoroutine(ReloadCoroutine());
        }
    }
    private void Fire()
    {
        firing = true;
        theCH.FireAnim();
        holdingGun.magazineBulletCount--;
        fireRate = holdingGun.fireRate;
        ClientSend.PlayerFire();
        holdingGun.muzzleFlash.Play();
        Hit();
        StopAllCoroutines();
        StartCoroutine(ReboundCoroutine());
    }

    private void Reload()
    {
        if (Input.GetKeyDown(KeyCode.R) && !reloading && holdingGun.magazineBulletCount < holdingGun.magazine)
        {
            StartCoroutine(ReloadCoroutine());
        }
    }

    IEnumerator ReloadCoroutine()
    {
        if (holdingGun.bulletCount > 0)
        {
            reloading = true;
            holdingGun.anim.SetTrigger("Reload");

            holdingGun.bulletCount += holdingGun.magazineBulletCount;
            holdingGun.magazineBulletCount = 0;

            yield return new WaitForSeconds(holdingGun.reloadTime);

            if (holdingGun.bulletCount >= holdingGun.magazine)
            {
                holdingGun.magazineBulletCount = holdingGun.magazine;
                holdingGun.bulletCount -= holdingGun.magazine;
            }
            else
            {
                holdingGun.magazineBulletCount = holdingGun.bulletCount;
                holdingGun.bulletCount = 0;
            }
            reloading = false;
        }
        else
        {
            Debug.Log("총알없음");
        }
    }

    public void CancelReload()
    {
        if (reloading)
        {
            StopAllCoroutines();
            reloading = false;
        }
    }

    private void Aim()
    {
        if (Input.GetButtonDown("Fire2") && !firing)
        {
            aiming = true;
            Magnifi();
        }
        else if (Input.GetButtonUp("Fire2") && !firing)
        {
            aiming = false;
            Magnifi();
        }
    }
    public void CancelAim()
    {
        aiming = false;
        Magnifi();
    }

    private void Magnifi()
    {
        theCH.AimingAnim(aiming);
        holdingGun.anim.SetBool("Aimer", aiming);

        if (aiming)
        {
            StopCoroutine(MagnifiCoroutine());
            StopCoroutine(DeMagnifiCoroutine());
            StartCoroutine(MagnifiCoroutine());
        }
        else
        {
            StopCoroutine(MagnifiCoroutine());
            StopCoroutine(DeMagnifiCoroutine());
            StartCoroutine(DeMagnifiCoroutine());
        }
    }

    IEnumerator MagnifiCoroutine()
    {
        while (holdingGun.transform.localPosition != holdingGun.AimPos)
        {
            aimingSmooth++;
            holdingGun.transform.localPosition = Vector3.Lerp(holdingGun.transform.localPosition, holdingGun.AimPos, 0.2f);
            yield return null;
            if (aimingSmooth > 30)
            {
                holdingGun.transform.localPosition = holdingGun.AimPos;
            }
        }
        aimingSmooth = 0;
    }

    IEnumerator DeMagnifiCoroutine()
    {
        while (holdingGun.transform.localPosition != notAimPos)
        {
            aimingSmooth++;
            holdingGun.transform.localPosition = Vector3.Lerp(holdingGun.transform.localPosition, notAimPos, 0.2f);
            yield return null;
            if (aimingSmooth > 30)
            {
                holdingGun.transform.localPosition = notAimPos;
            }
        }
        aimingSmooth = 0;
    }
    IEnumerator ReboundCoroutine()
    {
        Vector3 aimRebound = new Vector3(holdingGun.reboundAimForce, holdingGun.AimPos.y, holdingGun.AimPos.z);
        Vector3 notAimRebound = new Vector3(holdingGun.reboundNormalForce, notAimPos.y, notAimPos.z);

        if (!aiming)
        {
            holdingGun.transform.localPosition = notAimPos;

            while (holdingGun.transform.localPosition.x <= holdingGun.reboundNormalForce - 0.02f)
            {
                holdingGun.transform.localPosition = Vector3.Lerp(holdingGun.transform.localPosition, notAimRebound, 0.4f);
                yield return null;
            }

            while (holdingGun.transform.localPosition.x >= notAimPos.x + 0.02f)
            {
                holdingGun.transform.localPosition = Vector3.Lerp(holdingGun.transform.localPosition, notAimPos, 0.2f);
                yield return null;
            }
            holdingGun.transform.localPosition = notAimPos;
        }
        else
        {
            holdingGun.transform.localPosition = holdingGun.AimPos;

            while (holdingGun.transform.localPosition.x <= holdingGun.reboundAimForce - 0.02f)
            {
                holdingGun.transform.localPosition = Vector3.Lerp(holdingGun.transform.localPosition, aimRebound, 0.4f);
                yield return null;
            }

            while (holdingGun.transform.localPosition.x >= holdingGun.AimPos.x + 0.02f)
            {
                holdingGun.transform.localPosition = Vector3.Lerp(holdingGun.transform.localPosition, holdingGun.AimPos, 0.2f);
                yield return null;
            }
            holdingGun.transform.localPosition = holdingGun.AimPos;
        }
        firing = false;
    }

    private void Hit()
    {
        if (Physics.Raycast(theCam.transform.position, theCam.transform.forward + new Vector3(Random.Range(-theCH.GetAccuracy() - holdingGun.accuracy, theCH.GetAccuracy() + holdingGun.accuracy), Random.Range(-theCH.GetAccuracy() - holdingGun.accuracy, theCH.GetAccuracy() + holdingGun.accuracy), 0), out hitInfo))
        {
            int hitID;
            GameObject clone = Instantiate(hit_effect, hitInfo.point, Quaternion.LookRotation(hitInfo.normal));
            if (hitInfo.transform.name == "Player(Clone)")
            {
                hitID = hitInfo.transform.GetComponent<PlayerManager>().id;
                Debug.Log(hitID);
                ClientSend.PlayerHit(hitID);
            }
            Debug.Log(hitInfo.transform.name);
            Destroy(clone, 2f);
        }
    }
    public bool GetAim()
    {
        return aiming;
    }
}
