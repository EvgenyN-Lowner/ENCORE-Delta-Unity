using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{

    public float panSpeedNormal = 10f;
    public float panSpeedFast = 20f;
    float panSpeed;

    public float rotSpeedNormal = 1.5f;
    public float rotSpeedFast = 3f;
    float rotSpeed;

    public float zoomSpeedNormal = 500f;
    public float zoomSpeedFast = 1000f;
    float zoomSpeed;

    public Camera cam;
    Vector3 targerPos;
    bool isMoving;

    public float targetSpeed = 1f;

    public float zoomMin = 5.0f;
    public float zoomMax = 150.0f;
    private float mouseX, mouseY;

    void Awake()
    {
        cam = Camera.main;

    }

    void LateUpdate()
    {
        //Targeting();
        Movement();
        Rotation();
        Zoom();

    }


    void Movement()
    {
        Vector3 pos = transform.position;
        Vector3 forward = transform.forward;
        forward.y = 0;
        forward.Normalize();
        Vector3 right = transform.right;
        right.y = 0;
        right.Normalize();

        if (Input.GetKey(KeyCode.LeftShift))
        {
            panSpeed = panSpeedFast;
            rotSpeed = rotSpeedFast;
            zoomSpeed = zoomSpeedFast;
        }
        else
        {
            panSpeed = panSpeedNormal;
            rotSpeed = rotSpeedNormal;
            zoomSpeed = zoomSpeedNormal;
        }

            if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
        {
            pos += forward * panSpeed * Time.deltaTime;
            isMoving = true;
        }

        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
        {
            pos -= forward * panSpeed * Time.deltaTime;
            isMoving = true;
        }

        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            pos += right * panSpeed * Time.deltaTime;
            isMoving = true;
        }

        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            pos -= right * panSpeed * Time.deltaTime;
            isMoving = true;
        }

        transform.position = pos;

    }

    void Targeting()
    {
        if (Input.GetMouseButtonDown(0))
        {
            isMoving = false;
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 150f))
            {
                if (hit.collider != null)
                {
                    targerPos = hit.transform.position;
                }
            }
        }
        if (isMoving == false)
        {
        transform.position = Vector3.Lerp(transform.position, targerPos, Time.deltaTime * targetSpeed);
        }

    }

    void Rotation()
    {
        if (Input.GetMouseButton(2))
        {
            mouseX += Input.GetAxis("Mouse X") * rotSpeed;
            mouseY -= Input.GetAxis("Mouse Y") * rotSpeed;
            mouseY = Mathf.Clamp(mouseY, -30, 45);
            transform.rotation = Quaternion.Euler(mouseY, mouseX, 0);

        }
    }

    void Zoom()
    {
        Vector3 camPos = cam.transform.position;
        float distance = Vector3.Distance(transform.position, cam.transform.position);

        if (Input.GetAxis("Mouse ScrollWheel") > 0f && distance > zoomMin)
        {
            camPos += cam.transform.forward * zoomSpeed * Time.deltaTime;

        }

        if (Input.GetAxis("Mouse ScrollWheel") < 0f && distance < zoomMax)
        {
            camPos -= cam.transform.forward * zoomSpeed * Time.deltaTime;

        }

        cam.transform.position = camPos;

    }
}