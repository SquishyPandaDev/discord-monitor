using UnityEngine;
using UnityEditor;

namespace DiscordMonitor {
  public class RegionZoneGizmo : MonoBehaviour {
    private const float HEIGHT       = 0.3f;
    private const float OFFSET       = 0.2f;
    private const float LABEL_OFFSET = 0.5f;

    private readonly static float Y_POSSITION = HEIGHT + OFFSET;

    [DrawGizmo(GizmoType.Selected | GizmoType.NonSelected)]
    private static void DrawRegionZone(
      GameObject gameObject,
      GizmoType type
    ) {
      // ensure game object is region
      var isRegion  = false;
      var transform = gameObject.transform;

      while(transform.parent != null) {
        isRegion = transform.parent.name == "Regions";

        if(isRegion) break;

        transform = transform.parent.transform;
      }

      if(!isRegion) return;

      var cubePossition = gameObject.transform.position
        + new Vector3(0f, Y_POSSITION, 0f);


      Gizmos.DrawWireCube(
        cubePossition,
        new Vector3(
          gameObject.transform.localScale.x,
          HEIGHT,
          gameObject.transform.localScale.z
        )
      );
    }
  }
}
