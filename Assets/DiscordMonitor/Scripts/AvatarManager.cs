using System.Collections.Generic;
using UnityEngine;

namespace DiscordMonitor {

  using WSReceive     = WS.Message.Receive;
  using WSReceiveKind = WS.Message.Receive.Kind;

  public class AvatarManager : MonoBehaviour {
    [SerializeField]
    private GameObject _avatarPrefab = null;

    [SerializeField]
    private WSClientManager _wsClientManager = null;

    [SerializeField]
    private Transform _spawnArea = null;

    private Dictionary<string, Avatar> _avatarById =
      new Dictionary<string, Avatar>();

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

        avatarObject.transform.position = new Vector3(
          Random.Range(-spawnBounds.x, spawnBounds.x),
          0,
          Random.Range(-spawnBounds.z, spawnBounds.z)
        );

        var avatar = avatarObject.GetComponent<Avatar>();

        avatar.SetName(message.name);

        this._avatarById.Add(
          message.id,
          avatar
        );
      }
    }
  }

}
