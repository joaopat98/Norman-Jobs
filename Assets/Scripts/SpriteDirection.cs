using UnityEngine;

public static class SpriteDirection
{
    public static Vector2Int ToSpriteDirection(this Vector2 v, float threshold)
    {
        Vector2Int dir_ceil = new Vector2Int();
        if (Mathf.Abs(v.x) > threshold)
            dir_ceil.x = v.x > 0 ? 1 : -1;
        else
            dir_ceil.x = 0;
        if (Mathf.Abs(v.y) > threshold)
            dir_ceil.y = v.y > 0 ? 1 : -1;
        else
            dir_ceil.y = 0;
        return dir_ceil;
    }
}