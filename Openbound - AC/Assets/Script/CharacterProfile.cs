using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;

[CreateAssetMenu]
public class CharacterProfile : ScriptableObject
{
    public string characterID;

    public string characterName;

    public AnimatorController animator;

    public Color32 primaryColor;
    public Color32 secondaryColor;
    public Color32 fontColor;

    public DialogueAudioInfoSO dialogueASO;
}
