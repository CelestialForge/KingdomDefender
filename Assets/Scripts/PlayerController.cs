using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private GameManager gameManager;
    private float speed = 10;
    private float jumpForce = 450;
    private float hurtReactionForce = 250;
    private int jumpLimit = 1;
    private int jumpCount = 0;
    private int lifeForce = 5;
    private Rigidbody playerRB;
    private GameObject mainCamera;
    private GameObject weaponHardPoint;
    public TextMeshProUGUI healthText;

    public GameObject weapon;
    public bool isOnGround = true;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        playerRB = GetComponent<Rigidbody>();
        mainCamera = GameObject.Find("FocalPoint");
        weaponHardPoint = GameObject.Find("WeaponHand");
    }

    // Update is called once per frame
    void Update()
    {
        if (gameManager.isGameActive)
        {
            // Snap Camera to player
            mainCamera.transform.position = transform.position;

            // Handle Movement
                float horizontalInput = Input.GetAxis("Horizontal");
                float verticalInput = Input.GetAxis("Vertical");
                Vector3 heading = new Vector3(horizontalInput, 0.0f, verticalInput);
                transform.Translate(heading * speed * Time.deltaTime, relativeTo: Space.World);

                // Player Rotation
                if (heading.magnitude > 0)
                {
                    transform.LookAt(heading + transform.position);
                }
            // update weapon if holding
            if (weapon != null)
            {
                weapon.transform.rotation = transform.rotation;
                weapon.transform.position = weaponHardPoint.transform.position;
            }
                  
            // Handle Jumps, multiples allowed if enabled
            if (Input.GetKeyDown(KeyCode.Space) && jumpCount < jumpLimit)
            { //isOnGround) {
                jump();
            }

            // If falls off the world
            if (transform.position.y < -1)
            {
                lifeForce = 0;
            }

            // If life depleted
            if (lifeForce <= 0)
            {
                gameManager.GameOver();
                //Destroy(gameObject);
            }
            healthText.text = "Health: " + lifeForce;
        }
    }

    void jump()
    {
        isOnGround = false;
        playerRB.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        jumpCount++;
    }

    private void OnCollisionEnter(Collision collision)
    {
        
        //Debug.Log(collision.gameObject.name);
        if (collision.gameObject.CompareTag("Ground"))
        {
            jumpCount = 0;
            isOnGround = true;
        }
        else if (collision.gameObject.CompareTag("Enemy"))
        {
            lifeForce--;
            Debug.Log("HP:" + lifeForce);
            Vector3 jumpBackDir = transform.position - collision.transform.position;
            jumpBackDir.y= 0;
            jumpBackDir.Normalize();
            playerRB.AddForce((jumpBackDir * hurtReactionForce) + (Vector3.up * hurtReactionForce), ForceMode.Impulse);
        }else if (collision.gameObject.CompareTag("Weapon") && weapon == null)
        {
            weapon = collision.gameObject;
        }
    }
}
