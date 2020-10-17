using UnityEngine;

namespace DiscordMonitor {

  public class PathingTest : MonoBehaviour {
    [SerializeField]
    private GameObject _avatar = null;

    [SerializeField]
    private Region _region = new Region();

    private void OnEnable() {
      GameObject
        .Instantiate(
          this._avatar,
          this._region.avatarSpawn.transform.position,
          Quaternion.identity
        )
        .GetComponent<Avatar>()
        .PathTo(this._region.penthouseVoice);
    }

    [System.Serializable]
    private class Region {
      public GameObject avatarSpawn { get => this._avatarSpawn; }
      public GameObject penthouseVoice { get => this._penthouseVoice; }

      [SerializeField]
      private GameObject _avatarSpawn = null;

      [SerializeField]
      private GameObject _penthouseVoice = null;
    }
  }

}
