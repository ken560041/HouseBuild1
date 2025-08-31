using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentSystem : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] GameObject weaponHolder;
    [SerializeField] GameObject weapon;
    [SerializeField] GameObject weaponSheath;
    GameObject currentWeaponInHand;
    //GameObject currentWeaponInSheath;

    void Start()
    {
        //currentWeaponInSheath = Instantiate(weapon, weaponSheath.transform);
    }

    public void DrawWeapon()
    {
        currentWeaponInHand = Instantiate(weapon, weaponHolder.transform);
        //Destroy(currentWeaponInSheath);
    }

    public void SheathWeapon()
    {
        //currentWeaponInSheath = Instantiate(weapon, weaponSheath.transform);
        Destroy(currentWeaponInHand);
    }

    public void StartDealDamage()
    {
        DamageDealer damageDealer = currentWeaponInHand.GetComponentInChildren<DamageDealer>();

        if (damageDealer == null)
        {
            Debug.LogError("⚠ Lỗi: Vũ khí không có DamageDealer!");
            return;
        }

        damageDealer.StartDealDamage();
    }
    public void EndDealDamage()
    {
        currentWeaponInHand.GetComponentInChildren<DamageDealer>().EndDealDamage();
    }
}
