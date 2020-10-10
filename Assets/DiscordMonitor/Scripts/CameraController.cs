using UnityEngine;

namespace DiscordMonitor {

  public class CameraController : MonoBehaviour {
    private void Update() {
      this.gameObject.transform.LookAt(Vector3.zero);

      this.gameObject.transform.Translate(
        Vector3.right * Time.deltaTime
      );
    }
  }

}
