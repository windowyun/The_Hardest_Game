using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    [SerializeField]
    Transform startPoint;

    [SerializeField]
    int currentScene = 0;

    Vector3 spawnPoint;
    public float moveSpeed = 1.0f;
    bool differentPoint = false;

    int currentCoin;
    GameObject[] coins;
    List<GameObject> coin = new List<GameObject>();

    void Start()
    {
        GameObject[] number = GameObject.FindGameObjectsWithTag("Coin");

        if (number == null)
        {
            currentCoin = 0;
            differentPoint = true;
        }

        else
            currentCoin = number.Length;

        transform.position = startPoint.position + new Vector3(0f,0.74f,0f);
        spawnPoint = startPoint.position + new Vector3(0f, 0.74f, 0f);
    }

    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        transform.Translate(new Vector3(h, 0f, v).normalized * moveSpeed * 0.1f);
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            transform.position = spawnPoint;

            StartCoroutine(ActiveCoin());
        }

        else if (other.gameObject.tag == "GreenSpace")
        {
            spawnPoint = other.gameObject.transform.position + new Vector3(0f,0.74f,0f);
            
            if(differentPoint && other.transform == startPoint)
            {
                return;
            }

            else if(currentCoin - coin.Count > 0)
            {
                currentCoin -= coin.Count;
                coin.Clear();
            }

            else if( currentCoin - coin.Count <= 0)
            {
                SceneManager.LoadScene(currentScene + 1);
            }
        }

        else if(other.gameObject.tag == "Coin")
        {
            other.gameObject.SetActive(false);
            coin.Add(other.gameObject);
        }
    }

    IEnumerator ActiveCoin()
    {
        for(int i = 0; i< coin.Count; i++)
        {
            coin[i].SetActive(true);
        }

        coin.Clear();

        yield return null;
    }

    
}
