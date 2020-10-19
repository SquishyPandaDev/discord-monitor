using UnityEngine;

namespace DiscordMonitor {
namespace Avatar {

  [AddComponentMenu("Discord Monitor/Avatar Simulator")]
  public class Simulator : MonoBehaviour {
    public bool DoSimulate => this._simulate;

    [Space]
    [SerializeField]
    private bool _simulate = false;
  }

}
}
