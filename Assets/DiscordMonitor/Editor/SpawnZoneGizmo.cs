using UnityEngine;
using UnityEditor;

namespace DiscordMonitor {

  public class SpawnZoneGizmo : MonoBehaviour {
    [DrawGizmo(GizmoType.NonSelected)]
    static void DrawSpawnZone(GameObject gameObject, GizmoType type) {
      if(gameObject.name == "Avatar Spawn") {
        var possition = gameObject.transform.position
          + new Vector3(0f, 0.2f, 0f);

        Gizmos.DrawWireCube(
          possition,
          new Vector3(
            gameObject.transform.localScale.x,
            0,
            gameObject.transform.localScale.z
          )
        );
      }
    }
  }

}
