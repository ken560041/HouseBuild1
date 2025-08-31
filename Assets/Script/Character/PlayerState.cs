using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState : MonoBehaviour
{
    // Start is called before the first frame update

    public static PlayerState Instance { get; set; }
    //Player Health
    public float currentHealth;
    public float maxHealth;


    public float currentCalories;
    public float maxCalories;

    public float currentHydration;
    public float maxHydration;



    float distanceTravelled = 0;
    Vector3 lastPosition;
    public GameObject playerBody;
    private void Awake()
    {
        if(Instance!=null&& Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }
    void Start()
    {
        currentHealth = maxHealth;
        currentCalories = maxCalories;
    }

    // Update is called once per frame
    void Update()
    {

        distanceTravelled += Vector3.Distance(playerBody.transform.position, lastPosition);
        lastPosition=playerBody.transform.position;
        if(distanceTravelled >=5 ) {

            distanceTravelled = 0;
            currentCalories -= 1;       
         }



        if (Input.GetKeyDown(KeyCode.N))
        {
            currentHealth -= 10;
            
        }
    }
}
