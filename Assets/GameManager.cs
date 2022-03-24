using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject enemy;
    public GameObject enemy2;
    public float timer;


    private void Start()
    {
        StartCoroutine(CreateEnemy());
    }
    IEnumerator CreateEnemy()
    {
        yield return new WaitForSeconds(timer);
        if (GameObject.FindGameObjectsWithTag("Enemy").Length <= 1)
        {
            Vector3 _pos = PlayerController.Instance.transform.position;
            _pos.y = 0;
            _pos.x += Random.Range(-10, 10);
            _pos.z += Random.Range(-10, 10);
            Instantiate(enemy2).transform.position = _pos;
            _pos = PlayerController.Instance.transform.position;
            _pos.x += Random.Range(-10, 10);
            _pos.z += Random.Range(-10, 10);
            Instantiate(enemy2).transform.position = _pos;
            _pos = PlayerController.Instance.transform.position;
            _pos.x += Random.Range(-10, 10);
            _pos.z += Random.Range(-10, 10);
            Instantiate(enemy).transform.position = _pos;
        }
        StartCoroutine(CreateEnemy());

    }
}
