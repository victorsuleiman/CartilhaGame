using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdateSprite : MonoBehaviour
{
    Cartilha cartilha;
    public Sprite cardFace;
    public Sprite cardBack;
    SpriteRenderer spriteRenderer;

    // Start is called before the first frame update
    void Start()
    {
        cartilha = FindObjectOfType<Cartilha>();
        List<string> deck = Cartilha.generateDeck();

        //Identify which sprite is the card by the card's name
        int i = 0;
        foreach (string card in deck)
        {
            if (this.name == card)
            {
                cardFace = cartilha.cardFaces[i];
                break;
            }
            i++;
        }

        try
        {
            //if you're assigned to a player, your face is up. if not, it should remain down.
            if (transform.parent.name == "Player" || transform.parent.name == "Board")
            {
                spriteRenderer = GetComponent<SpriteRenderer>();
                spriteRenderer.sprite = cardFace;
            }
        } catch (NullReferenceException)
        {
            print("just for the error not to appear lol");
        }
        
        

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
