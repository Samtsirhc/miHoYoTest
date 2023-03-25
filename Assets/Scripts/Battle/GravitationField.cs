using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravitationField : MonoBehaviour
{
    public float force;
    public float maxSpeed;
    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Enemy")
        {
            Gravitation(other.gameObject, force);
        }
    }
    public void Gravitation(GameObject obj, float force)
    {
        Vector3 dir = transform.position - obj.transform.position;
        dir.y = 0;
        dir.Normalize();
        CharacterController characterController = obj.GetComponent<CharacterController>();
        characterController.Move(dir * force * Time.deltaTime);
    }


}
