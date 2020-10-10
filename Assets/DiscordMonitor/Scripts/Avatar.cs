using UnityEngine;
using TMPro;

namespace DiscordMonitor {

  public class Avatar : MonoBehaviour {
    public void SetName(string name) {
      this._nameText.text = name;
    }

    [SerializeField]
    private TextMeshProUGUI _nameText = null;
  }

}
