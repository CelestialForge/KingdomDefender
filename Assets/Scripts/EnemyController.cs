using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements.Experimental;

public class EnemyController : MonoBehaviour
{

    private GameManager gameManager;
    private float speedWander = 3;
    private float speedChase = 8;
    private float aggroRange = 8;
    private GameObject playerObj;
    private Rigidbody enemyRB;
    private Vector3 wanderTarget;
    private float areaLimit = 20;


    // Start is called before the first frame update
    void Start()
    {

        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        playerObj = GameObject.Find("Player");
        enemyRB = gameObject.GetComponent<Rigidbody>();
        wanderTarget = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (gameManager.isGameActive)
        {
            Move();
            ConstrainToArea(-areaLimit, areaLimit, -areaLimit, areaLimit); // X:-20,20; Z:-20,20
            
            // if it fell off
            if (enemyRB.position.y < -2)
            {
                Destroy(gameObject);
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        //Debug.Log("COLL:"+ collision.gameObject.name);
        if (collision.gameObject.CompareTag("Weapon"))
        {
            gameManager.score++;
            Destroy(gameObject);
        }
    }

    private Vector3 GetNewWanderTarget()
    {
        float theta = Random.Range(0, 360);
        float range = Random.Range(1.0f, 4.0f);
        Vector3 result = new Vector3(Mathf.Cos(theta), 0.0f, Mathf.Sin(theta));
        result *= range;
        result += transform.position;
        result.y = 0;
        return result;
    }

    private void Move()
    {
        Vector3 facingVelocity;
        // Get direction to player
        Vector3 dirToPlayer = (playerObj.transform.position - transform.position);
        // If in aggro range, chase player
        if (dirToPlayer.magnitude <= aggroRange)
        {
            dirToPlayer.y = 0;
            facingVelocity = dirToPlayer.normalized * speedChase;
            //transform.Translate(lineOfSight.normalized * speedChase * Time.deltaTime);
        }
        // Wander
        else
        {   
            // get direction to wanderTarget
            Vector3 dirToWanderTgt = wanderTarget - transform.position;
            // if we have reached our wander target, choose a new one
            if (dirToWanderTgt.magnitude <= 0.1)
            {
                wanderTarget = GetNewWanderTarget();
                dirToWanderTgt = wanderTarget - transform.position;
            }

            // move towards our wander target
            dirToWanderTgt.y = 0;
            facingVelocity = dirToWanderTgt.normalized * speedWander;
            
            //transform.Translate(lineToWanderTgt.normalized * speedWander * Time.deltaTime);
        }
        
        // facingVelocity = Direction * Speed        
        transform.Translate(facingVelocity * Time.deltaTime, relativeTo:Space.World);
        if (facingVelocity.magnitude > 0.1)
        {
            transform.LookAt(transform.position + facingVelocity);
        }
    }
    void ConstrainToArea(float minX = -20, float maxX = 20, float minZ = -20, float maxZ = 20)
    {
        float newX = transform.position.x;
        float newZ = transform.position.z;
        bool findNewTarget = false;

        if (newX > maxX)
        {
            newX = maxX;
            findNewTarget = true;
        }
        else if (newX < minX)
        {
            newX = minX;
            findNewTarget = true;
        }
        if (newZ > maxZ)
        {
            newZ = maxZ;
            findNewTarget = true;
        }
        else if (newZ < minZ)
        {
            newZ = minZ;
            findNewTarget = true;
        }
        transform.position = new Vector3 (newX, transform.position.y, newZ);

        if (findNewTarget)
        {
            // Find a new wander target in case old target is out of bounds
            wanderTarget = GetNewWanderTarget();
        }

    }
}
