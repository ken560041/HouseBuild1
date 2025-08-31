using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageDealer : MonoBehaviour
{
    // Start is called before the first frame update

    bool canDealDamage;
    List<GameObject> hasDealtDamage;
    [SerializeField] float weeaponLength;
    [SerializeField] float weaponDamage;

    void Start()
    {
        canDealDamage = false;
        hasDealtDamage = new List<GameObject>();
    }

    // Update is called once per frame
    void Update()
    {
        if(canDealDamage)
        {
            RaycastHit hit;
            int layerMask = LayerMask.GetMask("Enemy");
            if (Physics.Raycast(transform.position, -transform.up, out hit, weeaponLength, layerMask))
            {
                if (hit.transform.TryGetComponent(out Enemy enemy) && !hasDealtDamage.Contains(hit.transform.gameObject))
                {
                    enemy.TakeDamage(weaponDamage);
                    enemy.HitVFX(hit.point);
                    hasDealtDamage.Add(hit.transform.gameObject);

                }
            }
            
        }
    }

    public void StartDealDamage()
    {
        canDealDamage = true;
        hasDealtDamage.Clear();
    }

    public void EndDealDamage()
    {
        canDealDamage = false;

    }


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position, transform.position - transform.up * weeaponLength);
    }
}
