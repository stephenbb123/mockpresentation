using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Level3 : MonoBehaviour
{


    List<string> commands = new List<string>();
    float speed = 1f;
    SceneObject scene;
    CarController car;
    int counter;
    List<string> userInput = new List<string>();
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
        randText = FindObjectOfType<SetText>();
            num = randText.GetNum();
        
            if (row < 0 || col < 0)
            {
                code.Crashed();
            }
        if (Vector3.Distance(map[3, 0], car.transform.localPosition) < 0.1f|| Vector3.Distance(map[1,3], car.transform.localPosition) < 0.1f)
        {
            randText = FindObjectOfType<SetText>();
            randText.SetString("Congratulations! You won!\n The number is:"+num);
        }
        if (Vector3.Distance(car.transform.localPosition, Vector3.zero) < 0.1f)
        {
            car.Clear();
            randText = FindObjectOfType<SetText>();
            randText.SetString("Crashed! Please try again.");
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
        num = randText.GetNum();

        for (int i = 0; i < commands.Count; i++) //loop all input.
        {
            if (num % 2 == 1)//if odd number
            {
                int ifIndex = commands.IndexOf("if");
                if (commands[ifIndex + 1].Equals("odd"))
                {
                    if (commands[ifIndex + 3].Equals("up"))
                    {
                        start = commands.IndexOf("{");
                        end = commands.IndexOf("}");
                        number = end+1;
                        for (int n = 0; n < end; n++)
                        {
                           userInput.Add(commands[n]);
                           
                        }
                       /* for (int y = end+1; y < commands.Count; y++)
                        {   
                            //commands.Insert(number + 1, commands[y]);
                            commands.RemoveAt(number);
                        }*/
                        
                    }
                    else
                    {
                        randText.SetString("You got a wrong way.1");

                    }
                }
                else if (commands[ifIndex + 1].Equals("even"))
                {
                    if (commands[ifIndex + 3].Equals("right"))
                    {
                        start = commands.IndexOf("else");
                        end = commands.LastIndexOf("}");
                        number = end;
                        for (int n = start+1; n < end; n++)
                        {
                         userInput.Add(commands[n]);
                        }
                        /*for (int y = ifIndex; y < start; y++)
                        {
                            //commands.Insert(number + 1, commands[y]);
                            commands.RemoveAt(ifIndex);
                        }*/
                    }
                    else
                    {
                        randText.SetString("You got a wrong way.2");
                    }
                }
                else
                {
                    randText.SetString("You got a wrong way.3");
                }
            }

            if (num % 2 == 0)//if even number
            {
                int ifIndex = commands.IndexOf("if");
                if (commands[ifIndex + 1].Equals("even"))
                {
                    if (commands[ifIndex + 3].Equals("right"))
                    {
                        start = commands.IndexOf("{");
                        end = commands.IndexOf("else");
                        number = end;
                        for(int n = 0; n < end; n++)
                        {
                            userInput.Add(commands[n]);
                        }
                        break;
                        /*for (int y = end; y < commands.Count; y++)
                        {
                            //commands.Insert(number + 1, commands[y]);
                            commands.RemoveAt(number);
                        }*/
                    }
                    else
                    {
                        randText.SetString("You got a wrong way.4");
                    }
                }
                else if (commands[ifIndex + 1].Equals("odd"))
                {
                    if (commands[ifIndex + 3].Equals("up"))
                    {
                        start = commands.IndexOf("else");
                        end = commands.LastIndexOf("}");
                        number = end;
                        for(int n = 0; n < start; n++)
                        {
                            userInput.Add(commands[n]);
                        }
                        break;
                        /*for (int y = ifIndex; y < start; y++)
                        {
                            //commands.Insert(number + 1, commands[y]);
                            commands.RemoveAt(ifIndex);
                        }*/
                    }
                    else
                    {
                        randText.SetString("You got a wrong way.5");
                    }
                }
                else
                {
                    randText.SetString("You got a wrong way.6");
                }
            }
        }




        for(int i = 0; i < userInput.Count; i++)
        {
            Debug.Log("userInput command: "+userInput[i]);
            switch (userInput[i])    //if there is no while in the user input
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
/* if (commands[i] == "if")
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
            }*/
