using UnityEngine;
using System;
using System.Collections.Generic;

public class Robot : MonoBehaviour {

    [SerializeField] Rigidbody2D rb;
    [SerializeField] LayerMask ground;
    [SerializeField] LayerMask robotLayer;
    
    public bool isHacked;

    protected Texture Texture;
    protected string label;
    protected int health;
    protected int speed;
    protected int jump;
    protected float playerMultiplyer;

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
            rb.AddForce(new Vector2 (speed * playerMultiplyer, 0));
         }

         if (Input.GetKey(KeyCode.A)) { 
            Flip(false);
            rb.AddForce(new Vector2 (-speed * playerMultiplyer, 0));

         }

         if (Input.GetKey(KeyCode.W) && grounded) { 
             rb.AddForce(new Vector2 (0, jump * 10));
         }
        }
    }

    public void AIRoam() { 
        
        aiMovementStaller += Time.deltaTime;
        if (aiMovementStaller >= .05)
        {

            float size = 53.5f;
            float endpoint1 = 27 - (transform.localScale.x * 2.5f);
            float endpoint2 = -27 + (transform.localScale.x * 2.5f);
            float momentum = 1;
            bool shouldAIRoam = true;

            Debug.DrawRay(transform.position, Vector2.down * 3.5f, Color.blue, .05f);
            if (grounded)
            {
                GameObject Platform = (Physics2D.Raycast(transform.position, Vector2.down, 3.5f, ground).collider.gameObject);
                size = Platform.transform.localScale.x;
                endpoint1 = Platform.transform.localPosition.x + size;
                endpoint2 = Platform.transform.localPosition.x - size;
                shouldAIRoam = ShouldAIRoam(Platform);
            }

            momentum = Math.Abs((size - Math.Abs(transform.localPosition.x - ((endpoint1 + endpoint2) / 2))) / 3) + 1;
            if (momentum > 2) { momentum = 2; }

            if (shouldAIRoam)
            {
                Vector2 side = new Vector2();
                float rayXStart = 0;
                if (isLeft)
                {
                    side = Vector2.left;
                    rayXStart = -1;
                }
                else
                {
                    side = Vector2.right;
                    rayXStart = 1;
                }

                rayXStart = transform.position.x + (rayXStart * 2.2f * transform.localScale.x);
                Debug.DrawRay(new Vector2(rayXStart, transform.position.y), side * 2, Color.yellow, .05f);
                RaycastHit2D hitScan = Physics2D.Raycast(new Vector2(rayXStart, transform.position.y), side, 2, robotLayer);
                if (hitScan.collider != null && hitScan.collider.gameObject != Main.Hacked) Flip(isLeft);

                if (rb.transform.position.x >= endpoint1) Flip(false);
                else if (rb.transform.position.x <= endpoint2) Flip(true);
                if (rb.transform.position.x < endpoint1 && !isLeft) { rb.transform.position = new Vector2(rb.transform.position.x + (.01f * speed * momentum), rb.transform.position.y); }
                else if (rb.transform.position.x > endpoint2 && isLeft) { rb.transform.position = new Vector2(rb.transform.position.x - (.01f * speed * momentum), rb.transform.position.y); }
                aiMovementStaller = 0;

            }
        }
    }

    public bool ShouldAIRoam(GameObject Platform)
    {
        int spaceFilled = 0;
        for (int i; i < Platform.transform.localScale.x; )
            Debug.DrawRay(transform.position + i, Vector2.down * 3.5f, Color.green, .05f);
        return true
    }


    private void Flip (bool curr) {
         if (isLeft && curr) {
            rb.transform.Rotate(new Vector3(0, 180, 0));
            isLeft = false; 
         }
         else if (!isLeft && !curr) {
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
