using System.Collections;
using System.IO.Compression;
using System.Threading;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Gun : MonoBehaviour
{
    [Header("Variables")]
    public int currentAmmo;
    public int magazineSize;
    public int damage;

    public float reloadTime;
    public float fireRateTime;

    [Space]

    public float aimSpeed;

    public Vector3 aimDownScope;
    public Vector3 hipFire;

    [Space]

    public float startingSpread = 0.2f;
    public float maxSpread = 4f;
    public float spreadIncrease = 0.5f;
    public float spreadRecovery = 2f;
    public float aimingSpread = 1f;

    [Space]

    public float recoilX;
    public float recoilY;
    public float recoilZ;

    [Space]

    public float aimRecoilX;
    public float aimRecoilY;
    public float aimRecoilZ;

    [Header("Checks")]
    public bool canShoot;
    public bool isReloading;
    public bool isAiming;

    [Header("References")]
    public InputActionReference shootInput;
    public InputActionReference reloadInput;
    public InputActionReference aimInput;

    [Space]

    public TMP_Text ammoText;
    public GameObject Crosshair;

    [Space]

    public GameObject bulletPrefab;
    public Transform shootPoint;
    public Camera playerCamera;
    public Recoil recoil;

    float _shootTimer;
    float _currentSpread;
    float _lastShotTime;

    private Coroutine runningCoroutine;

    private void Start()
    {
        _shootTimer = fireRateTime;

        hipFire = transform.localPosition;

        _currentSpread = startingSpread;

        UpdateUI();
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time - _lastShotTime > 0.2f)
        {
            _currentSpread = Mathf.MoveTowards(_currentSpread, startingSpread, spreadRecovery * Time.deltaTime);
        }

        _shootTimer -= Time.deltaTime;

        if (_shootTimer <= 0)
        {
            canShoot = true;
        }
        else
        {
            canShoot = false;
        }

        if (aimInput.action.IsPressed())
        {
            transform.localPosition = Vector3.Lerp(transform.localPosition, aimDownScope, aimSpeed * Time.deltaTime);
            isAiming = true;
            WeaponSway.Instance.isAiming = isAiming;
            Crosshair.SetActive(false);
        }
        else
        {
            transform.localPosition = Vector3.Lerp(transform.localPosition, hipFire, aimSpeed * Time.deltaTime);
            isAiming = false;
            WeaponSway.Instance.isAiming = isAiming;
            Crosshair.SetActive(true);
        }

        if (shootInput.action.WasPressedThisFrame() && currentAmmo > 0 && !isReloading && canShoot)
        {
            Shoot();
        }
        else if (reloadInput.action.IsPressed() && !isReloading && runningCoroutine == null)
        {
            runningCoroutine = StartCoroutine(Reload());
            isReloading = true;
        }
        else if (currentAmmo <= 0 && runningCoroutine == null)
        {
            runningCoroutine = StartCoroutine(Reload());
            isReloading = true;
        }
    }

    IEnumerator Reload()
    {
        yield return new WaitForSeconds(reloadTime);
        currentAmmo = magazineSize;
        UpdateUI();
        runningCoroutine = null;
        isReloading = false;
    }

    void Shoot()
    {
        if (isAiming)
        {
            _currentSpread = aimingSpread;
        }
        else
        {
            _currentSpread = Mathf.Min(_currentSpread + spreadIncrease, maxSpread);
        }

        _lastShotTime = Time.time;

        Ray ray = playerCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));

        float spread = _currentSpread;

        ray.direction += playerCamera.transform.right * Random.Range(-spread, spread) / 100f;

        ray.direction += playerCamera.transform.up * Random.Range(-spread, spread) / 100f;

        Vector3 aimPoint;

        if (Physics.Raycast(ray, out RaycastHit hit, 1000f))
        {
            aimPoint = hit.point;
        }
        else
        {
            aimPoint = ray.GetPoint(1000f);
        }

        Vector3 direction = (aimPoint - shootPoint.position).normalized;

        Instantiate(bulletPrefab, shootPoint.position, Quaternion.LookRotation(direction));

        currentAmmo--;
        _shootTimer = fireRateTime;

        if (isAiming)
        {
            recoil.GunRecoil(aimRecoilX, aimRecoilY, aimRecoilZ);
        }
        else
        {
            recoil.GunRecoil(recoilX, recoilY, recoilZ);
        }

        UpdateUI();
    }

    void UpdateUI()
    {
        ammoText.text = currentAmmo.ToString() + " / " + magazineSize.ToString();
    }
}
