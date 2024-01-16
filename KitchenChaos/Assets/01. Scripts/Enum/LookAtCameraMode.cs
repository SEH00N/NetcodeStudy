using UnityEngine;

public partial class LookAtCamera
{
	public enum Mode {
        LookAt, // set my forward to the camera position
        LookAtInverted, // subtraction the camera position from my forward
        CameraForward, // match my forward with camera
        CameraForwardInverted // match my forward with camera inverted
    }
}
