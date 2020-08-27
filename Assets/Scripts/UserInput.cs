using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserInput : MonoBehaviour
{

    Cartilha cartilha;

    float boardXOffset = 0;

    // Start is called before the first frame update
    void Start()
    {
        cartilha = FindObjectOfType<Cartilha>();
    }

    // Update is called once per frame
    void Update()
    {
        playCard();
    }

    private void playCard()
    {
        if (Input.GetMouseButtonDown(0))
        {
            //Grab the mouse position that clicked on something on the screen
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, -10));

            //Cast an omnidirectional ray to verify which collider the mouse clicked to we know WHAT the mouse clicked. Neat. 
            //Then the collider name is saved on it
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);

            //If a card is clicked, destroy it in the player's hand and instantiate it to the board.
            //Also make the board detect (or add the card to its value)
            if (hit.collider.CompareTag("Card"))
            {

                GameObject cardToBeDestroyed = GameObject.Find(hit.collider.name);

                //who played the card?
                string whoPlayed = cardToBeDestroyed.transform.parent.name;

                Destroy(cardToBeDestroyed);

                GameObject board = GameObject.Find("Board");

                GameObject newCard = Instantiate(cartilha.cardPrefab,
                        new Vector3(board.transform.position.x + boardXOffset, board.transform.position.y, board.transform.position.z),
                        Quaternion.identity, board.transform);               

                newCard.name = hit.collider.name;
                boardXOffset += 1.5f;

                //adding the value to the board list on main script, and who played the card
                cartilha.board.Add(newCard.name);
                cartilha.playerTurnLog.Add(whoPlayed);
            }
        }
    }
}
