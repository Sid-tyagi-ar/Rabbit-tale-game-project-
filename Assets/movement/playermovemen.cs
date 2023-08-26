using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playermovemen : MonoBehaviour
{
    private Rigidbody2D body;
    [SerializeField] private float speed; //just makinng it easier to control speed and jump value from the unity engine itself 
    [SerializeField] private float jumpPower;
    [SerializeField] private float dashPower;
    [SerializeField] private LayerMask groundLayer;
    private float cooldownTime = dashCooldown;
    private const float dashCooldown = 1.3f;
    private WaitForSeconds timegap = new WaitForSeconds(5f); // Interval between repetitions
    private BoxCollider2D boxCollider;
    private float horizontal;
    private int maker = 0;
    private Animator anim;
    [SerializeField] private GameObject prefab;

    
    
    private void Awake()
    {

        body = GetComponent<Rigidbody2D>();
        boxCollider = GetComponent<BoxCollider2D>();
        anim = GetComponent<Animator>();
    }
    private void Start()
    {
        StartCoroutine(RepeatCommand());
    }


    void Update()
    {
        //speed controller :-
        horizontal = Input.GetAxis("Horizontal") ; //getaxis is basically getting the orientation 
        body.velocity = new Vector2(horizontal * speed, body.velocity.y); // by declaring new vector which changes upon speed we have SPEEEEEEEEEEEEEEEEEED  MOVEMENT
        //flip mechanism 
        if(horizontal> 0.01f)
        {
            transform.localScale = new Vector3(-4, 4, 1); // localscale transform changes size of game object accordingly
        }
        else if(horizontal < -0.01f)
            transform.localScale = new Vector3(4, 4, 1);
        
        //dash mechanics-
        Dash();
        Gravswitch();
        anim.SetBool("run", horizontal !=0);
        Fallspeed();
        floormaker();
       
    }
     
     //Jumping logic ->

    private bool isGrounded() // creates box around the player to check if layers touch each other or not
    {
         RaycastHit2D raycastHit =  Physics2D.BoxCast(boxCollider.bounds.center,boxCollider.bounds.size,0,Vector2.down,0.1f, groundLayer);
         return raycastHit.collider != null;
    }
        
    private void Jump()
    {
        if (isGrounded())
            {
            body.velocity = new Vector2(body.velocity.x,jumpPower);
            }

    }
    
    //Making Periodic jump mechanic ->
    private IEnumerator RepeatCommand()
    {
        while (true)
        {
            // Command to be repeated
            Jump();

            yield return timegap; // Wait for the specified interval
        }
    }

    //dash mechanic-
    
    private void Dash()
    {
        if (!isGrounded())
            {
            cooldownTime -= Time.deltaTime;

                if (cooldownTime <= 0)
                {
                if(Input.GetKeyDown(KeyCode.W))
                {
                body.velocity = new Vector2(1,dashPower/3);
                cooldownTime = dashCooldown;
                }
                 if(Input.GetKeyDown(KeyCode.P))
                {
                body.velocity = new Vector2(body.velocity.x*dashPower,1);
                cooldownTime = dashCooldown;
                }
               //  if(Input.GetKeyDown(KeyCode.E))
                //{
               // body.velocity = new Vector2(dashPower,1);
                //cooldownTime = dashCooldown;
               // }
                }
            }  
    }
    private void Gravswitch()
    {
        if(Input.GetKeyDown(KeyCode.G))
        {
            body.gravityScale *= -1;
        }

    }
    private void Fallspeed()
    {
        int i;
        if(!isGrounded())
        {
            for (i = 1; i < 5 ; ++i)
            {
            body.gravityScale += 0.0005f;
            }
        }
        if(isGrounded())
        {
            body.gravityScale = 1;
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Floormaker"))
        {
        maker += 1;
        Destroy(collision.gameObject);
        Debug.Log("g");
        }
    }
    private void floormaker()
    {
        if(!isGrounded()&&maker == 1&&Input.GetKeyDown(KeyCode.F))
        {
            Vector3 playerPosition = body.transform.position;
            body.velocity = new Vector2(0,0); 
            GameObject newPrefab = Instantiate(prefab, playerPosition + Vector3.down, Quaternion.identity);
            maker = maker - 1;
            Debug.Log("m");
        }
    }


}
