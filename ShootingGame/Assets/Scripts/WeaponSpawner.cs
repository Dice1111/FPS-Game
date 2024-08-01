using System.Collections;
using UnityEngine;

public class WeaponSpawner : MonoBehaviour
{
    [SerializeField] private GameObject gun;
    [SerializeField] private GameObject attach;
    private bool isSpawning = false;

    private void Start()
    {
        SpawnGun();
    }

    private void Update()
    {
        if (!isSpawning && attach.transform.childCount == 0)
        {
            StartCoroutine(GenerateGun());
        }
    }

    private void SpawnGun()
    {
        GameObject newGun = Instantiate(gun, attach.transform.position, attach.transform.rotation);
        newGun.transform.SetParent(attach.transform);
    }

    private IEnumerator GenerateGun()
    {
        isSpawning = true;
        yield return new WaitForSeconds(4);
        SpawnGun();
        isSpawning = false;
    }
}
