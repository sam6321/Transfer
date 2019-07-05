using UnityEngine;

public class TileBlocker : MonoBehaviour
{
    public enum BlockType
    {
        MoveOn, // Can move onto the blocker's tile
        StopInfront, // Cannot move onto the blocker's tile
        None // Do not block
    }

    public enum TileBlockingType
    {
        Direction, // Block movement in a specific direction
        Tile // Block all movement on to this tile.
    }

    [SerializeField]
    [Tooltip("If true, robots will be blocked by this blocker. Otherwise")]
    private bool block = true;
    public bool Block
    {
        get => block;
        set
        {
            if(value != block)
            {
                block = value;
                Animate();
            }
        }
    }

    [SerializeField]
    [Tooltip("The way this tile will block movement")]
    private TileBlockingType blockingType;
    public TileBlockingType BlockingType { get => blockingType; set => blockingType = value; }

    [SerializeField]
    [Tooltip("The local direction to block movement in")]
    private Vector3 blockDirection = Vector3.left;
    public Vector3 BlockDirection { get => blockDirection; set => blockDirection = value; }

    private Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
        Animate();
    }

    private void Animate()
    {
        if (animator)
        {
            animator.SetBool("block", block);
        }
    }

    public BlockType CheckBlock(Vector3 from, Vector3 to)
    {
        if(!Block)
        {
            // Blocking disabled
            return BlockType.None;
        }

        switch(blockingType)
        {
            case TileBlockingType.Direction:
                // Only block movement across a side of the tile
                return CheckBlockDirection(from, to);

            case TileBlockingType.Tile:
            default:
                // Stop infront from all directions
                return BlockType.StopInfront;
        }
    }

    private BlockType CheckBlockDirection(Vector3 from, Vector3 to)
    {
        // 2 blocking cases to handle
        // - Source is from a tile in the reverse of the block direction, and from source to this tile points in the block direction
        // - Source is from a tile which is in block direction, and movement is against the block direction

        Vector3 movement = (to - from).normalized;
        Vector3 toTile = (transform.position - from).normalized;
        Vector3 worldBlockDirection = transform.TransformVector(BlockDirection).normalized;
        float blockDirectionDot = Vector3.Dot(movement, worldBlockDirection);
        float toTileDot = Vector3.Dot(toTile, worldBlockDirection);

        if (blockDirectionDot > 0.95 && (toTileDot > 0.95 || TileManager.IsSameTile(from, transform.position)))
        {
            // Moving onto the tile, in the same direction as the block direction.
            return TileManager.IsSameTile(from, transform.position) ? BlockType.StopInfront : BlockType.MoveOn;
        }
        else if (blockDirectionDot < -0.95 && (toTileDot < -0.95 || TileManager.IsSameTile(from, transform.position)))
        {
            // Trying to move on to the tile, against the block direction.
            return BlockType.StopInfront;
        }
        else
        {
            return BlockType.None;
        }
    }
}
