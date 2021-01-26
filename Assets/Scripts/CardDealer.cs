using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class CardDealer : NetworkBehaviour
{
    [Server]
    public static void dealCards(Object[] playerList, int numberOfCards)
    {
        List<string> deck = Cartilha.generateDeck();
        int numberOfCardsStillInTheDeck = deck.Count;
        System.Random random = new System.Random();
        MatchManager matchManager = FindObjectOfType<MatchManager>();
        GameObject cardPrefab = matchManager.cardPrefab;

        foreach (GameObject p in playerList)
        {

            float xOffset = 0;
            //float yOffset = 0;
            float zOffset = 0.03f;

            for (int i = 0; i < numberOfCards; i++)
            {
                int k = random.Next(numberOfCardsStillInTheDeck);


                GameObject newCard = Instantiate(cardPrefab,
                    new Vector3(p.transform.position.x + xOffset, p.transform.position.y, p.transform.position.z + zOffset),
                    Quaternion.identity, p.transform);
                newCard.name = deck[k];
                xOffset += 0.5f;
                zOffset += 0.03f;
                deck.RemoveAt(k);
                numberOfCardsStillInTheDeck--;

            }

        }
    }

}
