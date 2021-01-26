using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mirror;

public class MatchManager : NetworkBehaviour
{
    //workaround for proper dealing: maybe use a syncvar for the player's hands
        //that only the server will run. Then when the var is synced, call an RPC to instantiate the cards on all clients
        //hook of the syncVar can be instantiate the card on the player's hand

    [SerializeField] public GameObject buttonGO;
    Button button;

    private List<string> deck;
    public GameObject[] players;
    public GameObject cardPrefab;

    private bool matchStarted = false;

    public Sprite[] cardFaces;

    // Start is called before the first frame update
    void Start()
    {
        deck = Cartilha.generateDeck();
        button = buttonGO.GetComponent<Button>();
        button.onClick.AddListener(RpcMatchStart);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    [ClientRpc]
    void RpcMatchStart()
    {
        Debug.Log("Match has started");
        matchStarted = true;
        Destroy(buttonGO);

        players = GameObject.FindGameObjectsWithTag("Player");

        //CardDealer.dealCards(players, 2);
    }
}
