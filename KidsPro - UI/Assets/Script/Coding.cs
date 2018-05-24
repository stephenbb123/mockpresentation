using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Vuforia;

public class Coding : MonoBehaviour
{
    public List<Vector3> wayPointList;
    int currentWayPoint = 0;
    GameObject car;
    List<string> commands = new List<string>();
    Vector3 transPos;
    float speed = 20f;
    string direction;
    GetText text;
    public Vector3[,] array2Da = new Vector3[5, 4] { {new Vector3 (677.3f, 383.4f, -4.3f), new Vector3(),new Vector3 (),new Vector3 () } ,
                                            { new Vector3(677.3f, 383.4f, 61.5f), new Vector3(764.9f,383.4f,63.1f), new Vector3(854.1f,387.7f,63.1f),new Vector3 (926.0f,387.7f,63.1f) },
                                            { new Vector3(677.3f, 383.4f, 136.3f), new Vector3(), new Vector3(),new Vector3 (926.0f,387.7f,136.3f) },
                                            { new Vector3(677.3f, 383.4f,217.9f), new Vector3(764.9f,383.4f,217.9f),new Vector3 (854.1f, 387.7f, 217.9f),new Vector3 (926.0f, 387.7f, 217.9f) } ,
                                            { new Vector3(677.3f, 383.4f,292.9f), new Vector3(), new Vector3(),new Vector3 () }};
    int row = 0;
    int col = 0;

    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        if (currentWayPoint < wayPointList.Count)
        {
            transPos = wayPointList[currentWayPoint];
            StartCoroutine(Walk());

        }

        
       
    }

    public void Run(GameObject gameObj)
    {
        commands.Clear();
        car = gameObj;
        //car.transform.position = array2Da[0, 0];
        Debug.Log(car.transform.position);
        string stat;

        foreach (Transform child in gameObject.transform)
        {
            foreach (Transform grandChild in child)
            {
                if (grandChild != null)
                {

                    //System.Threading.Thread.Sleep(2000);
                    stat = grandChild.name;

                    if (grandChild.name.Contains("(Clone)"))
                    {
                        int index = grandChild.name.IndexOf("(");
                        stat = grandChild.name.Substring(0, index);
                    }
                    commands.Add(stat);
                    Debug.Log("Count:" + commands.Count);
                    switch (stat)
                    {
                        case "up":
                            MovementUP();
                            break;

                        case "down":
                            MovementDOWN();
                            //transPos.z = car.transform.position.z - 77;
                            //wayPointList.Add(transPos);
                            break;

                        case "left":
                            MovementLEFT();
                            //transPos.x = car.transform.position.x - 77;
                            //wayPointList.Add(transPos);
                            //Rotat();
                            break;

                        case "right":
                            MovementRIGHT();
                            //transPos.x = car.transform.position.x + 77;
                            //wayPointList.Add(transPos);
                            //Rotat();
                            break;
                        case "while":
                            break;
                        case "if":
                            break;
                        case "":
                            break;
                        case "default":
                            break;
                    }
                }
            }
        }

    }

    void MovementUP()
    {
        row++;
        wayPointList.Add(array2Da[row, col]);
    }
    void MovementDOWN()
    {
        row--;
        wayPointList.Add(array2Da[row, col]);
    }
    void MovementLEFT()
    {
        col--;
        wayPointList.Add(array2Da[row, col]);
    }
    void MovementRIGHT()
    {
        col++;
        wayPointList.Add(array2Da[row, col]);
    }


    IEnumerator Walk()
    {

        // move towards the target
        car.transform.position = Vector3.MoveTowards(car.transform.position, transPos, speed * Time.deltaTime);
        // rotate towards the target
        //car.transform.forward = Vector3.RotateTowards(car.transform.forward, (transPos - car.transform.position), speed * Time.deltaTime, 0.0f);
        Debug.Log("Start waiting");

        //Wait for 5 seconds
        yield return new WaitForSecondsRealtime(4);

        Debug.Log("Position: " + car.transform.position + " , TransPos: " + transPos);

        if (transPos.Equals(car.transform.position))
        {
            currentWayPoint++;
            transPos = wayPointList[currentWayPoint];
            Debug.Log("EndPoint :" + transPos);
        }
        else Debug.Log("Error");

    }

    IEnumerator Wait()
    {
        Debug.Log("Start waiting");
       
        //Wait for 4 seconds
        yield return new WaitForSecondsRealtime(5);

       
    }
    public void Clear()
    {
        row = 0;
        col = 0;
        car.transform.position = array2Da[0, 0];
        speed = 0;

        foreach (Transform child in gameObject.transform)
        {
            foreach (Transform grandChild in child)
            {
                if (grandChild != null)
                {
                    Destroy(grandChild.gameObject);

                }
            }
        }
    }
}

/*  private void OnCollisionEnter(Collision collision)
    {
       car.GetComponent<Rigidbody>().velocity = Vector3.zero;
       car.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
       
    }
       IEnumerator Wait()
    {
        yield return new WaitForSeconds(10);
        Debug.Log("waiting 10");
    }

   

    void Rotat()
    {
        car.transform.position = Vector3.RotateTowards(car.transform.position, (transPos - car.transform.position), speed * Time.deltaTime, 0.0f);
    }*/
