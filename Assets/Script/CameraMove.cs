using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    public Transform target;
    public GameObject targetObj;
    public Transform minimapCamera;

    public float dis = 5;
    public float rotSpeed = 2;
    public float heigh = 3;

    private float mouseX;

    private void Update()
    {
        
        foreach (var playerObj in GameObject.FindGameObjectsWithTag("Player"))
        {
            Player player;
            if (playerObj.TryGetComponent<Player>(out player))
            {
                if (player.root)
                {
                    target = playerObj.transform;
                    targetObj = playerObj;
                }
            }
        }
        
        if (target != null && GameManager.Instance.cameraMove)
        {
            minimapCamera.transform.position = new Vector3(target.position.x, target.transform.position.y+10, target.transform.position.z);
            mouseX += Input.GetAxis("Mouse X") * rotSpeed;
            Player player = target.GetComponent<Player>();
            if (player.dir != Vector3.zero)
            {
                targetObj.transform.eulerAngles = new Vector3(0, transform.rotation.eulerAngles.y, 0);
            }
            Quaternion rot = Quaternion.Euler(0, mouseX, 0);
            Vector3 disPos = target.position - (rot * Vector3.forward * dis);
            disPos = disPos + Vector3.up * heigh;
            transform.position = disPos;
            transform.LookAt(target.position);
            
        }
        

        
    }
}
