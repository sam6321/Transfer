using UnityEngine;

public class GateTile : MonoBehaviour
{
    private Animator animator;
    private TileBlocker blocker;

    void Start()
    {
        animator = GetComponent<Animator>();
        blocker = GetComponent<TileBlocker>();
    }

    public void SetGate(bool down)
    {
        animator.SetBool("down", down);
        blocker.Block = !down;
    }
}
