using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Level3 : MonoBehaviour
{


    List<string> commands = new List<string>();
    float speed = 20f;
    SceneObject scene;
    CarController car;
    int counter;
    List<Vector3> ans = new List<Vector3>();
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
    void Start()
    {
        car = FindObjectOfType<CarController>();
        code = FindObjectOfType<Coding>();
        car.enabled = false;
       

    }

    // Update is called once per frame
    void Update()
    {
        
            if (row < 0 || col < 0)
            {
                code.Crashed();
            }

           
        


    }

    public void SetWayPoint()
    {
        code = FindObjectOfType<Coding>();
        commands = code.Read();
        if (!commands.Contains("else"))
        {
            code.LogicError();
            randText = FindObjectOfType<SetText>();
            randText.SetString(randText.GetString() + "\nThere should be an \"else\" statement.");
        }
        car = FindObjectOfType<CarController>();
        flag = FindObjectOfType<Flag>();
        randText = FindObjectOfType<SetText>();
        int number;
        int start = 0;
        int end = 0;
        for (int i = 0; i < commands.Count; i++) //loop all input.
        {
            if (commands[i] == "if")
            {

                number = randText.GetNum();
                start = commands.IndexOf("{");
                end = commands.IndexOf("}");
                int ifIndex = commands.IndexOf("if");
                Debug.Log("Start index:" + start + ", end " + end);

                if (number % 2 == 1)
                {
                    if (commands.Contains("odd"))
                    {
                        ifIndex = commands.IndexOf("odd");
                        start = ifIndex + 1;
                        end = commands.IndexOf("}");
                        if (start > end)
                        { end = commands.LastIndexOf("}"); }
                        if (commands[start + 1] == "up")
                        {
                            for (int a = start + 1; a < end; a++)
                            {

                                commands.Insert(end + 1, commands[a]);

                            }
                        }

                        else
                        {
                            randText.SetString("Wrong Way.Please reset and try again.");
                            break;
                        }
                    }
                }

                else if (commands.Contains("even"))
                {

                    start = commands.IndexOf("else");
                    end = commands.LastIndexOf("}");
                    if (commands[start + 1] == "odd")
                    {
                        for (int b = start + 3; b < end; b++)
                        {
                            if (commands[b].Equals("up"))
                            {
                                commands.Insert(end + 1, commands[b]);

                            }
                            else
                            {
                                randText.SetString("Wrong Way.Please reset and try again.");
                                break;
                            }
                        }
                    }
                }

                if (number % 2 == 0)
                {
                    if (commands.Contains("even"))
                    {
                        ifIndex = commands.IndexOf("even");
                        start = ifIndex + 1;
                        end = commands.IndexOf("}");
                        if (start > end)
                        { end = commands.LastIndexOf("}"); }
                        if (commands[start + 1] == "right")
                        {
                            for (int a = start + 1; a < end; a++)
                            {
                                commands.Insert(end + 1, commands[a]);

                            }
                        }
                        else
                        {
                            randText.SetString("Wrong Way.Please reset and try again.");
                            break;
                        }
                    }
                    if (commands.Contains("odd"))
                    {
                        ifIndex = commands.IndexOf("else");
                        start = ifIndex + 2;
                        end = commands.LastIndexOf("}");
                        if (commands[ifIndex + 1] == "even")
                        {
                            if (commands[start + 1] == "right")
                            {
                                for (int b = start + 1; b < end; b++)
                                {
                                    if (commands[b].Equals("up"))
                                    {
                                        commands.Insert(end + 1, commands[b]);

                                    }

                                }
                            }
                            else
                            {
                                randText.SetString("Wrong Way.Please reset and try again.");
                                break;
                            }


                        }
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
