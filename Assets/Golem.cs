using UnityEngine;

public class Golem : Robot 
{
    [SerializeField] Texture2D defualtTexture;
    [SerializeField] Texture2D hackedTexture;

    void Start()
    {
        label = "Golem";
        health = 300;
        speed = 10;
        jump = 15;
        weight = 90;
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
            baseAIControl();
        }
     }   
        
}



