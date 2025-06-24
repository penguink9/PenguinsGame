using System.Collections.Generic;

[System.Serializable]
public class MapState
{
    public List<EnemyState> enemies;
    public List<ItemState> items;

    public MapState(List<EnemyState> e, List<ItemState> i)
    {
        enemies = e;
        items = i;
    }
}

[System.Serializable]
public class EnemyState
{
    public string id;
    public int currentHP;
}

[System.Serializable]
public class ItemState
{
    public string id;
    public bool isDestroyed;
}
