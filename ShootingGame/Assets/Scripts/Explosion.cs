using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Explosion : MonoBehaviour
{
    public float radius = 5.0f;
    public float power = 10.0f;
    public float delay = 4.0f;
    public float anidelay = 1f;
    public GameObject explosionPrefab;
    public GameObject explosionSound;
    private AudioSource audioSource;



    private void OnCollisionEnter(Collision collision)
    {
        StartCoroutine(DelayedExplosion());
    }

    private IEnumerator DelayedExplosion()
    {
        yield return new WaitForSeconds(delay);

        Vector3 explosionPos = transform.position;
        Collider[] colliders = Physics.OverlapSphere(explosionPos, radius);

        foreach (Collider hit in colliders)
        {
            Rigidbody rb = hit.GetComponent<Rigidbody>();
            

            if (rb != null & hit.CompareTag("Enemy"))
            {
                rb.AddExplosionForce(power, explosionPos, radius, 3.0f, ForceMode.Impulse);
                Renderer renderer = hit.GetComponent<Renderer>();
                Color randomColor = new Color(Random.value, Random.value, Random.value);
                renderer.material.color = randomColor;
            }
        }


        GameObject bombExplodeSFX = Instantiate(explosionSound,transform.position,Quaternion.identity);
        GameObject ps = Instantiate(explosionPrefab, transform.position, Quaternion.identity);
        audioSource = bombExplodeSFX.GetComponent<AudioSource>();
        audioSource.Play();
        Destroy(audioSource,2f);
        Destroy(ps, 2f);
        Destroy(gameObject);
    }

  
}
