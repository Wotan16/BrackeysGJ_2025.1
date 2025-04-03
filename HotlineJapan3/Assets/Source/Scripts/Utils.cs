using System.Collections.Generic;
using UnityEngine;

public class Utils : MonoBehaviour
{
    public static bool RaycastHitsTargetCollider(Vector2 startPosition, Vector2 direciton, float range,
        LayerMask layerMask, List<Collider2D> ignoredColliders, Collider2D targetCollider)
    {
        RaycastHit2D[] hits = Physics2D.RaycastAll(startPosition, direciton, range, layerMask);
        RaycastHit2D targetHit = new RaycastHit2D();

        foreach (RaycastHit2D hit in hits)
        {
            if (hit.collider == targetCollider)
            {
                targetHit = hit;
                break;
            }
        }
        if (targetHit.collider == null)
            return false;

        float distanceToTarget = Vector2.Distance(startPosition, targetHit.point);

        foreach (RaycastHit2D hit in hits)
        {
            if (hit.collider == targetHit.collider)
                continue;

            if (ignoredColliders.Contains(hit.collider))
                continue;

            if (hit.distance < distanceToTarget)
                return false;
        }
        return true;
    }

    public static bool HitsContainTargetCollider(RaycastHit2D[] hits, Collider targetCollider)
    {
        foreach (RaycastHit2D hit in hits)
        {
            if (hit.collider == targetCollider)
            {
                return true;
            }
        }
        return false;
    }
}
