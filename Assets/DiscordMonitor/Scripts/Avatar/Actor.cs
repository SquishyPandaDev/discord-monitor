using UnityEngine;
using UnityEngine.AI;
using TMPro;

namespace DiscordMonitor {
namespace Avatar {

  [AddComponentMenu("Discord Monitor/Avatar Actor")]
  public class Actor : MonoBehaviour {
    public void SetName(string name) {
      this._nameText.text = name;
    }

    public void PathTo(Transform region) {
      var targetLocation =
        Library.Util.Random2DPositionInRegion(region);

      var result=
        this._navMeshAgent.SetDestination(targetLocation);

      if(!result) {
        Debug.Log($"Could not path to '{transform.position}'");
      }
    }

    [Space]
    [SerializeField]
    private TextMeshProUGUI _nameText = null;

    [Space]
    [SerializeField]
    private NavMeshAgent _navMeshAgent = null;
  }

}
}
