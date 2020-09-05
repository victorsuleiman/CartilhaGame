using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Test : MonoBehaviour
{
    List<string> list = new List<string> { "1", "2", "3" };
    GameObject canvas;
    GameObject guesses;
    Dropdown dropdown;

    // Start is called before the first frame update
    void Start()
    {
        canvas = GameObject.Find("Canvas");
        guesses = canvas.transform.Find("Guesses").gameObject;
        dropdown = guesses.GetComponent<Dropdown>();
        dropdown.ClearOptions();
        dropdown.AddOptions(list);


    }

    // Update is called once per frame
    void Update()
    {
        int choice = dropdown.value;
        print(choice);
    }

}
