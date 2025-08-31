using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class HealthSystem : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] float health = 100;
    [SerializeField] GameObject hitVFX;
    [SerializeField] GameObject ragdoll;

    Animator animator;
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void TakeDamage(float damageAmount)
    {
        health -= damageAmount;
        animator.SetTrigger("damage");
        //CameraShake.Instance.ShakeCamera(2f, 0.2f);

        if (health <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Instantiate(ragdoll, transform.position, transform.rotation);
        Destroy(this.gameObject);
    }
    public void HitVFX(Vector3 hitPosition)
    {
        GameObject hit = Instantiate(hitVFX, hitPosition, Quaternion.identity);
        Destroy(hit, 3f);

    }
}
