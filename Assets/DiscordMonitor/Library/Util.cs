using UnityEngine;

namespace DiscordMonitor {
namespace Library {

  using WSReceive = WS.Message.Receive;

  public static class Util {
    public static bool FilterMessage(
      WSClientManager  wsClientManager,
      WSReceive.Kind     wantedKind,
      out WSReceive.Base message
    ) {
      bool result = false;

      message = null;

      var peekResult = wsClientManager.TryReceivePeek(
        out WSReceive.Base tempMessage
      );

      if(peekResult) {
        if(tempMessage.kind == wantedKind) {
          result = wsClientManager.TryReceiveDequeue(out message);
        }
      }

      return result;
    }

    public static Vector3 Random2DPositionInRegion(
      Transform region
    ) {
      var position = region.position;

      var bounds = region.localScale / 2;

      var rangeX = new Vector2(
        position.x - bounds.x,
        position.x + bounds.x
      );

      var rangeZ = new Vector2(
        position.z - bounds.z,
        position.z + bounds.z
      );

      return new Vector3(
        Random.Range(rangeX.x, rangeX.y),
        0,
        Random.Range(rangeZ.x, rangeZ.y)
      );
    }

    public static Avatar.Actor SpawnAvatar(
      GameObject avatarPrefab,
      Region     region,
      Transform  parent
    ) {
      var spawnPosition =
        Random2DPositionInRegion(
          region.list[Region.Name.AVATAR_SPAWN]
        );

      return GameObject
        .Instantiate(
          avatarPrefab,
          spawnPosition,
          Quaternion.identity,
          parent
        )
        .GetComponent<Avatar.Actor>();
    }
  }
}
}
