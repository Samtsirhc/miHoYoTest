using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testMove : MonoBehaviour
{
    public float factor = 1;
    public Vector3 direction;
    public float distance;

    private float value = 1;
    private bool isMoving;
    private Vector3 destination;
    private Vector3 start;
    private void Start()
    {

    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (!isMoving)
            {
                isMoving = true;
                value = 1;
                start = transform.position;
                destination = transform.position + direction.normalized * distance;
            }
        }
        testtMove();
    }
    void testtMove()
    {
        if (isMoving)
        {

            value = Mathf.Lerp(value, 0, Time.deltaTime * factor);
            Vector3 pos = (1 - value) * distance * direction.normalized + start;
            transform.position = pos;
        }
        if (value < 0.05)
        {
            isMoving = false;
        }
    }
}
