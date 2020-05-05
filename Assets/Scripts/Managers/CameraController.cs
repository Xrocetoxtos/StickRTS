using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private Camera camera;

    [SerializeField] private float moveSpeed;
    [SerializeField] private float zoomSpeed;

    [SerializeField] private float minCamSize = 1;
    [SerializeField] private float maxCamSize = 10;

    [SerializeField] private float edgeRange = 20;

    private void Awake()
    {
        camera = GetComponent<Camera>();
    }

    private void Update()
    {
        Move();
        Zoom();
    }

    private void Move()
    {
        Vector2 mousePosition = Input.mousePosition;
        Vector2 moveVector = Vector2.zero;

        if (mousePosition.x < edgeRange || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
            moveVector += Vector2.left;
        if (mousePosition.x > Screen.width - edgeRange || Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
            moveVector += Vector2.right;
        if (mousePosition.y < edgeRange || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
            moveVector += Vector2.down;
        if (mousePosition.y > Screen.height - edgeRange || Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow)) 
            moveVector += Vector2.up;

        transform.position += new Vector3(moveVector.x, moveVector.y, 0) * moveSpeed * Time.deltaTime;
    }

    private void Zoom()
    {
        camera.orthographicSize = Mathf.Clamp(camera.orthographicSize + Input.GetAxis("Mouse ScrollWheel") * zoomSpeed, minCamSize, maxCamSize);
    }
}
