
using UnityEngine;

public class HeadPusher : MonoBehaviour
{
    public Vector3 torqueDirection;
    public float torque;
    
    public void PushDatHead(Rigidbody player)
    {

        if (player == null) 
        {
            return;
        }

        player.AddTorque(torqueDirection * torque, ForceMode.Impulse);
    }

    // private void OnDrawGizmos()
    // {
    //     Gizmos.color = Color.red;
    //     Gizmos.DrawRay(transform.position, transform.forward * 15f);
    // }
}
