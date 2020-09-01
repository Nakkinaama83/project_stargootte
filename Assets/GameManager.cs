using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public List<GameObject> squad;
    public int soldiers;
    public int techs;
    public int scientists;


    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(this);        
        squad = new List<GameObject>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void NewLevel()
    {
        foreach (var player in squad)
        {
            FindObjectOfType<NewSceneHandler>().squad.Add(player);
        }
        FindObjectOfType<NewSceneHandler>().MovePlayersToShip();

    }
}
