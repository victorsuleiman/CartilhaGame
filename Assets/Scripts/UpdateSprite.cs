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
                //for multiplayer
                if (cartilha == null)
                {
                    MatchManager matchManager = FindObjectOfType<MatchManager>();
                    cardFace = matchManager.cardFaces[i];
                }
                else
                {
                    cardFace = cartilha.cardFaces[i];
                    break;
                }

                
            }
            i++;
        }


        //if you're assigned to a player or the board, your face is up. if not, it should remain down.
        if (transform.parent.name == "Player" || transform.parent.name == "Board")
        {
                spriteRenderer = GetComponent<SpriteRenderer>();
                spriteRenderer.sprite = cardFace;
        }

        //for multiplayer
        if (cartilha == null)
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
            spriteRenderer.sprite = cardFace;
            if (transform.parent.GetComponent<Player>().hasAuthority) spriteRenderer.sprite = cardFace;
            else spriteRenderer.sprite = cardBack;


        }

        
        

    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
