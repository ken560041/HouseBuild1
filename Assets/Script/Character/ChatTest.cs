using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DialogueEditor;
public class ChatTest : MonoBehaviour
{
    // Start is called before the first frame update
    public NPCConversation myConversation;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnMouseOver()
    {
        if(Input.GetMouseButtonDown(0))
        {
            ConversationManager.Instance.StartConversation(myConversation);
        }
    }

}
