using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DialogueEditor;

[CreateAssetMenu(fileName = "New NPC", menuName = "Game/NPC Data")]
public class NPCdata : ScriptableObject
{
    // Start is called before the first frame update\

    [Header("Thông tin của NPC")]

    public string npcName;
    public Sprite npcAvatar;
    public float voiceVolume=1.0f;

    [Header("Hội thoại")]
    public GameObject npcConversation;




}
