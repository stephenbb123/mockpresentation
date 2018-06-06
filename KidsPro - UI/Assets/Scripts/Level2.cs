using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Level2 : MonoBehaviour
{


    List<string> commands = new List<string>();
    float speed = 20f;
    SceneObject scene;
    CarController car;
    int counter;
    int row;
    int col;
    int num;
    Crashed crash;
    Flag flag;
    SetText randText;
    public Vector3[,] map = { {new Vector3 (-420f, -137f, -165f), new Vector3(),new Vector3 (),new Vector3 () } ,
                                            { new Vector3(-514f, -137f, -165f), new Vector3(-514f, -137f, -63f), new Vector3(-514f, -137f, 64f),new Vector3 (-514f, -137f, 148f) },
                                            { new Vector3(-608f, -137f, -165f), new Vector3(), new Vector3(),new Vector3 (-608f, -137f, 148f) },
                                            { new Vector3(-702f, -137f, -165f), new Vector3(-702f, -137f, -63f),new Vector3 (-702f, -137f, 64f),new Vector3 (-702f, -137f, 148f) } ,
                                            { new Vector3(-796f, -137f, -165f), new Vector3(), new Vector3(),new Vector3 () }};

    Coding code;
    List<Vector3> lastPosition;
    void Start()
    {
        car = FindObjectOfType<CarController>();
        car.enabled = false;
        
       

    }

    // Update is called once per frame
    void Update()
    {
        code = FindObjectOfType<Coding>();
        num = code.getUserInput();
        car = FindObjectOfType<CarController>();
        lastPosition = car.GetWayPoints();
        if (num == 3)
        {
            if (Vector3.Distance(map[4, 0], car.transform.localPosition) < 0.1f)
            {
                randText = FindObjectOfType<SetText>();
                randText.SetString("Congratulations! You won!");
            }
        }
        else if(num!=3)
        {
            
                if (Vector3.Distance(lastPosition[lastPosition.Count - 1], car.transform.localPosition) < 0.1f)
                {
                    randText = FindObjectOfType<SetText>();
                    randText.SetString("Sorry. You did not get 3 flags.");
                }
            
        }
        if (Vector3.Distance(car.transform.localPosition, Vector3.zero) < 0.1f)
        {
            randText = FindObjectOfType<SetText>();
            randText.SetString("Crashed. Please try again.");
            car.Clear();
        }


    }


    public void SetWayPoint()
    {

        code = FindObjectOfType<Coding>();
        commands=code.Read();
        car = FindObjectOfType<CarController>();
        flag = FindObjectOfType<Flag>();
        randText = FindObjectOfType<SetText>();
        int start = 0;
        int end = 0;
        for (int i = 0; i < commands.Count; i++) //loop all input.
        {
            if (commands[i] == "while")   //do following loop if input contains while.
            {

                start = commands.IndexOf("{");
                end = commands.IndexOf("}");
                Debug.Log("Start index:" + start + ", end " + end);
                for (counter = 1; counter < num; counter++)
                {
                    for (int y = start + 1; y < end; y++)
                    {
                        commands.Insert(end + 1, commands[y]);

                        Debug.Log("Added");
                    }
                }
            }

            switch (commands[i])    //if there is no while in the user input
            {
                case "up":
                    row++;
                    car.SetWayPoints(map[row, col]);
                    break;

                case "down":
                    row--;
                    car.SetWayPoints(map[row, col]);
                    break;

                case "left":
                    col--;
                    car.SetWayPoints(map[row, col]);
                    break;

                case "right":
                    col++;
                    car.SetWayPoints(map[row, col]);
                    break;
                case "loop":

                    for (int u = 0; u < 2; u++)
                    {
                        row++;
                        car.SetWayPoints(map[row, col]);
                    }
                    for (int u = 0; u < 3; u++)
                    {
                        col++;
                        car.SetWayPoints(map[row, col]);
                    }
                    for (int u = 0; u < 2; u++)
                    {
                        row--;
                        car.SetWayPoints(map[row, col]);
                    }
                    for (int u = 0; u < 3; u++)
                    {
                        col--;
                        car.SetWayPoints(map[row, col]);
                    }
                    break;

                case "default":
                    break;
            }
        }
        car.enabled = true;
    }
    public void Clear()
    {
        code = FindObjectOfType<Coding>();
        code.Clear();
    }
}