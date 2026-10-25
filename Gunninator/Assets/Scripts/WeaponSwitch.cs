using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WeaponSwitch : MonoBehaviour
{
    public List<GameObject> Weapons;
    public int currentWeaponIndex;

    public GameObject currentWeapon;

    private void Start()
    {
        SwitchWeapon(0);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
            SwitchWeapon(0);

        if (Input.GetKeyDown(KeyCode.Alpha2))
            SwitchWeapon(1);

        if (Input.GetKeyDown(KeyCode.Alpha3))
            SwitchWeapon(2);

        if (Input.GetKeyDown(KeyCode.Alpha4))
            SwitchWeapon(3);
    }

    private void SwitchWeapon(int index)
    {
        if (index < 0 || index >= Weapons.Count)
            return;

        // Disable the current weapon
        if (currentWeapon != null)
        {
            Gun _disableGun = currentWeapon.GetComponent<Gun>();
            _disableGun.StopAllCoroutines();
            _disableGun.isReloading = false;
            _disableGun.runningCoroutine = null;
            currentWeapon.SetActive(false);
        }

        // Set the new weapon
        currentWeaponIndex = index;
        currentWeapon = Weapons[currentWeaponIndex];
        Gun _enableGun = currentWeapon.GetComponent<Gun>();
        _enableGun.UpdateUI();
        currentWeapon.SetActive(true);
    }
}
