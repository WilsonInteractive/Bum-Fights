using System.Collections;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 50f;
    public int damage = 10;

    [Space]

    public float timeTillDestroyed;

    [Space]

    public GameObject particle;

    public LayerMask hitMask;

    private void Start()
    {
        StartCoroutine(DeleteObject());
    }

    void Update()
    {
        float distance = speed * Time.deltaTime;

        if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, distance, hitMask))
        {
            if (hit.collider.gameObject.CompareTag("Enemy"))
            {
                EnemyHealth enemy = hit.collider.GetComponent<EnemyHealth>();
                enemy.TakeDamage(damage);
            }

            Instantiate(particle, hit.point + (hit.normal * 0.1f), Quaternion.FromToRotation(Vector3.forward, hit.normal));

            StopAllCoroutines();
            Destroy(gameObject);

            return;
        }
        transform.position += transform.forward * distance;
    }

    IEnumerator DeleteObject()
    {
        yield return new WaitForSeconds(timeTillDestroyed);
        Destroy(gameObject);
    }
}
