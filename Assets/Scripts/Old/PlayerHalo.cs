using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHalo : MonoBehaviour
{
    public GameObject player;
    public PlayerController playerController;
    public Vector3 offset;
    // Start is called before the first frame update
    void Start()
    {
        playerController = player.GetComponent<PlayerController>();
        offset = transform.position - player.transform.position;

    }

    // Update is called once per frame
    void Update()
    {
        Vector3 _origen = transform.position;
        Vector3 _new = player.transform.position + offset;
        Vector3 _diff = _new - _origen;
        Vector3 _tmp = Vector3.Dot(_diff, playerController.moveDirection.normalized) * playerController.moveDirection.normalized;
        transform.position = transform.position + _tmp;
    }
}
