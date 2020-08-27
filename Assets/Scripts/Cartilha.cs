using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cartilha : MonoBehaviour
{
    //Deal 5 cards to the player and to the CPUs -> done.
    //now do the sprite renderer and try to make the cards the child of each player -> done!
    //make the cards on the CPUs face down -> done!
    //make the card go to the board when you click it -> done!
    


    public static string[] suits = { "D", "S", "H", "C" };
    public static string[] values = {"4", "5", "6", "7", "Q", "J", "K", "A", "2", "3"};

    GameObject player;
    GameObject CPU1;
    GameObject CPU2;
    GameObject CPU3;

    public List<GameObject> playerList;

    //deck is a list of type string
    public List<string> deck;

    //array of 52 sprites for each card, allocated inside engine
    public Sprite[] cardFaces;

    //used to instantiate each card on deck
    public GameObject cardPrefab;

    //make the board detect its childs so it can detect who won the round. I'll need to make the card inform the board to update it maybe.
    public GameObject boardGameObject;
    public List<string> board;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
        CPU1 = GameObject.Find("CPU1");
        CPU2 = GameObject.Find("CPU2");
        CPU3 = GameObject.Find("CPU3");
        boardGameObject = GameObject.Find("Board");

        playerList.Add(player);
        playerList.Add(CPU1);
        playerList.Add(CPU2);
        playerList.Add(CPU3);

        deck = generateDeck();
        //shuffle(deck);

        deal(playerList);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public static List<string> generateDeck()
    {
        List<string> newDeck = new List<string>();

        //Matching each suit to each value and concatenating the names
        foreach (string v in values)
        {
            foreach (string s in suits)
            {
                newDeck.Add(s + v);
            }
        }

        return newDeck;
    }

    //logic to shuffle the deck
    void shuffle<T>(List<T> list)
    {
        System.Random random = new System.Random();
        int n = list.Count;
        while (n > 1)
        {
            int k = random.Next(n);
            n--;
            T temp = list[k];
            list[k] = list[n];
            list[n] = temp;
        }
    }

    //dealing the cards
    void deal(List<GameObject> playerList)
    {
        List<string> deck = generateDeck();
        int numberOfCardsStillInTheDeck = deck.Count;
        System.Random random = new System.Random();
        bool verticalOffset = false;
        

        foreach (GameObject p in playerList)
        {

            float xOffset = 0;
            float yOffset = 0;
            float zOffset = 0.03f;

            for (int i = 0; i < 5; i++)
            {
                int k = random.Next(numberOfCardsStillInTheDeck);

                if (verticalOffset)
                {
                   GameObject newCard = Instantiate(cardPrefab, 
                        new Vector3(p.transform.position.x, p.transform.position.y + yOffset, p.transform.position.z + zOffset), 
                        Quaternion.identity, p.transform);
                    newCard.name = deck[k];
                    yOffset += 0.7f;
                }

                else
                {
                    GameObject newCard = Instantiate(cardPrefab,
                        new Vector3(p.transform.position.x + xOffset, p.transform.position.y, p.transform.position.z + zOffset),
                        Quaternion.identity, p.transform);
                    newCard.name = deck[k];
                    xOffset += 0.5f;
                }

                zOffset += 0.03f;
                deck.RemoveAt(k);
                numberOfCardsStillInTheDeck--;
                
            }

            verticalOffset = !verticalOffset;
        }
    }
}
