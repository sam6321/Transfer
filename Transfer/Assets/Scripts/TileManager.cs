using System.Collections.Generic;
using UnityEngine;

public class TileManager : MonoBehaviour
{
    private Dictionary<Vector3Int, GameObject> tileDict = new Dictionary<Vector3Int, GameObject>();

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
            Transform child = transform.GetChild(i);
            if(child.CompareTag("Tile"))
            {
                tileDict.Add(ToVector3Int(child.position), child.gameObject);
            }
        }
    }

    public bool TileExists(Vector3 position)
    {
        return tileDict.ContainsKey(ToVector3Int(position));
    }

    public T GetTileComponent<T>(Vector3 position) where T : MonoBehaviour
    {
        GameObject tile = GetTile(position);
        return tile ? tile.GetComponent<T>() : null;
    }

    public GameObject GetTile(Vector3 position)
    {
        if (tileDict.TryGetValue(ToVector3Int(position), out GameObject value))
        {
            return value;
        }
        else
        {
            return null;
        }
    }
}
