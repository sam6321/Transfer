using UnityEngine;

public class TileLinkData : MonoBehaviour
{
    public Vector3Int? LinkKey { get; set; } = null;
    public TileManager LinkedManager { get; set; } = null;

    private void OnDestroy()
    {
        if(LinkedManager && LinkKey.HasValue)
        {
            LinkedManager.UnlinkObject(gameObject);
        }
    }
}
