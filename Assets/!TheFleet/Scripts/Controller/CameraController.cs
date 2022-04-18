using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EZCameraShake;

public class CameraController : MonoBehaviourSingleton<CameraController>
{
    // Start is called before the first frame update
    public void Shake(CameraShakeInstance preset)
    {
        CameraShaker.Instance.Shake(preset);
    }
}
