using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Coding : MonoBehaviour
{

    
    List<string> commands = new List<string>();
    float speed = 20f;
    SceneObject scene;
    CarController car;
    int counter;
    List <Vector3> ans = new List<Vector3>();
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


    void Start()
    {   car = FindObjectOfType<CarController>();


        Debug.Log("map[4,0]:"+map[4, 0]);
        car.enabled = false;
        Update();

        
    }

    // Update is called once per frame
    void Update()
    {
       Debug.Log("Distance: "+Vector3.Distance(transform.localPosition, map[4, 0]));
    

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
        this.finish();
        this.SetWayPoint();
    }


    public void SetWayPoint()
    {

        car = FindObjectOfType<CarController>();
        flag = FindObjectOfType<Flag>();
        randText = FindObjectOfType<SetText>();
        int number;
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

            if (commands[i] == "if")
            {
                if (commands.Contains("else"))
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
                } else
                {
                    randText.SetString("There should be a else statement.");
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
                                     //finish looping all input.
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

        car.SetWayPoints(map[row, col]);
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
        Scene currentScene = SceneManager.GetActiveScene();
        if (currentScene.name == "Level3")
        {
            randText = FindObjectOfType<SetText>();
            randText.SetString("Objective:Odd Number-> Go up. Even number -> Go right.\nNumber: " + Random.Range(1, 10));
        }
        else if(currentScene.name == "Level2"){
            randText.SetString("Objective: Collect 3 flags.");
        }
        else randText.SetString("");
    }

    public void Text_Changed(string changeText)
    {
         num = int.Parse(changeText);


    }

    public void finish(){
        car = FindObjectOfType<CarController>();
        if (car.transform.localPosition == map[4,0]){
            Debug.Log("You win!");
        }
    }
}

/*  

    void Rotat()
    {
        car.transform.position = Vector3.RotateTowards(car.transform.position, (transPos - car.transform.position), speed * Time.deltaTime, 0.0f);
    }*/
