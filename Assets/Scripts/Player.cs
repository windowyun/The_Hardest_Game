using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    [SerializeField]
    Transform startPoint;
    [SerializeField]
    Transform endPoint;

    [SerializeField]
    int currentScene = 0;

    Vector3 spawnPoint;
    public float moveSpeed = 1.0f;
    //bool differentPoint = false;
    bool spaceOn = false;
    float spaceDirect = 1f;

    int currentCoin;
    GameObject[] coins;
    List<GameObject> coin = new List<GameObject>();

    GameObject[] number1;
    GameObject[] number2;

    Vector3[] coinPosition;
    Vector3[] enemyPosition;

    void Start()
    {
        number1 = GameObject.FindGameObjectsWithTag("Coin");
        number2 = GameObject.FindGameObjectsWithTag("Enemy");

        coinPosition = new Vector3[number1.Length];
        enemyPosition = new Vector3[number2.Length];

        if (number1 == null)
        {
            currentCoin = 0;
            //differentPoint = true;
        }


        else
            currentCoin = number1.Length;



        StartCoroutine(RemeberPosition());
        transform.position = startPoint.position + new Vector3(0f,0.74f,0f);
        spawnPoint = startPoint.position + new Vector3(0f, 0.74f, 0f);
    }

    void Update()
    {
        bool raycastHit = Physics.Raycast(transform.position, new Vector3(0f,spaceDirect,0f), Mathf.Infinity, LayerMask.GetMask("Ground"));

        if (raycastHit && !spaceOn && Input.GetKeyDown(KeyCode.Space))
        {
            spaceOn = true;
        }
    }

    private void FixedUpdate()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        bool raycastG = Physics.Raycast(transform.position + new Vector3(h, 0f, v)  * 0.55f, new Vector3(0f, spaceDirect * -1f, 0f), Mathf.Infinity, LayerMask.GetMask("Ground"));

        if (raycastG)
        {
            if (!spaceOn)
                transform.Translate(new Vector3(h, 0f, v).normalized * moveSpeed * 0.1f);

            else if (spaceOn)
                transform.Translate(new Vector3(0f, spaceDirect, 0f).normalized * 5f * 0.1f);
        }    
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Ground")
        {
            if(spaceOn)
            {
                spaceOn = false;
                spaceDirect *= -1f;
            }
        }    
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            transform.position = spawnPoint;
            StartCoroutine(ResetPosition());
            StartCoroutine(ActiveCoin());
        }

        else if (other.gameObject.tag == "GreenSpace")
        {
            spawnPoint = other.gameObject.transform.position + new Vector3(0f,0.74f * spaceDirect,0f);
            
            if(currentCoin - coin.Count > 0)
            {
                currentCoin -= coin.Count;
                coin.Clear();
            }

            else if( currentCoin - coin.Count <= 0 && other.transform == endPoint)
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

    IEnumerator ResetPosition()
    {
        

        if(coinPosition != null)
        {
            for(int i = 0; i< number1.Length; i++)
            {
                number1[i].transform.position = coinPosition[i];
                if (number1[i].GetComponent<Enemy>() != null)
                    number1[i].GetComponent<Enemy>().ZeroCtime();
            }
        }


        if(enemyPosition != null)
        {
            for (int i = 0; i < number2.Length; i++)
            {
                number2[i].transform.position = enemyPosition[i];
                if (number2[i].GetComponent<Enemy>() != null)
                    number2[i].GetComponent<Enemy>().ZeroCtime();
            }
        }

        yield return null;
    }

    IEnumerator RemeberPosition()
    {
        if (number1 != null)
        {
            for (int i = 0; i < number1.Length; i++)
            {
                coinPosition[i] = number1[i].transform.position;
            }
        }

        if (number2 != null)
        {
            for (int i = 0; i < number2.Length; i++)
            {
                enemyPosition[i] = number2[i].transform.position;
            }
        }
        yield return null;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

      
        Gizmos.DrawRay(transform.position + new Vector3(h, 0f, v) * 0.5f, new Vector3(0f, spaceDirect * -1f, 0f) * 10f);
    }
}
