using UnityEngine;

public class Blaster : Robot 
{
    [SerializeField] Texture2D defualtTexture;
    [SerializeField] Texture2D hackedTexture;

    void Start()
    {
        label = "Blaster";
        health = 75;
        speed = 12;
        jump = 300;
        playerMultiplyer = 10f;
    }


    void FixedUpdate()
    {
        if (isHacked) {
            Main.Hacked = this;
            GetComponent<SpriteRenderer>().sprite = Sprite.Create( hackedTexture, new Rect(0, 0, hackedTexture.width, hackedTexture.height), new Vector2(0.5f, 0.5f));
            basePlayerControl();
        }
        else {
            GetComponent<SpriteRenderer>().sprite = Sprite.Create( defualtTexture, new Rect(0, 0, defualtTexture.width, defualtTexture.height), new Vector2(0.5f, 0.5f));
            AIRoam();
        }
     }   
        
} 
        








