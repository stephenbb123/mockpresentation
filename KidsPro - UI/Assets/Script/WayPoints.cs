using UnityEngine;
using System.Collections;

public class WayPoints : MonoBehaviour
{

    // put the points from unity interface
    public Vector3[] wayPointList;
    public GameObject win;
    int currentWayPoint = 0;
    Vector3 targetWayPoint;
    public float speed = 4f;

    // Use this for initialization
    void Start()
    {
        win.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        // check if we have somewere to walk
        if (currentWayPoint < this.wayPointList.Length)
        {
            targetWayPoint = wayPointList[currentWayPoint];
            walk();

        }
        else win.SetActive(true);
    }

    void walk()
    {
        // rotate towards the target
        transform.forward = Vector3.RotateTowards(transform.forward, (targetWayPoint - transform.position), speed * Time.deltaTime, 0.0f);
        //Debug.Log(targetWayPoint +" , "+ transform.position);
        // move towards the target
        transform.position = Vector3.MoveTowards(transform.position, targetWayPoint, speed * Time.deltaTime);

        if (transform.position == targetWayPoint)
        {
            currentWayPoint++;
            targetWayPoint = wayPointList[currentWayPoint];
        }


    }

    private void OnCollisionEnter(Collision collision)
    {
        Destroy(GetComponent<Rigidbody>());
        speed = 0;
        win.SetActive(true);
    }
}