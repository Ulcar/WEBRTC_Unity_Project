using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
public class CharacterMovement : SerializedMonoBehaviour
{

    // Use this for initialization

    [SerializeField]
    BattleGrid grid;

    Tile currentTile;


    Character character;

    [SerializeField]
   public List<Tile> debugTiles = new List<Tile>();


    bool moved = false;

    [SerializeField]
    int movespeed = 3;

    void OnEnable()
    {
        MoveTo(grid.GetTile(0, 0));
        debugTiles = CheckMovement();
        foreach (Tile tile in debugTiles) 
        {
            tile.gameObject.SetActive(true);
        }
        

    }

    // Update is called once per frame
    void Update()
    {

    }

    public List<Tile> CheckMovement() 
    {
        // list of tiles moveable to
        List<Tile> vaildtiles = new List<Tile>();
        checkSides(currentTile, null, ref vaildtiles, 0);
        return vaildtiles;

    }



    public bool MoveTo(Tile tile) 
    {

        if (tile.characterOnTile == null) 
        {
            this.transform.position = tile.teleportPos;
            tile.characterOnTile = character;
            moved = true;
            this.currentTile = tile;
            return true;

        }

        return false;

    }


    void checkSides (Tile tile, Tile lastTile, ref List<Tile> vaildTiles, int depth) 
    {
        // could add hover and height checks etc, here

        if (depth <= movespeed && tile != null)
        {
            if (!vaildTiles.Contains(tile) && tile.characterOnTile == null)
            {
                vaildTiles.Add(tile);
            }
            Tile xTile = grid.GetTile((int)tile.TilePos.x + 1, (int)tile.TilePos.y);

                checkSides(xTile, tile, ref vaildTiles, depth + 1);
            xTile = grid.GetTile((int)tile.TilePos.x, (int)tile.TilePos.y + 1);

                checkSides(xTile, currentTile, ref vaildTiles, depth + 1);

            xTile = grid.GetTile((int)tile.TilePos.x, (int)tile.TilePos.y - 1);

                checkSides(xTile, currentTile, ref vaildTiles, depth + 1);

            xTile = grid.GetTile((int)tile.TilePos.x - 1, (int)tile.TilePos.y);

                checkSides(xTile, currentTile, ref vaildTiles, depth + 1);
        }


    }




}
