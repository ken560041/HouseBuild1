using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField] float health = 5;
    [SerializeField] GameObject hitVFX;
    [SerializeField] GameObject ragdoll;

    [Header("Combat")]
    [SerializeField] float attackCD = 3f;
    [SerializeField] float attackRange = 1f;
    [SerializeField] float aggroRange = 4f;


    GameObject player;
    NavMeshAgent agent;
    Animator animator;
    float timePassed;
    float newDestinationCD = 0.5f;
    void Start()
    {
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player");
    }


    // Update is called once per frame
    void Update()
    {
        //animator.SetFloat("speed", agent.velocity.magnitude / agent.speed);


        if (player == null)
        {
            return;

        }

        if(timePassed>attackCD)
        {
            if (Vector3.Distance(player.transform.position, transform.position) < attackRange)
            {
                animator.SetTrigger("attack");
                timePassed = 0;
            }

        }
        timePassed += Time.deltaTime;

        if(newDestinationCD<=0 && Vector3.Distance(player.transform.position, transform.position) < aggroRange)// bán kính hoạt động của quái 
        {
            newDestinationCD = 0.5f;
            agent.SetDestination(player.transform.position);
        }

        newDestinationCD-=Time.deltaTime;
        Vector3 direction = (player.transform.position - transform.position).normalized;
        direction.y = 0;
        Quaternion targetRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 5f);
    }


    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            print("true");
            player=collision.gameObject;
        }
    }


    void Die()
    {
        Instantiate(ragdoll, transform.position, transform.rotation);
        Destroy(this.gameObject);
    }

    public void TakeDamage(float damageAmount)
    {
        health-=damageAmount;
        animator.SetTrigger("damage");
        //CameraShake.Instance

        if (health <= 0)
        {
            Die();
        }
    }

    public void StartDealDamage()
    {
        GetComponentInChildren<EnemyDamageDealer>().StartDealDamage();
    }


    public void EndDealDamage()
    {
        GetComponentInChildren<EnemyDamageDealer>().EndDealDamage();
    }

    public void HitVFX(Vector3 hitPosition)
    {
        GameObject hit = Instantiate(hitVFX, hitPosition, Quaternion.identity);
        Destroy(hit, 3f);
    }
}
