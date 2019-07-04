using UnityEngine;

public class TileBlocker : MonoBehaviour
{
    [SerializeField]
    [Tooltip("If true, robots will not be able to move onto this blocker")]
    private bool block = true;
    public bool Block { get => block; set => block = value; }
}
