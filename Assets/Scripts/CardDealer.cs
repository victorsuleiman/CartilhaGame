using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class CardDealer : NetworkBehaviour
{
    //Cards already dealt on playerHands, make them appear to each player now
    //[Server]
    public static void distributeCards(GameObject[] playerList, List<List<string>> playerHands, GameObject cardPrefab)
    {
        for (int i = 0; i < playerList.Length; i++)
        {
            float xOffset = 0;
            //float yOffset = 0;
            float zOffset = 0.03f;

            for (int j = 0; j < playerHands[i].Count; j++)
            {
                GameObject newCard = Instantiate(cardPrefab,
                    new Vector3(playerList[i].transform.position.x + xOffset, playerList[i].transform.position.y, 
                        playerList[i].transform.position.z + zOffset),
                    Quaternion.identity, playerList[i].transform);
                newCard.name = playerHands[i][j];
                xOffset += 0.5f;
                zOffset += 0.03f;
            }
        }

        //foreach (GameObject p in playerList)
        //{

        //    float xOffset = 0;
        //    //float yOffset = 0;
        //    float zOffset = 0.03f;

        //    for (int i = 0; i < playerHands; i++)
        //    {
        //        int k = random.Next(numberOfCardsStillInTheDeck);


        //        GameObject newCard = Instantiate(cardPrefab,
        //            new Vector3(p.transform.position.x + xOffset, p.transform.position.y, p.transform.position.z + zOffset),
        //            Quaternion.identity, p.transform);
        //        newCard.name = deck[k];
        //        xOffset += 0.5f;
        //        zOffset += 0.03f;
        //        deck.RemoveAt(k);
        //        numberOfCardsStillInTheDeck--;

        //    }

        //}
    }

    //[Server]
    public static void dealCards(GameObject[] playerList, int numberOfCards)
    {
        List<string> deck = Cartilha.generateDeck();
        int numberOfCardsStillInTheDeck = deck.Count;
        System.Random random = new System.Random();
        MatchManager matchManager = FindObjectOfType<MatchManager>();

        foreach (GameObject p in playerList)
        {

            List<string> hands = new List<string>();

            for (int i = 0; i < numberOfCards; i++)
            {
                int k = random.Next(numberOfCardsStillInTheDeck);
                string newCard = deck[k];
                hands.Add(newCard);
                deck.RemoveAt(k);
                numberOfCardsStillInTheDeck--;
            }

            matchManager.playerHands.Add(hands);

        }

    }
}
