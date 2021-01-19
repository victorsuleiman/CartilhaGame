using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CartilhaOnline : NetworkManager
{

    //Connect 2 players, players can see their positions IRT - done!
    //For the host, have a button to begin match. Log that the match has begun
    //When the match has begun, deal cards to the player
    //Make the player see their cards only (and the board)

    public bool matchStarted = false;

    public override void OnServerAddPlayer(NetworkConnection conn)
    {
        base.OnServerAddPlayer(conn);

        Player player = conn.identity.GetComponent<Player>();

        player.setDisplayName($"Player {numPlayers}");

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void matchStart()
    {
        matchStarted = true;
    }
}
