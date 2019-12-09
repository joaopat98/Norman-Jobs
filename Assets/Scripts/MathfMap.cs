using UnityEngine;


public static class MathfMap
{
    public static float Map(float val, float start1, float end1, float start2, float end2)
    {
        return ((val - start1) / (end1 - start1)) * (end2 - start2) + start2;
    }
}