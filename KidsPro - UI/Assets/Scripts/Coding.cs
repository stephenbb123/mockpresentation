using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class Coding : MonoBehaviour
{

    
    List<string> commands = new List<string>();
    float speed = 20f;
    public Vector3[,] map = { {new Vector3 (-420f, -137f, -165f), new Vector3(),new Vector3 (),new Vector3 () } ,
                                            { new Vector3(-514f, -137f, -165f), new Vector3(-514f, -137f, -63f), new Vector3(-514f, -137f, 64f),new Vector3 (-514f, -137f, 148f) },
                                            { new Vector3(-608f, -137f, -165f), new Vector3(), new Vector3(),new Vector3 (-608f, -137f, 148f) },
                                            { new Vector3(-702f, -137f, -165f), new Vector3(-702f, -137f, -63f),new Vector3 (-702f, -137f, 64f),new Vector3 (-702f, -137f, 148f) } ,
                                            { new Vector3(-796f, -137f, -165f), new Vector3(), new Vector3(),new Vector3 () }};
    CarController car;
    int counter;
    List <Vector3> ans = new List<Vector3>();
    int row;
    int col;
    int num;
    Crashed crash;
    Flag flag;

   

    void Start()
    {   car = FindObjectOfType<CarController>();
        row =0;
        col = 0;
        car.enabled = false;
        for (int i = 0; i < 5;i++){

            ans.Add(map[i, 0]);
        }
        
    }

    // Update is called once per frame
    void Update()
    {   
       if(row<0 || col < 0)
        {
            crash = FindObjectOfType<Crashed>();
            crash.CallCrash();
            
        }
    }

    public void Read()
    {  
        string stat;

        foreach (Transform child in gameObject.transform)
        {   
            foreach (Transform grandChild in child)
            {
                if (grandChild != null)
                {
                  
                    stat = grandChild.name;
                    

                    if (grandChild.name.Contains("(Clone)"))
                    {
                        int index = grandChild.name.IndexOf("(");
                        stat = grandChild.name.Substring(0, index);
                    }
                    
                        commands.Add(stat);
                        
                    
                    
                    Debug.Log("Count:" + commands.Count);
                }
            }
        }

        this.SetWayPoint();

    }


    public void SetWayPoint()
    {
        
        car = FindObjectOfType<CarController>();
        flag = FindObjectOfType<Flag>();
        int start = 0;
        int end = 0;
        for (int i = 0; i < commands.Count; i++) //loop all input.
        {
            if (commands[i] == "while")   //do following loop if input contains while.
            {

                start = commands.IndexOf("{");
                end = commands.IndexOf("}");
                Debug.Log("Start index:" + start + ", end " + end);
                for (counter = 0; counter < num; counter++)
                {
                    for (int y = start+1; y < end; y++)
                    {
                        commands.Insert(commands.IndexOf("}")+1, commands[y]);
                        //commands.Add(commands[y]);
                        Debug.Log("Added");
                    }
                }
            }
        }

        for (int i = 0; i < commands.Count; i++) {

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

                case "if":
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
            }                               //finish looping all input.
        car.enabled = true;
        //car.Movement();
    }



    public List<string> GetCommands(){
        return commands;
    }

    public Vector3[,] GetCoord(){
        return map;
    }

    public List<Vector3> GetAns(){
        return ans;
    }

    public void Clear(){
        row = 0;
        col = 0;
        commands.Clear();
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

    public void Text_Changed(string changeText)
    {
         num = int.Parse(changeText);
        Debug.Log(num);

    }

}

/*  

    void Rotat()
    {
        car.transform.position = Vector3.RotateTowards(car.transform.position, (transPos - car.transform.position), speed * Time.deltaTime, 0.0f);
    }*/
