using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class Shooter : MonoBehaviour
{


    private Camera cam;
    private Weapon.WeaponType equiptWeapon;
    private bool shooting;

    [SerializeField] private GameObject particleSysPrefab;
    [SerializeField] private GameObject grenadePrefab;
    [SerializeField] private float grenadeImpulse = 2f;
    [SerializeField] private float maxImpulse = 20f;
    [SerializeField] private float grenadeCountDown = 0.2f;
    [SerializeField] private float impulseStrength = 5.0f;
    [SerializeField] private float attackRate = 1f;
    [SerializeField] private int pelletCount = 10;
    [SerializeField] private float deviationAngle = 5.0f;
    [SerializeField] private AudioClip GunShotSound;
    [SerializeField] private AudioClip GunShotSoundSG;
    [SerializeField] private AudioSource SoundSource;
    




    
    void Start()
    {
        cam = GetComponent<Camera>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void OnGUI()
    {
        int size = 12;
        float posX = cam.pixelWidth / 2 - size / 4;
        float posY = cam.pixelHeight / 2 - size / 2;
        GUI.Label(new Rect(posX, posY, size, size), "*");
    }

    void Update()
    {
        GetInput();
       
    }

    private void GetInput()
    {
        if (equiptWeapon == Weapon.WeaponType.PISTOL)
        {
            if (Input.GetMouseButtonDown(0))
            {
                Shoot();
            }
        }
        else if (equiptWeapon == Weapon.WeaponType.SHOTGUN)
        {
            if (Input.GetMouseButtonDown(0))
            {
                ShotGunShoot();
            }
        }
        else if (equiptWeapon == Weapon.WeaponType.AK)
        {
            if (Input.GetMouseButton(0))
            {
                if (!IsInvoking("Shoot"))
                {
                 
                    InvokeRepeating("Shoot", 0f, attackRate);
                } 
            }
            else if(Input.GetMouseButtonUp(0))
            {
                CancelInvoke("Shoot");
            }
        }else if(equiptWeapon == Weapon.WeaponType.BOMB)
        {
            HandleGrenadeThrowing();
        }
    }

    private void HandleGrenadeThrowing()
    {

        if (Input.GetMouseButton(0))
        {
            if (!IsInvoking("IncreaseGrenadeImpulse"))
            {
                InvokeRepeating("IncreaseGrenadeImpulse", 0f, grenadeCountDown);
          
            }
            
        }

        if (Input.GetMouseButtonUp(0))
        {
          
            CancelInvoke("IncreaseGrenadeImpulse");
            GameObject grenade = Instantiate(grenadePrefab, transform.position + cam.transform.forward * 2, transform.rotation);
            Rigidbody target = grenade.GetComponent<Rigidbody>();
            Vector3 impulse = cam.transform.forward * grenadeImpulse;
            target.AddForceAtPosition(impulse, cam.transform.position, ForceMode.Impulse);
            grenadeImpulse = 0f;
        }
    }


    private void IncreaseGrenadeImpulse()
    {
        if(grenadeImpulse < maxImpulse)
        {
            grenadeImpulse += 1f;
        }
    }

    private void Shoot()
    {
        SoundSource.PlayOneShot(GunShotSound);
        Vector3 point = new Vector3(cam.pixelWidth / 2, cam.pixelHeight / 2, 0);
        Ray ray = cam.ScreenPointToRay(point);
        ray.direction = Quaternion.Euler(UnityEngine.Random.Range(-deviationAngle, deviationAngle),
                                          UnityEngine.Random.Range(-deviationAngle, deviationAngle),
                                          UnityEngine.Random.Range(-deviationAngle, deviationAngle)) * ray.direction;

        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            GameObject hitObject = hit.transform.gameObject;
            StartCoroutine(GeneratePS(hit));

            Shootable target = hitObject.GetComponent<Shootable>();
            if (target != null)
            {
                Vector3 impulse = Vector3.Normalize(hit.point - transform.position) * impulseStrength;
                hit.rigidbody.AddForceAtPosition(impulse, hit.point, ForceMode.Impulse);
                target.ReduceHealth(1);
            }
        }

    }


    private void ShotGunShoot()
    {
        SoundSource.PlayOneShot(GunShotSoundSG);
        Vector3 point = new Vector3(cam.pixelWidth / 2, cam.pixelHeight / 2, 0);
        Ray originalRay = cam.ScreenPointToRay(point);
        for (int i = 0; i < pelletCount; i++)
        {
           
            Ray ray = new Ray(originalRay.origin, originalRay.direction);
            ray.direction = Quaternion.Euler(UnityEngine.Random.Range(-deviationAngle, deviationAngle),
                                             UnityEngine.Random.Range(-deviationAngle, deviationAngle),
                                             UnityEngine.Random.Range(-deviationAngle, deviationAngle)) * ray.direction;

            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                GameObject hitObject = hit.transform.gameObject;
                StartCoroutine(GeneratePS(hit));

                Shootable target = hitObject.GetComponent<Shootable>();
                if (target != null)
                {
                    Vector3 impulse = Vector3.Normalize(hit.point - transform.position) * impulseStrength;
                    hit.rigidbody.AddForceAtPosition(impulse, hit.point, ForceMode.Impulse);
                    target.ReduceHealth(1);
                }
            }
        }
    }


    private IEnumerator GeneratePS(RaycastHit hit)
    {
        GameObject ps = Instantiate(particleSysPrefab, hit.point, Quaternion.LookRotation(hit.normal));
        yield return new WaitForSeconds(1);
        Destroy(ps);
    }

    public void EquiptPistol()
    {
        equiptWeapon = Weapon.WeaponType.PISTOL;
    }

    public void EquiptAk()
    {
        equiptWeapon = Weapon.WeaponType.AK;
    }

    public void EquiptShotgun()
    {
        equiptWeapon = Weapon.WeaponType.SHOTGUN;
    }

    public void EquiptBomb()
    {
        equiptWeapon = Weapon.WeaponType.BOMB;
    }
}
