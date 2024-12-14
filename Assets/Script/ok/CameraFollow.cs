using UnityEngine;

/* https://docs.unity3d.com/ScriptReference/Vector3.Lerp.html */
//guidance from external sources about lerp function from unity website
//taojin sha 
// 2024.10.18
public class CameraFollow : MonoBehaviour
{
    public Transform player; 
    public Vector3 offset;   
    public float smoothSpeed = 0.2f; 

    private void LateUpdate()
    {
        
        Vector3 desiredPosition = player.position + offset;

        
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);

       
        transform.position = smoothedPosition;

        
    }
}
