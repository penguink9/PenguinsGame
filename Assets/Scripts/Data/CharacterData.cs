[System.Serializable]
public class CharacterData
{
    public int index;
    public int health;
    public bool isDead;
}
[System.Serializable]
public class MapStateEntry
{
    public int mapIndex;
    public MapState state;
}

[System.Serializable]
public class BoolEntry
{
    public int key;
    public bool value;
}
[System.Serializable]
public class ScoreRecord
{
    public int levelIndex;
    public int score;
}