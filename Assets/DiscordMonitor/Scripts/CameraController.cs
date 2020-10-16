using UnityEngine;
using UnityEngine.InputSystem;

namespace DiscordMonitor {

  public class CameraController : MonoBehaviour {
    #region Input Events

    public void OnPan(InputAction.CallbackContext callback) {
      this._panDirection = callback.ReadValue<Vector2>();
    }

    public void OnRotate(InputAction.CallbackContext callback) {
      this._rotation = callback.ReadValue<Vector2>().x;
    }

    public void OnToggleRotate(InputAction.CallbackContext callback) {
      this._doRotate = callback.started;
    }

    public void OnZoom(InputAction.CallbackContext callback) {
      this._zoom = callback.ReadValue<Vector2>().y;
    }

    #endregion

    private void Update() {
      var transformPan =
        this._panDirection * this._speed.pan * Time.deltaTime;

      var transformZoom =
        this._zoom * this._speed.zoom * Time.deltaTime;

      var transform = new Vector3(
        transformPan.x,
        transformPan.y,
        transformZoom
      );

      this.transform.Translate(transform, Space.Self);

      if(this._doRotate) {
        var rotationFactor =
          this._rotation * this._speed.rotate * Time.deltaTime;

        var currentRotation = this.transform.rotation.eulerAngles;

        var newRotation = new Vector3(
          45.0f,
          currentRotation.y + rotationFactor,
          0
        );

        this.transform.rotation = Quaternion.Slerp(
          this.transform.rotation,
          Quaternion.Euler(newRotation),
          0.5f
        );
      }
    }

    [SerializeField]
    private Speed _speed = new Speed();

    private bool _doRotate = false;

    private Vector2 _panDirection;
    private float   _rotation;
    private float   _zoom;

    [System.Serializable]
    private class Speed {
      public float pan    { get => this._pan; }
      public float rotate { get => this._rotate; }
      public float zoom   { get => this._zoom; }

      [SerializeField]
      private float _pan = 20;

      [SerializeField]
      private float _rotate = 1;

      [SerializeField]
      private float _zoom = 1;
    }
  }

}
