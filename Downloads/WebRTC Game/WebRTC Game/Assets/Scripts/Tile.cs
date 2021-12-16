using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
// consider this, make it a monobehaviour or just a generic class?

[Serializable]
public class Tile:MonoBehaviour
{


    public GameObject relatedGameobj;
    public string name;
    public Character characterOnTile;
    [SerializeField]
    public List<TileEffects> effects = new List<TileEffects>();

    public Vector2 TilePos;
    public Vector3 teleportPos;

    BattleGrid grid;



}


public class TileEffects:ScriptableObject
{
    
}

public class TileData : ScriptableObject
{
    public Vector2 Pos { get { return pos; } private set { pos = Pos; } }

    public int height;


    // rest of data I guess, list of effects etc

    [SerializeField]
    private Vector2 pos;


}
