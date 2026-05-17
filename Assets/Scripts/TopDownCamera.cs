using UnityEngine;
using Unity.Cinemachine;

public class TopDownCamera : MonoBehaviour
{
    [SerializeField]CinemachineCamera cam;

    public void SetFollowTarget(Transform target)
    {
        cam.Follow = target;
    }
}
