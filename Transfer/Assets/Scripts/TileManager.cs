using System.Collections.Generic;
using UnityEngine;

public class TileManager : MonoBehaviour
{
    private Dictionary<Vector3Int, Tile> tileDict = new Dictionary<Vector3Int, Tile>();

    Vector3Int ToVector3Int(Vector3 v)
    {
        return new Vector3Int
        (
            Mathf.RoundToInt(v.x),
            Mathf.RoundToInt(v.y),
            Mathf.RoundToInt(v.z)
        );
    }

    // Start is called before the first frame update
    void Start()
    {
        for(int i = 0; i < transform.childCount; i++)
        {
            Tile tile = transform.GetChild(i).GetComponent<Tile>();
            if(tile)
            {
                tileDict.Add(ToVector3Int(tile.transform.position), tile);
            }
        }
    }

    public bool TileExists(Vector3 position)
    {
        return tileDict.ContainsKey(ToVector3Int(position));
    }

    public Tile GetTile(Vector3 position)
    {
        if (tileDict.TryGetValue(ToVector3Int(position), out Tile value))
        {
            return value;
        }
        else
        {
            return null;
        }
    }
}
