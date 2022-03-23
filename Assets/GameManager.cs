using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject enemy;
    public float timer;

    IEnumerator CreateEnemy()
    {
        yield return new WaitForSeconds(timer);

        Vector3 _pos = PlayerController.Instance.transform.position;
        _pos.y = 0;
        _pos.x += Random.Range(-10, 10);
        _pos.z += Random.Range(-10, 10);
        Instantiate(enemy).transform.position = _pos;
        _pos = PlayerController.Instance.transform.position;
        _pos.x += Random.Range(-10, 10);
        _pos.z += Random.Range(-10, 10);
        Instantiate(enemy).transform.position = _pos;
        _pos = PlayerController.Instance.transform.position;
        _pos.x += Random.Range(-10, 10);
        _pos.z += Random.Range(-10, 10);
        Instantiate(enemy).transform.position = _pos;
        StartCoroutine(CreateEnemy());

    }
}
