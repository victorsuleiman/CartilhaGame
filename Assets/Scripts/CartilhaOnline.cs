using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//class intention: to be both a network AND match managers
public class CartilhaOnline : NetworkManager
{
    //Connect 2 players, players can see their positions IRT - done!
    //Dynamic naming of players - done!
    //Have a button to begin match. Log that the match has begun - done!
    //When the match has begun, deal cards to the player and sync them (cards need to be the same between clients) - done!
    //Make the player see their cards only (and the board)

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


}
