using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CompassController : MonoBehaviour
{
    [SerializeField] Transform pointer;
    [SerializeField] Transform target;
    [SerializeField] RectTransform compassBar;

    RectTransform rect;
    Transform player;

    private void Start() 
    {
        rect = pointer.GetComponent<RectTransform>();
        player = GameObject.FindWithTag("Player").transform;
    }

    private void Update() 
    {
        Vector3[] v = new Vector3[4];
        compassBar.GetLocalCorners(v);
        float pointerScale = Vector3.Distance(v[1], v[2]); // Both bottom corners

        Vector3 direction = target.position - player.position;
        float angleToTarget = Vector3.SignedAngle(player.forward,direction,player.up);
        angleToTarget = Mathf.Clamp(angleToTarget, -90, 90) / 180.0f * pointerScale;
        rect.localPosition = new Vector3(angleToTarget, rect.localPosition.y,rect.localPosition.z);
    }
}
