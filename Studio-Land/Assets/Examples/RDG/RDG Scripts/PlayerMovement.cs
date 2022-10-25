using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour{

    public static PlayerMovement instance { get; private set; }

    private void Awake()
    {
        //instatiating singleton instance
        if (instance == null)
        {
            Debug.Log("player created");
            instance = this; //sets it to this instance if script is running for the first time
            DontDestroyOnLoad(gameObject);

            gameObject.GetComponent<SpriteRenderer>().enabled = true;
            gravestone.SetActive(false);
        }
        else
        {
            Destroy(gameObject); //keeps only the first instance
        }
    }
    
    public float speed;

    private Animator animator;

    private bool gameStarted = false;

    [SerializeField] GameObject gravestone;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void Update()
    {

        if (GameManager.instance.GameEnded()){
            if (GameManager.instance.GameLost()) Death();
        }

        Vector2 dir = Vector2.zero;
        if (Input.GetKey(KeyCode.A))
        {
            dir.x = -1;
            animator.SetInteger("Direction", 3);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            dir.x = 1;
            animator.SetInteger("Direction", 2);
        }

        if (Input.GetKey(KeyCode.W))
        {
            dir.y = 1;
            animator.SetInteger("Direction", 1);
        }
        else if (Input.GetKey(KeyCode.S))
        {
            dir.y = -1;
            animator.SetInteger("Direction", 0);
        }

        dir.Normalize();
        animator.SetBool("IsMoving", dir.magnitude > 0);

        GetComponent<Rigidbody2D>().velocity = speed * dir;
    }

    void OnCollisionEnter2D(Collision2D collision){

        if (collision.gameObject.tag == "EndStatue") {
            Debug.Log("game ended");
            GameManager.instance.Victory();
        }
    }

    void OnTriggerExit2D(Collider2D other){
        if (other.gameObject.tag == "StartPlatform"){
            gameStarted = true;
            GameManager.instance.StartCountdown();
        }
    }

    private void Death(){
        GetComponent<SpriteRenderer>().enabled = false;
        gravestone.SetActive(true);
    }
}
