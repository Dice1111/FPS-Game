using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
   
    private GameObject player;
    private Camera cam;
    private Shooter shooter;

    public enum WeaponType
    {
        NOTHING,
        PISTOL,
        AK,
        SHOTGUN,
        BOMB
    }

    public WeaponType type;
    


    void Start()
    {
       
        cam = Camera.main;
        shooter = cam.GetComponent<Shooter>();
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) 
        {
            if (type == WeaponType.PISTOL)
            {
                shooter.EquiptPistol();
            }
            else if (type == WeaponType.AK)
            {
                shooter.EquiptAk();
            }
            else if (type == WeaponType.SHOTGUN)
            {
                shooter.EquiptShotgun();
            }else if (type == WeaponType.BOMB)
            {
                shooter.EquiptBomb();
            }
            
            Destroy(gameObject);
            visualDisable();
            visualActive();
        }
    }


    private void visualActive()
    {
        if (type == WeaponType.PISTOL)
        {
            cam.transform.GetChild(0).GetChild(0).gameObject.SetActive(true);
        }
        else if (type == WeaponType.AK)
        {
            cam.transform.GetChild(0).GetChild(1).gameObject.SetActive(true);
        }
        else if (type == WeaponType.SHOTGUN)
        {
            cam.transform.GetChild(0).GetChild(2).gameObject.SetActive(true);
        }else if (type == WeaponType.BOMB)
        {
            cam.transform.GetChild(0).GetChild(3).gameObject.SetActive(true);
        }

    }


    private void visualDisable()
    {
        cam.transform.GetChild(0).GetChild(0).gameObject.SetActive(false);
        cam.transform.GetChild(0).GetChild(1).gameObject.SetActive(false);
        cam.transform.GetChild(0).GetChild(2).gameObject.SetActive(false);
        cam.transform.GetChild(0).GetChild(3).gameObject.SetActive(false);
    }

    private IEnumerator Respawn()
    {
        
        yield return new WaitForSeconds(10f);
        gameObject.SetActive(true);
    }

}
