using UnityEngine;

public class ParticleDestroy : MonoBehaviour
{
    public float destroyTime = 2f;

    private void Start()
    {
        Destroy(gameObject, destroyTime);
    }
}
