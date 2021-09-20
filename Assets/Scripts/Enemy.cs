using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float range = 4f;
    public float speed = 3f;
    public float waitSecond = 3f;
    [SerializeField]
    bool XorY = false;

    int nextMove = 1;
    float rememberSpeed;
    float ctime = 0f;
  
    Vector3 rememberPosition;
    Rigidbody rigid2;

    // Start is called before the first frame update
    void Start()
    {
        rememberSpeed = speed;
        rigid2 = GetComponent<Rigidbody>();
        rememberPosition = transform.position;

        //StartCoroutine(eagleStop());
    }

    void Update()
    {
        ctime += Time.deltaTime;

        if (ctime > range)//+ waitSecond)
        {
            ctime = 0;
            //StartCoroutine(eagleStop());
            nextMove *= -1;
        }

    }

    void FixedUpdate()
    {
        //rigid2.velocity = new Vector3(nextMove * speed, rigid2.velocity.y, 0f);
        if (XorY)
            transform.Translate(speed * nextMove * 0.1f, 0f, 0f);

        else if (!XorY)
            transform.Translate(0f, speed * nextMove * 0.1f, 0f);
    }

    IEnumerator eagleStop()
    {
        speed = 0;
        yield return new WaitForSeconds(waitSecond);
        speed = rememberSpeed;
        yield return null;
    }

    public void ZeroCtime()
    {
        ctime = 0f;
        nextMove = 1;
    }

}