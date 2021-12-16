using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ProBuilder;
using UnityEngine.UI;

public class BattleGrid : MonoBehaviour
{
    // Start is called before the first frame update



    
    
    
    [SerializeField]
    int x, y;


    [SerializeField]
    int xOffset = 1, yOffset = 1;


    [SerializeField]
    List<TileData> tileData;

    [SerializeField]
    Dictionary<Vector2, TileData> tileDict;



    [SerializeField]
    GameObject tilePrefab;
    [SerializeField]
    MeshFilter terrain;


    [SerializeField]
    List<List<Tile>> tiles = new List<List<Tile>>();

    [SerializeField]
    Vector3 tileOffset = new Vector3(1, 0, 1);

    void Start()
    {
        GenerateField(x, y);
    }

    // Update is called once per frame
    void Update()
    {
        
    }



    void GenerateField(int length, int height) 
    {
        float xpos = 0;
        float ypos = 0;
        
        Vector3 pos = terrain.transform.TransformPoint(terrain.mesh.vertices[terrain.mesh.vertexCount - 1]) + tileOffset;
        Debug.Log(pos);
        xpos = pos.x;
        ypos = pos.z;
        GameObject grid =   Instantiate(new GameObject("grid"));
        int linePosition = 0;
        
        for (int i = 0; i < length; i++) 
        {

            List<Tile> row = new List<Tile>();
            for (int j = 0; j < height; j++) 
            {
                GameObject tmp = Instantiate(tilePrefab);

                tmp.transform.position = new Vector3(xpos, 10f, ypos);
                // rotate to normal of mesh underneath
                Ray ray = new Ray(tmp.transform.position, -Vector2.up);
                RaycastHit hit;
                // add terrain layer filter later
                if (Physics.Raycast(ray, out hit)) 
                {
                    Vector3 normal = hit.normal;
                    tmp.transform.position = new Vector3(tmp.transform.position.x, hit.point.y + 0.5f, tmp.transform.position.z);
                    tmp.transform.rotation = Quaternion.LookRotation(tmp.transform.forward, hit.normal);
                }
                ypos += yOffset;



                Tile tile = tmp.AddComponent<Tile>();
                tile.relatedGameobj = tmp;
                row.Add(tile);
                tmp.transform.parent = grid.transform;
                tile.TilePos = new Vector2(i, j);
                tile.teleportPos = tmp.transform.position;
                tile.gameObject.SetActive(false);
                

            }
            ypos = pos.z;
            tiles.Add(row);
            xpos += xOffset;

        }
    }


    public Tile GetTile(int x, int y) 
    {
        if (x >= tiles.Count || x < 0) 
        {
            return null;
        }
        if (y >= tiles[x].Count || y < 0) 
        {
            return null;
        }
        return tiles[x][y];
    }
}
