using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DialogueEditor;
public class NPC : MonoBehaviour, IInteractable
{
    // Start is called before the first frame update
    public NPCdata npcData;
    private GameObject npcConversationInstance;

    public void Interact()
    {
        //Debug.Log($"Bắt đầu hội thoại với {npcData.npcName}");
        // DialogueManager.Instance.StartDialogue(npcData);
        if (npcData.npcConversation != null)
        {
            // Tạo một instance của NPCConversation trong runtime
            if (npcConversationInstance == null)
            {
                npcConversationInstance = Instantiate(npcData.npcConversation);
            }

            NPCConversation conversation = npcConversationInstance.GetComponent<NPCConversation>();
            ConversationManager.Instance.StartConversation(conversation);


        }
    }











    public string GetInteractText()
    {
        return npcData.npcName;
    }



}
