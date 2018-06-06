using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Level1 : MonoBehaviour
{
    List<string> commands = new List<string>();
    CarController car;
    int row;
    int col;
    int num;
    Flag flag;
    SetText randText;
    public Vector3[,] map = { {new Vector3 (-420f, -137f, -165f), new Vector3(),new Vector3 (),new Vector3 () } ,
                                            { new Vector3(-514f, -137f, -165f), new Vector3(-514f, -137f, -63f), new Vector3(-514f, -137f, 64f),new Vector3 (-514f, -137f, 148f) },
                                            { new Vector3(-608f, -137f, -165f), new Vector3(), new Vector3(),new Vector3 (-608f, -137f, 148f) },
                                            { new Vector3(-702f, -137f, -165f), new Vector3(-702f, -137f, -63f),new Vector3 (-702f, -137f, 64f),new Vector3 (-702f, -137f, 148f) } ,
                                            { new Vector3(-796f, -137f, -165f), new Vector3(), new Vector3(),new Vector3 () }};
    Coding code;
    // Use this for initialization
    void Start()
    {
        car = FindObjectOfType<CarController>();
        car.enabled = false;
       
    }

    // Update is called once per frame
    void Update()
    {
        code = FindObjectOfType<Coding>();
        car = FindObjectOfType<CarController>();
        if (row < 0 || col < 0)
        {
            code.Crashed();
        }
        if (Vector3.Distance(car.transform.localPosition, map[2, 0]) < 0.1f)
        {
            randText = FindObjectOfType<SetText>();
            randText.SetString("Crashed. Please try again.");
            car.Clear();
            
        }
        if(Vector3.Distance(car.transform.localPosition, Vector3.zero) < 0.1f)
        {
            randText = FindObjectOfType<SetText>();
            randText.SetString("Crashed. Please try again.");
            car.Clear();
        }
    }

    
    public void SetWayPoint()
    {
        code = FindObjectOfType<Coding>();
        commands = code.Read();
        car = FindObjectOfType<CarController>();
        flag = FindObjectOfType<Flag>();
        randText = FindObjectOfType<SetText>();
        for (int i = 0; i < commands.Count; i++) //loop all input.
        {
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
              

                case "default":
                    break;
            }
        }
        //finish looping all input.
        car.enabled = true;
        //car.Movement();
    }


    public void Clear()
    {
        code = FindObjectOfType<Coding>();
        code.Clear();
    }
}
