using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{

    [SerializeField]
    Transform cameraArm;
    [SerializeField]
    float speed = 10f;


    Vector3 worldDefalut;

    void Start()
    {
        worldDefalut = transform.forward;    
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void LateUpdate()
    {
        LookAround();
        zoomin();
    }

    void LookAround()
    {
        if (Input.GetKey(KeyCode.Mouse0))
        {
            Vector2 mouseDelta = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y")) * 2f;


            Vector3 camAngle = cameraArm.rotation.eulerAngles;

            cameraArm.rotation = Quaternion.Euler(camAngle.x - mouseDelta.y, camAngle.y + mouseDelta.x, camAngle.z);
        }
    }

    void zoomin()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel") * speed;

        transform.localPosition = transform.localPosition + (worldDefalut * scroll);
    }
}
