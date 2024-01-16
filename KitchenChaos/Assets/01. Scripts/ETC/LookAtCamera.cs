using UnityEngine;

public partial class LookAtCamera : MonoBehaviour
{
    [SerializeField] Mode lookAtMode;
    private Transform mainCameraTrm;

    private void Awake()
    {
        mainCameraTrm = Camera.main.transform;
    }

	private void LateUpdate()
    {
        switch(lookAtMode)
        {
            case Mode.LookAt: 
                transform.LookAt(mainCameraTrm);
                break;
            case Mode.LookAtInverted:
                Vector3 dirFromCam = transform.position - mainCameraTrm.position; 
                transform.LookAt(transform.position + dirFromCam);
                break;
            case Mode.CameraForward:
                transform.forward = mainCameraTrm.forward;
                break;
            case Mode.CameraForwardInverted:
                transform.forward = -mainCameraTrm.forward;
                break;
        }
    }
}
