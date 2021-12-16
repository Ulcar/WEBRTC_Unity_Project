using UnityEngine;
using System.Collections;

public class PlayerTurnController : MonoBehaviour
{

    // Use this for initialization


    Camera camera;

    Tile chosenTile;

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
      Ray ray =   camera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
          chosenTile =   hit.collider.GetComponent<Tile>();

            if(chosenTile != null) 
            {
                // show tile info

                if (Input.GetMouseButton(0)) 
                {
                    // get possible actions
                }
            }
        }
    }
}
