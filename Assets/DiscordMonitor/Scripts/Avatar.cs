using UnityEngine;
using UnityEngine.AI;
using TMPro;

namespace DiscordMonitor {

  public class Avatar : MonoBehaviour {
    public void SetName(string name) {
      this._nameText.text = name;
    }

    public void PathTo(GameObject region) {
      this._navMeshAgent.SetDestination(region.transform.position);
    }

    [SerializeField]
    private TextMeshProUGUI _nameText = null;

    [SerializeField]
    private NavMeshAgent _navMeshAgent = null;
  }

}
