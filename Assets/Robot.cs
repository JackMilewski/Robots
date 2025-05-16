using UnityEngine;
using System;
using System.Collections.Generic;

public class Robot : MonoBehaviour {

    public float endpoint1;
    public float endpoint2;
    
    [SerializeField] Rigidbody2D rb;
    [SerializeField] LayerMask ground;
    [SerializeField] LayerMask robotLayer;
    
    public bool isHacked;

    protected Texture Texture;
    protected string label;
    protected int health;
    protected int speed;
    protected int jump;
    protected int weight;

    private double aiMovementStaller;
    public bool grounded;
    private bool isLeft;


    void Awake () {
        Main.allRobots.Add(this);
    }
    void Update () {
        transform.eulerAngles = new Vector3 (0, transform.eulerAngles.y, 0);
    }

    private void OnMouseDown()
    {
        isHacked = !isHacked;
        foreach (Robot bot in Main.allRobots)
            if(bot != this) { bot.isHacked = false; }
    }

    public void basePlayerControl() {
    {
         if (Input.GetKey(KeyCode.D)) { 
            Flip(true);
            rb.AddForce(new Vector2 (speed, 0));
         }

         if (Input.GetKey(KeyCode.A)) { 
            Flip(false);
            rb.AddForce(new Vector2 (-speed, 0));

         }

         if (Input.GetKey(KeyCode.W) && grounded) { 
             rb.AddForce(new Vector2 (0, jump * 10));
         }
        }
    }

    public void baseAIControl() { 
        
        aiMovementStaller += Time.deltaTime;
        if (aiMovementStaller >= .05) {

            endpoint1 = 27 - (transform.localScale.x * 2.5f);
            endpoint2 = -27 + (transform.localScale.x * 2.5f);
            float momentum = 1;

            Debug.DrawRay(transform.position, Vector2.down * 3, Color.blue, .05f);
           if (grounded){ 
                GameObject Platform = (Physics2D.Raycast(transform.position, Vector2.down, 3, ground).collider.gameObject);
                endpoint1 = Platform.transform.localPosition.x + (Platform.transform.localScale.x);
                endpoint2 = Platform.transform.localPosition.x - (Platform.transform.localScale.x);
                momentum = ((Platform.transform.localScale.x - Math.Abs(transform.localPosition.x - ((endpoint1 + endpoint2) / 2))) / 3) + 1;
                if (momentum > 2){ momentum = 2; };
            }

           Vector2 side = new Vector2();
           float rayXStart = 0;
           if (isLeft) { 
                side = Vector2.left; 
                rayXStart = -1;
           } else { 
               side = Vector2.right;
               rayXStart = 1;
           }

           rayXStart = transform.position.x + (rayXStart * 2.2f * transform.localScale.x);
           Debug.DrawRay(new Vector2(rayXStart, transform.position.y), side * 3, Color.yellow, .05f);
           RaycastHit2D hitScan = Physics2D.Raycast(new Vector2(rayXStart, transform.position.y), side, 3, robotLayer);
           if(hitScan.collider != null && hitScan.collider.gameObject != Main.Hacked) Flip(isLeft);

           if (rb.transform.position.x >= endpoint1) Flip(false);
           else if (rb.transform.position.x <= endpoint2) Flip(true);
           if (rb.transform.position.x < endpoint1 && !isLeft) { rb.transform.position = new Vector2 (rb.transform.position.x + (.01f * speed * momentum), rb.transform.position.y); }
           else if (rb.transform.position.x > endpoint2 && isLeft) { rb.transform.position = new Vector2 (rb.transform.position.x - (.01f * speed * momentum), rb.transform.position.y); } 
           aiMovementStaller = 0;
            
        }
    }

    private void Flip (bool side) {
         if (isLeft && side) {
            rb.transform.Rotate(new Vector3(0, 180, 0));
            isLeft = false; 
         }
         else if (!isLeft && !side) {
            rb.transform.Rotate(new Vector3(0, 180, 0));
            isLeft = true; 
         }
    }

    void OnCollisionEnter2D(Collision2D collision) {
        if (((1 << collision.gameObject.layer) & ground) == 0) return;
        grounded = true;
    }
    void OnCollisionExit2D(Collision2D collision) {
        if (((1 << collision.gameObject.layer) & ground) == 0) return;
        grounded = false;
    }

}
