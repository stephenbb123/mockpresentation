using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Vuforia;

public class Coding : MonoBehaviour
{
    public GameObject car;
    List<string> commands = new List<string>();
    Vector3 transPos;
    float speed = 60;
    string direction;
    Vector3[,] array2Da = new Vector3[5, 4] { {new Vector3 (689.0f,387.7f,-9.6f), new Vector3(),new Vector3 (),new Vector3 () } ,
                                            { new Vector3(689.0f,387.7f,63.1f), new Vector3(764.9f,387.7f,63.1f), new Vector3(854.1f,387.7f,63.1f),new Vector3 (926.0f,387.7f,63.1f) },
                                            { new Vector3(689.0f,387.7f,140.2f), new Vector3(), new Vector3(),new Vector3 (854.1f,387.7f,140.2f) },
                                            { new Vector3(689.0f,387.7f,216.1f), new Vector3(764.9f,387.7f,216.1f),new Vector3 (854.1f, 387.7f, 216.1f),new Vector3 (926.0f, 387.7f, 216.1f) } ,
                                            { new Vector3(689.0f,387.7f,388.6f), new Vector3(), new Vector3(),new Vector3 () }};


    void Start()
    {
        commands.Clear();
        transPos = car.transform.position;
      
        Debug.Log(transPos);
    
    }

    // Update is called once per frame
    void Update()
    {
       
    }

        
     


    public void Start(GameObject gameObj)
    {
        foreach (Transform child in gameObject.transform)
        {
            foreach (Transform grandChild in child)
            {
                if (grandChild)
                {
                    commands.Add(grandChild.name);
                }
            }
        }

        if (commands != null)
        {
            foreach (string command in commands)
            {
                string stat = command;

                {
                    if (command.Contains("(Clone)"))
                    {
                        int index = command.IndexOf("(");
                        stat = command.Substring(0, index);
                    }
                    Debug.Log(stat);

                    switch (stat)
                    {
                        case "up":
                            transPos.z = car.transform.position.z + 77;
                            MovementUp();
                            break;

                        case "down":
                            transPos.z = car.transform.position.z - (77 * 4);
                            //Movement();
                            break;

                        case "left":
                            transPos.x = car.transform.position.x - (77 * 2);
                            Rotat();
                            break;

                        case "right":

                            transPos.x = car.transform.position.x + (77 * 2);
                            Rotat();
                            break;
                    }
                }
            }

        }






    }

    private void OnCollisionEnter(Collision collision)
    {
       car.GetComponent<Rigidbody>().velocity = Vector3.zero;
       car.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
       
       
    }
    void MovementUp()
    {
        do { 
            car.transform.position = Vector3.MoveTowards(car.transform.position, transPos, speed * Time.deltaTime);
        }
        while (car.transform.position == transPos);
    }

    void Rotat()
    {
        car.transform.position = Vector3.RotateTowards(car.transform.position, (transPos - car.transform.position), speed * Time.deltaTime, 0.0f);
    }

}

/**/
