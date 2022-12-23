using System;

namespace Games.InCircles.Scripts.Categories.Factories
{
    [Flags]
    public enum GameMode 
    {
        None = 0,
        WhatDifferent = 1 << 1,
        ContinueLine = 1 << 2,
        FindPair = 1 << 3,
        MakeRight = 1 << 4,
        SelectAll = -1
    }
}