using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NpcChatController : MonoBehaviour {

    public Text chatBox;
    private Transform PlayerTransform;
    public int npcId;

    private bool textDisplayed;
    private string txt;

    public string[] Text;
    
    private static bool textIsRunning { get; set; }

    private static Text ChatBox;

    void Start () {
        textIsRunning = false;
        PlayerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        //Ethan = GameObject.FindGameObjectWithTag("Player");
        if (ChatBox == null) ChatBox = chatBox;
        textDisplayed = false;
        if (npcId == 1)
            txt = "What do you want?";
        else if (npcId == 2)
            txt = "Get lost guy!";
        else
            txt = "I've been expecting you...";
	}
    
    void Update() {
        if (!textIsRunning) {
            if (Vector3.Distance(PlayerTransform.position, transform.position) < 3f) {
                StartCoroutine(RunText());
            }
        }
    }

    void OnCollisionEnter(Collision c) {
        if (c.gameObject.tag == "Player" && !textDisplayed)
            StartCoroutine(DisplayText());
    }

    IEnumerator DisplayText() {
        textDisplayed = true;
        ChatBox.text = txt;
        do {
            yield return new WaitForSeconds(3);
        } while (Vector3.Distance(transform.position, PlayerTransform.transform.position) <= 4f);
        textDisplayed = false;
        ChatBox.text = "";
    }

    IEnumerator RunText() {
        foreach (string t in Text) {
            ChatBox.text = t;
            yield return new WaitForSeconds(4);
        }
        ChatBox.text = "";
        textIsRunning = false;
            
        
    }

    // when gets close, display text
    // after 3s reset text, unless player is still close
}
