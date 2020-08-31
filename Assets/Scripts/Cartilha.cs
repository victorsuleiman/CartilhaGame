using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//THIS GAME IS BEING MADE WITH LOTS OF HELP FROM THIS SOLITAIRE UNITY TUTORIAL BY MEGALOMOBILE: https://www.youtube.com/watch?v=1Cmb181-quI

public class Cartilha : MonoBehaviour
{
    //Deal 5 cards to the player and to the CPUs -> done!
    //now do the sprite renderer and try to make the cards the child of each player -> done!
    //make the cards on the CPUs face down -> done!
    //make the card go to the board when you click it -> done!
    //make the board detect its children so it can detect who won the round. I'll need to make the card inform the board to update it maybe. -> done!
    //make it turn-based. make the CPUs play after the player, and then return who won the round -> done!
    //tidy up the coroutine for the CPU1. it's playing just as I play it. -> done!
    //make the player only click HIS cards and only on HIS turn. -> done!
    //make a coRoutine for deal -> done!
    //make the "guesses" game logic. prompt the player how many rounds he thinks he will win before starting the round. -> done!
    //when someone plays and wins, take out the pokerChip from its hand -> done!
    //sort the player's hand after dealing -> done!
    //make more than one round, try to repeat the game logic. Also, make the MATCH scoreboard, update it each end of the round.


    public static string[] suits = { "D", "S", "H", "C" };
    public static string[] values = {"4", "5", "6", "7", "Q", "J", "K", "A", "2", "3"};

    GameObject player;
    GameObject CPU1;
    GameObject CPU2;
    GameObject CPU3;
    GameObject boardGameObject;
    GameObject buttonGameObject;
    GameObject inputFieldGameObject;
    GameObject guessTextGameObject;
    public GameObject canvasPrefab;
    Button button;
    InputField inputField;
    

    UserInput userInput;

    //player list
    public List<GameObject> playerList;

    //deck is a list of type string
    public List<string> deck;

    //array of 52 sprites for each card, allocated inside engine
    public Sprite[] cardFaces;

    //used to instantiate each card on deck
    public GameObject cardPrefab;

    //changed approach: create a board game logic inside the main script
    public List<string> board;
    public List<string> playerTurnLog;

    //bool list to indicate which turn is active
    public List<bool> activeTurn;
    public bool justPlayed = false;

    //now that I can identify who won the round I might as well put a scoreboard.
    public List<int> scoreboard = new List<int>();

    //poker chip prefab for the guesses
    public GameObject pokerChip;

    //guess list
    public List<int> guessList;

    // Start is called before the first frame update
    void Start()
    {

        player = GameObject.Find("Player");
        CPU1 = GameObject.Find("CPU1");
        CPU2 = GameObject.Find("CPU2");
        CPU3 = GameObject.Find("CPU3");
        boardGameObject = GameObject.Find("Board");
        

        userInput = FindObjectOfType<UserInput>();

        //add players to playerList and populate the activeTurn array. Player starts first
        playerList.Add(player);
        activeTurn.Add(false);
        playerList.Add(CPU1);
        activeTurn.Add(false);
        playerList.Add(CPU2);
        activeTurn.Add(false);
        playerList.Add(CPU3);
        activeTurn.Add(false);

        //populate the scoreboard
        foreach (GameObject player in playerList)
        {
            scoreboard.Add(0);
        }


        StartCoroutine(startRound(playerList));


        

        //prompt guesses before allowing player to start round.

        print("it is now Player's turn");
    }   

    // Update is called once per frame
    void Update()
    {
        //determining who won: for testing purposes, whenever I add a card to the board I want to see if it can determine who won properly.
        //obviously will change this in the future
        if (justPlayed)
        {
            justPlayed = false;
            controlTurns();
        }

        //I need to check when turns are over. maybe a bool of "just played the card" will help me. Then inside the controlTurns function I turn it off

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
    IEnumerator startRound(List<GameObject> playerList)
    {
        yield return new WaitForSeconds(0.5f);
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
                    yield return new WaitForSeconds(0.1f);
                }

                else
                {
                    GameObject newCard = Instantiate(cardPrefab,
                        new Vector3(p.transform.position.x + xOffset, p.transform.position.y, p.transform.position.z + zOffset),
                        Quaternion.identity, p.transform);
                    newCard.name = deck[k];
                    xOffset += 0.5f;
                    yield return new WaitForSeconds(0.1f);
                }

                zOffset += 0.03f;
                deck.RemoveAt(k);
                numberOfCardsStillInTheDeck--;
                
            }

            verticalOffset = !verticalOffset;
        }

        sortPlayerHand();

        //After dealing, instantiante canvas for guesses
        GameObject canvas = Instantiate(canvasPrefab, transform.position, Quaternion.identity);
        canvas.name = "Canvas";
        buttonGameObject = GameObject.Find("Button");
        inputFieldGameObject = GameObject.Find("InputField");
        guessTextGameObject = GameObject.Find("Text");

        inputField = inputFieldGameObject.GetComponent<InputField>();
        button = buttonGameObject.GetComponent<Button>();
        button.onClick.AddListener(guesses);
    }

    //determining who won the round and updating its score on the scoreboard
    string whoWonRound()
    {
        //it needs a list of values just to compare them. I got a list of card values deck
        //starting from weakest to strongest (not counting the manilha). Maybe I can use it.
        List<string> deck = generateDeck();

        //I need to create a list of values, see who's the maximum one, see its position, compare to the playerTurnLog to check who won.
        List<int> values = new List<int>();

        string whoWon;
        int whoWonIndex = 0;

        //Grab the position of the card (its value) in deck
        foreach (string card in board)
        {
            foreach (string cardName in deck)
            {
                if (card == cardName) 
                {
                    values.Add(deck.IndexOf(cardName));
                    break;
                }
            }
        }

        //see what's the maximum value in the list of values
        int maxValue = 0;
        foreach (int value in values)
        {
            if (value > maxValue)
            {
                whoWonIndex = values.IndexOf(value);
                maxValue = value;                
            }
        }

        //now we see the position of the card the has won. then we just grab the name of the player from playerTurnLog.
        whoWon = playerTurnLog[whoWonIndex];

        //updating the scoreboard
        scoreboard[whoWonIndex]++;

        //taking out a poker chip from whoever won the round
        GameObject playerWhoWon = GameObject.Find(whoWon);
        try
        {        
        GameObject pokerChip = playerWhoWon.transform.Find("Poker Chip").gameObject;
        Destroy(pokerChip);
        }
        catch (NullReferenceException) { print("no poker chips left"); }
        return whoWon;

    }

    //controlling the turns and when each player plays
    void controlTurns()
    {
        //order: clockwise. So player goes then CPU1, CPU2, CPU3.
        //maybe I make a bool array of which player is currently active to play. Turns change once a new card is instantiated in the board
        //In the beggining, player starts. After player plays, set CPU1 to play. If all players played, set player active again.
        int lastPlayerIndex = activeTurn.IndexOf(true);

        if (lastPlayerIndex != activeTurn.Count - 1)
        {
            activeTurn[lastPlayerIndex] = false;
            activeTurn[lastPlayerIndex + 1] = true;
            print("It is now " + playerList[lastPlayerIndex + 1].name + "'s turn.");
            StartCoroutine(cpuPlays(playerList[lastPlayerIndex + 1]));
        } 
        

        //this else represents the end of the turn. So I also have to destroy all the cards on the board to initiate another turn
        else 
        {
            activeTurn[lastPlayerIndex] = false;
            StartCoroutine(resetBoard());
            activeTurn[0] = true;
            print(whoWonRound() + " wins the round!");
            playerTurnLog.Clear();
            print("It is now Player's turn.");
        }
        
    }
    
    IEnumerator resetBoard()
    {
        yield return new WaitForSeconds(3);
        //destroy all the cards in the board
        foreach (Transform card in boardGameObject.transform)
        {
            Destroy(card.gameObject);
        }

        //reset the board offset position and the variable board itself
        userInput.boardXOffset = 0;
        board.Clear();
    }

    //making the CPU play and how it will do it
    IEnumerator cpuPlays(GameObject CPU)
    {
        yield return new WaitForSeconds(2);

        List<string> deck = generateDeck();

        List<string> cardsInHand = new List<string>();
        List<int> valuesInHand = new List<int>();

        foreach(Transform cardTransform in CPU.transform)
        {
            cardsInHand.Add(cardTransform.gameObject.name);
        }

        

        //Grab the position of the card (its value) in deck
        foreach (string card in cardsInHand)
        {
            foreach (string cardName in deck)
            {
                if (card == cardName)
                {
                    valuesInHand.Add(deck.IndexOf(cardName));
                    break;
                }
            }
        }

        //now that the CPU knows the values of the card, for simplifying purposes, it will always choose the maximum value for now.
        //I can increment the logic after haha
        int maxValueIndex = 0;
        int maxValue = 0;

        foreach (int value in valuesInHand)
        {
            if (maxValue < value)
            {
                maxValueIndex = valuesInHand.IndexOf(value);
                maxValue = value;
            }
        }

        //Now the CPU plays the card with the maximum value. Destroy it from the hand and instantiate it onto the board
        Destroy(CPU.transform.GetChild(maxValueIndex).gameObject);

        GameObject newCard = Instantiate(cardPrefab,
                        new Vector3(boardGameObject.transform.position.x + userInput.boardXOffset, boardGameObject.transform.position.y, 
                            boardGameObject.transform.position.z),
                        Quaternion.identity, boardGameObject.transform);

        newCard.name = cardsInHand[maxValueIndex];
        userInput.boardXOffset += 1.5f;

        //adding the value to the board list, and who played the card
        board.Add(newCard.name);
        playerTurnLog.Add(CPU.transform.name);

        justPlayed = true;

        
    }

    void guesses()
    {
        GameObject canvas = GameObject.Find("Canvas");

        //grabbing guesses from the player and converting them into int
        string text = inputField.text.ToString();
        int playerGuess = Int16.Parse(text);

        guessList = new List<int>();

        //for now, we grab the number of guesses of the player and make CPU guess 2 wins each. I'll work on a simple AI after.
        guessList.Add(playerGuess);
        guessList.Add(2);
        guessList.Add(2);
        guessList.Add(2);

        activeTurn[0] = true;

        Destroy(canvas);

        dealPokerChips();
    }

    void dealPokerChips()
    {

        //for each player, the game instantiates the poker chips according to the number of guesses
        //I can use playerList for this. gonna need to understand the positions though D=
        bool verticalOffset = false;

        foreach (GameObject player in playerList)
        {
            float xOffset;
            float yOffset;

            if (verticalOffset)
            {
                xOffset = -0.5f;
                yOffset = -1.3f;
                for (int i = 0; i < guessList[playerList.IndexOf(player)]; i++)
                {
                    GameObject newPokerChip = Instantiate(pokerChip,
                    new Vector3(player.transform.position.x + xOffset, player.transform.position.y + yOffset, player.transform.position.z),
                    Quaternion.identity, player.transform);
                    newPokerChip.name = "Poker Chip";
                    xOffset += 0.5f;
                }
            }
            else
            {
                xOffset = -1f;
                yOffset = 1f;
                for (int i = 0; i < guessList[playerList.IndexOf(player)]; i++)
                {
                    GameObject newPokerChip = Instantiate(pokerChip,
                    new Vector3(player.transform.position.x + xOffset, player.transform.position.y + yOffset, player.transform.position.z),
                    Quaternion.identity, player.transform);
                    newPokerChip.name = "Poker Chip";
                    yOffset -= 0.5f;
                }
            }

            verticalOffset = !verticalOffset;
        }
    }

    void sortPlayerHand()
    {
        //detect list of cards the player have
        List<string> cards = new List<string>();

        List<string> deck = generateDeck();
        
        foreach (Transform child in player.transform)
        {
            if (child.CompareTag("Card"))
                cards.Add(child.name);
        }

        

        //grab their values
        List<int> cardValues = new List<int>();

        foreach (string card in cards)
        {
            foreach (string cardName in deck)
            {
                if (card == cardName)
                {
                    cardValues.Add(deck.IndexOf(cardName));
                    break;
                }
            }
        }

        //sort it
        sort(cardValues, cards);

        //destroy player's hand hehe
        foreach (Transform child in player.transform) { Destroy(child.gameObject); }

        //instantiate sorted hand
        float xOffset = 0;
        float zOffset = 0.03f;
        foreach (string card in cards)
        {          
            GameObject newCard = Instantiate(cardPrefab,
                        new Vector3(player.transform.position.x + xOffset, player.transform.position.y, player.transform.position.z + zOffset),
                        Quaternion.identity, player.transform);
            newCard.name = card;
            xOffset += 0.5f;
            zOffset += 0.03f;
        }


    }

    static void sort(List<int> arr, List<string>arr2)
    {
        int n = arr.Count;

        // One by one move boundary of unsorted subarray 
        for (int i = 0; i < n - 1; i++)
        {
            // Find the minimum element in unsorted array 
            int min_idx = i;
            for (int j = i + 1; j < n; j++)
                if (arr[j] < arr[min_idx])
                    min_idx = j;

            // Swap the found minimum element with the first 
            // element 
            int temp = arr[min_idx];
            string temp2 = arr2[min_idx];
            arr[min_idx] = arr[i];
            arr2[min_idx] = arr2[i];
            arr[i] = temp;
            arr2[i] = temp2;
        }
    }
}
