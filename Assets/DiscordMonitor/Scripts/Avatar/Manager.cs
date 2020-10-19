using System.Collections.Generic;
using UnityEngine;

namespace DiscordMonitor {
namespace Avatar {

  using WSReceive     = WS.Message.Receive;
  using WSReceiveKind = WS.Message.Receive.Kind;

  [AddComponentMenu("Discord Monitor/Avatar Manager")]
  public class Manager : MonoBehaviour {
    [Space]
    [SerializeField]
    private Simulator _simulator = null;

    [Space]
    [SerializeField]
    private GameObject _avatarPrefab = null;

    [Space]
    [SerializeField]
    private WSClientManager _wsClientManager = null;

    [Space]
    [SerializeField]
    private Transform _spawnArea = null;

    private Dictionary<string, Actor> _avatarById =
      new Dictionary<string, Actor>();

    private void OnEnable() {
      if(this._simulator.DoSimulate) {
        this.enabled = false;
      }
    }

    private void Update() {
      var filterResult = Library.Util.FilterMessage(
        this._wsClientManager,
        WSReceiveKind.STREAMED_CONNECTED_USER,
        out WSReceive.Base rawMessage
      );

      if(!filterResult) return;

      var message = (WSReceive.StreamedConnectedUser)rawMessage;

      var existingUser = this._avatarById.ContainsKey(message.id);

      if(!existingUser) {
        var avatarObject = Instantiate(
          this._avatarPrefab,
          this._avatarPrefab.transform.localPosition,
          Quaternion.identity
        );

        var spawnBounds = this._spawnArea.localScale / 2;

        var spawnPosition = this._spawnArea.position;

        avatarObject.transform.position = new Vector3(
          spawnPosition.x + Random.Range(-spawnBounds.x, spawnBounds.x),
          0,
          spawnPosition.z + Random.Range(-spawnBounds.z, spawnBounds.z)
        );

        var avatar = avatarObject.GetComponent<Actor>();

        avatar.SetName(message.name);

        this._avatarById.Add(
          message.id,
          avatar
        );
      }
    }
  }

}
}
