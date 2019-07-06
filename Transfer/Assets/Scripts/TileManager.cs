using System.Collections.Generic;
using UnityEngine;

public class TileManager : MonoBehaviour
{
    private const int MoveBlocked = 2; // Cannot move onto the tile
    private const int MoveStopOn = 1; // Can move on to the tile
    private const int MoveNone = 0; // No interaction

    public static Vector3Int ToVector3Int(Vector3 v)
    {
        return new Vector3Int
        (
            Mathf.RoundToInt(v.x),
            Mathf.RoundToInt(v.y),
            Mathf.RoundToInt(v.z)
        );
    }

    public static bool IsSameTile(Vector3 v1, Vector3 v2)
    {
        return ToVector3Int(v1) == ToVector3Int(v2);
    }

    private Dictionary<Vector3Int, List<GameObject>> tileDict = 
        new Dictionary<Vector3Int, List<GameObject>>();

    // Start is called before the first frame update
    void Start()
    {
        for(int i = 0; i < transform.childCount; i++)
        {
            Transform child = transform.GetChild(i);
            if(child.CompareTag("Tile"))
            {
                LinkObject(child.gameObject);
            }
        }
    }

    public int RunMove(Vector3 position, Vector3 direction, int steps, float time)
    {
        if(!CheckBlockers(position, direction))
        {
            return 0; // Blocked from moving in that direction by a blocker on our current tile
        }

        // Accumulates movers for each tile so that they can be processed if all other
        // tiles allow us to move onto the next tile.
        List<TileMover> movers = new List<TileMover>();

        // Check all tiles along the way and move to the closest one we can move to.
        for (int i = 0; i < steps; i++)
        {
            Vector3 target = position + direction * (i + 1);
            Vector3 targetGround = target;
            targetGround.y -= 1;

            if (!TileExists(targetGround))
            {
                return i; // stop here because there's no ground infront of us
            }

            movers.Clear();

            int move = MoveNone;

            // Check all tiles at this position and interact with them accordingly
            foreach(GameObject tile in GetLinkBucket(ToVector3Int(target)))
            {
                // Check the move onto the tile, and choose the most restrictive one.
                move = Mathf.Max(RunMoveTile(tile, position, target, movers), move);
            }

            if(move < MoveBlocked)
            {
                // We're able to move onto the tile, but we might have a mover on the tile
                // so so lets see if we can push it out of the way
                foreach(TileMover mover in movers)
                {
                    // Try pushing the mover 1 space
                    if (mover.Move(direction, 1, time) > 0)
                    {
                        // We've successfully pushed this mover, so we can move onto its previous spot
                        move = Mathf.Max(move, MoveStopOn);
                    }
                    else
                    {
                        // Failed to push it, stop here
                        move = Mathf.Max(move, MoveBlocked);
                    }
                }
            }

            // Final result is that...
            switch (move)
            {
                case MoveBlocked:
                    // ... we're blocked and must stop here
                    return i;

                case MoveStopOn:
                    // ... we're not blocked, but we must stop here
                    return i + 1;

                case MoveNone:
                    // ... no interaction with anything at this tile
                    continue;
            }
        }

        return steps;
    }

    private int RunMoveTile(GameObject tile, Vector3 position, Vector3 target, List<TileMover> movers)
    {
        // Priorities ->
        // 1 - If there's a blocker that stops us before this tile, then stop there and don't move
        // onto the tile
        // 2 - If we're able to move onto the tile

        // there's something in our way, but we may be able to move onto / over it
        TileTrigger trigger = tile.GetComponent<TileTrigger>();
        if (trigger)
        {
            if (trigger.ResponseType == TileTrigger.TriggerResponseType.StopOn)
            {
                // This is a trigger we should stop on
                return MoveStopOn;
            }
            else
            {
                // Trigger does not stop movement
                return MoveNone;
            }
        }

        // Not a trigger, maybe it's a blocker?
        TileBlocker blocker = tile.GetComponent<TileBlocker>();
        if (blocker)
        {
            switch (blocker.CheckBlock(position, target))
            {
                case TileBlocker.BlockType.MoveOn:
                    // Move onto the blocker, but stop there
                    return MoveStopOn;

                case TileBlocker.BlockType.None:
                    return MoveNone;

                case TileBlocker.BlockType.StopInfront:
                    return MoveBlocked;
            }
        }

        // A tile mover can be moved by us, but we need to calculate the move appropriately
        TileMover mover = tile.GetComponent<TileMover>();
        if (mover)
        {
            switch (mover.MoveType)
            {
                case TileMover.TileMoveType.MoveThrough:
                    return MoveNone; // Can just move through the mover

                case TileMover.TileMoveType.Block:
                    return MoveBlocked; // Mover is set to not move, so we're blocked

                case TileMover.TileMoveType.Push:
                    // Process this later. If nothing else is blocking us, we might be able to push this mover.
                    movers.Add(mover);
                    return MoveNone;
            }
        }

        return MoveBlocked; // A wall
    }

    private bool CheckBlockers(Vector3 position, Vector3 direction)
    {
        // Check if the tile we're sitting on has any blockers pointing in the specified direction that
        // prevent us from moving that way

        List<GameObject> bucket = GetLinkBucket(ToVector3Int(position));
        foreach(GameObject tile in bucket)
        {
            TileBlocker blocker = tile.GetComponent<TileBlocker>();
            if (blocker && blocker.CheckBlock(position, position + direction) != TileBlocker.BlockType.None)
            {
                // We're sitting on a blocker that won't let us move in that direction.
                return false;
            }
        }

        return true;
    }

    private bool TileExists(Vector3 position)
    {
        List<GameObject> bucket = GetLinkBucket(ToVector3Int(position));
        return bucket.Count > 0;
    }

    public void UnlinkObject(GameObject linkObject)
    {
        TileLinkData linkData = linkObject.GetComponent<TileLinkData>();
        if(linkData && linkData.LinkKey.HasValue)
        {
            // This object is linked in somewhere, so go and get rid of it
            List<GameObject> bucket = GetLinkBucket(linkData.LinkKey.Value);
            bucket.Remove(linkObject);
            linkData.LinkKey = null;
        }
    }

    public void LinkObject(GameObject linkObject)
    {
        // Add / create link data on object when linking, if it doesn't have it
        TileLinkData linkData = linkObject.GetComponent<TileLinkData>();
        if(!linkData)
        {
            linkData = linkObject.AddComponent<TileLinkData>();
            linkData.LinkedManager = this;
        }

        if(linkData.LinkKey.HasValue)
        {
            // If we already have a link, unlink us.
            UnlinkObject(linkObject);
        }

        // No link, so we should link us in at the correct slot now
        Vector3Int key = ToVector3Int(linkObject.transform.position);
        linkData.LinkKey = key;
        List<GameObject> bucket = GetLinkBucket(key);
        bucket.Add(linkObject);
    }

    private List<GameObject> GetLinkBucket(Vector3Int key)
    {
        if(tileDict.TryGetValue(key, out List<GameObject> list))
        {
            // Already exists, nice
            return list;
        }
        else
        {
            // Doesn't exist, so create, add, then return
            List<GameObject> newList = new List<GameObject>();
            tileDict.Add(key, newList);
            return newList;
        }
    }
}
