using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CarController : MonoBehaviour
{
    

    public List<Vector3> wayPointList;
    public List<Vector3> levelAns;
    int current;
    Vector3 target;
    Vector3 original;
    int row;
    int col;
    private float startTime;
    private float journeyLength;
    Vector3[,] mapCoord;
    float speed = 1f;
    Coding abc;
    Crashed crash;


    void Start()
    {
        Debug.Log(transform.localPosition);
        abc = FindObjectOfType<Coding>();
        mapCoord = abc.GetCoord();
        levelAns = abc.GetAns();
        wayPointList.Add(mapCoord[0, 0]);
        //startTime = Time.time;
        //current = 0;
        current = 1;
        startTime = Time.time;
        journeyLength = Vector3.Distance(wayPointList[current-1], wayPointList[current]);
        crash = FindObjectOfType<Crashed>();

    }


    void Update()
    {
        /*if (wayPointList.Count > 0)
        {
            Movement();

        }*/

        if (wayPointList.Count > 1)
        {
          
            if (transform.localPosition != wayPointList[current])
            {
                float distCovered = (Time.time - startTime) * speed;
                float fracJourney = distCovered / journeyLength;
                //transform.localPosition = wayPointList[current];
                //transform.Translate(Vector3.left * 30 * Time.deltaTime);
                transform.localPosition = Vector3.Lerp(wayPointList[current - 1], wayPointList[current], fracJourney);
                Debug.Log("Now at: " + transform.localPosition);
                Debug.Log("[Current]: " + wayPointList[current]);
                startTime = Time.time;
                
            }
            else
            {
                current++;

                /*if (wayPointList[wayPointList.Count-1]!=mapCoord[5,4])
                {
                   crash.CallCrash();
                }
                if (wayPointList[current]==Vector3.zero)
                {
                   crash.CallCrash();
                }
                */
            }






        }
       
        
    }


    public void Clear()
    { 
        row = 0;
        col = 0;
        transform.localPosition = mapCoord[0, 0];
        Debug.Log(transform.localPosition);
        current = 0;
        wayPointList.Clear();
        wayPointList.Add(mapCoord[0, 0]);
    }

    public void TransPos(Vector3 targetPos){
        transform.localPosition = targetPos;
    }

    public void SetAns(List<Vector3> ans){
        this.levelAns = ans;
    }
    public void SetPosition(Vector3 pos){
        transform.localPosition = pos;
    }
    public void SetWayPoints(Vector3 pos){
        wayPointList.Add(pos);
    }
    public List<Vector3> GetWayPoints()
    {
        return wayPointList;
    }

    
}

/* void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Pick Up"))
        {
            other.gameObject.SetActive(false);
        }
    }

    public void Movement(){
        
        if (current < wayPointList.Count)
        {
            transform.position = Vector3.MoveTowards(transform.position, wayPointList[current], speed * Time.deltaTime);
            transform.forward = Vector3.RotateTowards(transform.position, wayPointList[current], speed * Time.deltaTime, 0.0f);
        }
        if (transform.position == wayPointList[current])
        {
            current++;

        }
    }
*/
