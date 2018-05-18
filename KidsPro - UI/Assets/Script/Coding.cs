using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Vuforia;

public class Coding : MonoBehaviour
{
    List<string> commands = new List<string>();
    Vector3 transPos;
    public float speed = 4f;

    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        

    }

    public void start(GameObject gameObj)
    {
        Debug.Log("GameObject to be moved :"+gameObj);
        commands.Clear();
        transPos = gameObj.transform.position;
        Debug.Log("TransPos:"+transPos);
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

        foreach (string command in commands)
        {
            string stat = command;
            if (command.Contains("(Clone)"))
            { 
            int index = command.IndexOf("(");
            stat = command.Substring(0, index);
                Debug.Log(stat);
            }   

            switch (stat)
            {
                case "up" :
                    transPos.x = gameObj.transform.position.x + 100;
                    gameObj.transform.position = Vector3.MoveTowards(gameObj.transform.position, transPos , speed * Time.deltaTime);
                    Debug.Log("TransPos:" +transPos + " , position:" + gameObj.transform.position);
                    break;

                case "down":
                    transPos.x = gameObj.transform.position.x - 100;
                    gameObj.transform.position = Vector3.MoveTowards(gameObj.transform.position, transPos, speed * Time.deltaTime);
                    break;

            }
        }
    }

}
