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
    public Vector3[,] array2Da = new Vector3[5, 4] { {new Vector3 (689.0f,387.7f,-9.6f), new Vector3(),new Vector3 (),new Vector3 () } ,
                                            { new Vector3(689.0f,387.7f,63.1f), new Vector3(764.9f,387.7f,63.1f), new Vector3(854.1f,387.7f,63.1f),new Vector3 (926.0f,387.7f,63.1f) },
                                            { new Vector3(689.0f,387.7f,140.2f), new Vector3(), new Vector3(),new Vector3 (854.1f,387.7f,140.2f) },
                                            { new Vector3(689.0f,387.7f,216.1f), new Vector3(764.9f,387.7f,216.1f),new Vector3 (854.1f, 387.7f, 216.1f),new Vector3 (926.0f, 387.7f, 216.1f) } ,
                                            { new Vector3(689.0f,387.7f,388.6f), new Vector3(), new Vector3(),new Vector3 () }};
    int row = 0;
    int col = 0;

    void Start()
    {
        commands.Clear();
        car.transform.position = array2Da[0, 0];
        
        Debug.Log(car.transform.position);
    
    }

    // Update is called once per frame
    void Update()
    {
       
    }

    public void Run(GameObject gameObj)
    {
        string stat;
        foreach (Transform child in gameObject.transform)
        {
            foreach (Transform grandChild in child)
            {
                if (grandChild)
                {
                    commands.Add(grandChild.name);
                    stat = grandChild.name;
                    Debug.Log(stat);
                    if (grandChild.name.Contains("(Clone)"))
                    {
                        int index = grandChild.name.IndexOf("(");
                        stat = grandChild.name.Substring(0, index);
                    }
                    switch (stat)
                    {
                        case "up":
                            row++;
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
        car.transform.position = array2Da[row, col];
        this.StartCoroutine(Wait());
    }

    void Rotat()
    {
        car.transform.position = Vector3.RotateTowards(car.transform.position, (transPos - car.transform.position), speed * Time.deltaTime, 0.0f);
    }

    IEnumerator Wait()
    {
        yield return new WaitForSeconds(5);
        Debug.Log("waiting");
    }

}

/**/
