using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class HelperFs
{
    public static string ScoreFormat(int score)
    {
        return string.Format("{0:0000}", score);
    }

    public static IEnumerator DoAfter(System.Action func,float time)
    {
        yield return new WaitForSeconds(time);
        func();
    }

    public static void GizmosDrawRect(Rect rect)
    {
        Gizmos.color = Color.yellow;

        // Left
        Gizmos.DrawLine(new Vector3(rect.x, rect.y, 0), new Vector3(rect.x, rect.y + rect.height, 0f));
        // Right
        Gizmos.DrawLine(new Vector3(rect.x + rect.width, rect.y, 0), new Vector3(rect.x + rect.width, rect.y + rect.height, 0f));
        // Top
        Gizmos.DrawLine(new Vector3(rect.x, rect.y, 0), new Vector3(rect.x + rect.width, rect.y, 0f));
        // Bot
        Gizmos.DrawLine(new Vector3(rect.x, rect.y + rect.height, 0f), new Vector3(rect.x + rect.width, rect.y + rect.height, 0f));
    }
}
