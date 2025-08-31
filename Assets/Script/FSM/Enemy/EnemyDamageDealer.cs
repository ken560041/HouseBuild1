using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDamageDealer : MonoBehaviour
{
    // Start is called before the first frame update
    bool canDealDamage;
    bool hasDealtDamage;

    [SerializeField] float weaponLength;
    [SerializeField] float weaponDamage;
    void Start()
    {
        canDealDamage = false;
        hasDealtDamage = false;
    }

    // Update is called once per frame
    void Update()
    {
        /*if (canDealDamage && !hasDealtDamage)
        {
            RaycastHit hit;

            int layerMask = LayerMask.GetMask("Player");
            if (Physics.Raycast(transform.position, -transform.up, out hit, weaponLength, layerMask))
            {
                if (hit.transform.TryGetComponent(out HealthSystem health))
                {
                    health.TakeDamage(weaponDamage);
                    health.HitVFX(hit.point);
                    hasDealtDamage = true;
                }
            }
        }*/
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!canDealDamage || hasDealtDamage) return; // Chỉ gây sát thương khi có thể

        if (other.CompareTag("Player")) // Chỉ tấn công Player
        {
            if (other.TryGetComponent(out HealthSystem health))
            {
                health.TakeDamage(weaponDamage);
                health.HitVFX(other.ClosestPoint(transform.position)); // Hiệu ứng đánh trúng
                hasDealtDamage = true; // Chỉ đánh một lần
            }
        }
    }

    public void StartDealDamage()
    {
        canDealDamage = true;
        hasDealtDamage = false;
    }
    public void EndDealDamage()
    {
        canDealDamage = false;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position, transform.position - transform.up * weaponLength);
    }
}
