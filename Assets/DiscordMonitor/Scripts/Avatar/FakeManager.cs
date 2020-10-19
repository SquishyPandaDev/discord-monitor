using UnityEngine;

namespace DiscordMonitor {
namespace Avatar {

  [AddComponentMenu("Discord Monitor/Avatar Fake Manager")]
  public class FakeManager : MonoBehaviour {
    [Space]
    [SerializeField]
    private Simulator _simulator = null;

    [Space]
    [SerializeField]
    private GameObject _avatarPrefab = null;

    [Space]
    [SerializeField]
    private Region _region = null;

    private void OnEnable() {
      if(!this._simulator.DoSimulate) {
        this.enabled = false;
        return;
      }

      Library
        .Util
        .SpawnAvatar(
          this._avatarPrefab,
          this._region,
          this.transform
        )
        .PathTo(this._region.list[Region.Name.PENTHOUSE_VOICE]);
    }
  }

}
}
