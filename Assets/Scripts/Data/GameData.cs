using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class GameData
{
    public int level;
    public int coinCollected;
    public int currentMap;
    public Vector2 playerPosition;
    public List<CharacterData> unlockCharacters;
    public int activeCharacterIndex;
    public List<MapStateEntry> mapStates;
    public List<BoolEntry> npcUnlockeds;
    public int healthPotions;
    public int keys;
}
