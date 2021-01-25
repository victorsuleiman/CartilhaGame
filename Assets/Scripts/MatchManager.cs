using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mirror;

public class MatchManager : NetworkBehaviour
{

    [SerializeField] public GameObject buttonGO;
    Button button;

    public bool matchStarted = false;

    // Start is called before the first frame update
    void Start()
    {
        button = buttonGO.GetComponent<Button>();
        button.onClick.AddListener(matchStart);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    [ClientRpc]
    void matchStart()
    {
        Debug.Log("Match has started");
        matchStarted = true;
        Destroy(buttonGO);
    }
}
