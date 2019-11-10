using UnityEngine;
using UnityEditor;

class LabelHandle : Editor
{
    [DrawGizmo(GizmoType.InSelectionHierarchy | GizmoType.NotInSelectionHierarchy)]
    static void DrawGameObjectName(Transform transform, GizmoType gizmoType)
    {
        if (transform.gameObject.CompareTag("Planet"))
        {
            Handles.Label(transform.position + new Vector3(0, (float)(1.2 * transform.gameObject.GetComponent<SpriteRenderer>().bounds.size.y / 2), 0), transform.gameObject.name);
            var routes = transform.gameObject.GetComponent<PlanetScript>().OutgoingPlanets;

            foreach(var planet in routes)
            {
                Handles.DrawLine(transform.position, planet.transform.position);
            }
        }
    }
}