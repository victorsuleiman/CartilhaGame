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

    //need to test cards appearing to the players now. distributeCards

    [SerializeField] public GameObject buttonGO;
    Button button;

    [SyncVar]
    public List<List<string>> playerHands;

    public GameObject[] players;
    
    public GameObject cardPrefab;

    private bool matchStarted = false;

    public Sprite[] cardFaces;



    // Start is called before the first frame update
    void Start()
    {
        button = buttonGO.GetComponent<Button>();
        button.onClick.AddListener(RpcMatchStart);
        playerHands = new List<List<string>>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    #region Server

    [Server]

    #endregion

    #region Client
    [ClientRpc]
    void RpcMatchStart()
    {
        Debug.Log("Match has started");
        matchStarted = true;
        Destroy(buttonGO);

        players = GameObject.FindGameObjectsWithTag("Player");
        
        CardDealer.dealCards(players, 2);
        CardDealer.distributeCards(players, playerHands, cardPrefab);
    }

    private void handlePlayerHandsUpdated()
    {
        //instantiate the cards into the player hands using the synced var playerHands. this will be a hook
    }

    #endregion
}
