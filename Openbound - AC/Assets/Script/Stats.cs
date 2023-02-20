using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class Stats : ScriptableObject
{

    public Sprite characterIcon;
    [Space(10)]
    public string fullName;
    public string chatHandle;
    public string sign;
    public string species;
    public string caste;
    public string title;
    [Space(10)]
    public Sprite signSprite;
    [Space(10)]
    public string age = "6 Sweeps";
    [Space(10)]
    public float powerLevel;
    public float intelligence;
    public float wisdom;
    public float vitality;
    public float speed;
    public float precision;
    public float charisma;
    public float spirit;
    [Space(10)]
    public Strifedeck[] strifedecks;
    [Space(10)]
    public RuntimeAnimatorController runtimeAnimatorController;
    [Space(10)]
    public CharacterProfile characterProfile;
}