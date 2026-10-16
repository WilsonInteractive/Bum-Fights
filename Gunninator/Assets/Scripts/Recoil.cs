using UnityEngine;

public class Recoil : MonoBehaviour
{
    [Header("Recoil")]
    public float recoilReturnSpeed = 8f;
    public float snappiness = 20f;

    private Vector3 currentRotation;
    private Vector3 targetRotation;

    void Update()
    {
        targetRotation = Vector3.Lerp(targetRotation, Vector3.zero, recoilReturnSpeed * Time.deltaTime);

        currentRotation = Vector3.Lerp(currentRotation, targetRotation, snappiness * Time.deltaTime);

        transform.localRotation = Quaternion.Euler(currentRotation);
    }

    public void GunRecoil(float recoilX, float recoilY, float recoilZ)
    {
        targetRotation += new Vector3(-recoilX, Random.Range(-recoilY, recoilY), Random.Range(-recoilZ, recoilZ));
    }
}
