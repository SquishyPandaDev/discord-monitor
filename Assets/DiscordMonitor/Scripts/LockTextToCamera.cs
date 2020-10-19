using UnityEngine;
using UnityEditor;

namespace DiscordMonitor {

  public class LockTextToCamera : MonoBehaviour {
    private Vector3 _lookPoint = Vector3.zero;

    private void Update() {
      transform.rotation = Quaternion.LookRotation(
        this.transform.position - Camera.main.transform.position
      );
    }
  }

}
