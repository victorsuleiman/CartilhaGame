using Mirror;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Player : NetworkBehaviour
{

    [SerializeField] private TMP_Text displayNameText = null;

    [SyncVar(hook = nameof(handleDisplayNameUpdated))]
    [SerializeField]
    private string displayName = "Player";

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void handleDisplayNameUpdated(string oldName, string newName)
    {
        displayNameText.text = newName;
    }

    [Command]
    private void cmdSetDisplayName(string newDisplayName)
    {
        //if (newDisplayName.Length < 2 || newDisplayName.Length > 20) { return; }

        //RpcLogNewName(newDisplayName);

        setDisplayName(newDisplayName);
    }

    [Server]
    public void setDisplayName(string newDisplayName)
    {
        displayName = newDisplayName;
    }

    public string getDisplayName()
    {
        return displayName;
    }
}
