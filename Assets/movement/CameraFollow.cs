using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public float followSpeed = 2f;
    public Transform target;
    public float yOffset = 1f;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 newPos = new Vector3(target.position.x+2f,target.position.y + yOffset,-10f);
        transform.position = Vector3.Slerp(transform.position,newPos,followSpeed*Time.deltaTime);    
    }
}
