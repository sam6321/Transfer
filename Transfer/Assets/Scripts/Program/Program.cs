using System.Collections.Generic;
using System;
using UnityEngine;

[Serializable]
public class Program 
{
    [SerializeField]
    private ProgramAction[] actions;
    public IReadOnlyList<ProgramAction> Actions => actions;

    [SerializeField]
    private Color colour;
    public Color Colour => colour;
}
