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
            Shootable shootcomponent = hit.GetComponent<Shootable>();

            if (rb != null && shootcomponent != null)
            {
                rb.AddExplosionForce(power, explosionPos, radius, 3.0f, ForceMode.Impulse);
                Renderer renderer = shootcomponent.GetComponent<Renderer>();
                Color randomColor = new Color(Random.value, Random.value, Random.value);
                renderer.material.color = randomColor;
            }
        }

        StartCoroutine(GeneratePS());
        Destroy(gameObject);
    }

    private IEnumerator GeneratePS()
    {
        GameObject ps = Instantiate(explosionPrefab, transform.position, Quaternion.identity);
        yield return new WaitForSeconds(anidelay);
        Destroy(ps);
    }
}
