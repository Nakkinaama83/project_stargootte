using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewSceneHandler : MonoBehaviour
{
    public List<GameObject> squad;

    private void Awake()
    {
        squad = new List<GameObject>();
        FindObjectOfType<GameManager>().NewLevel();
    }

    // Start is called before the first frame update
    void Start()
    {
        //squad = new List<GameObject>(GameObject.FindGameObjectsWithTag("Player"));
        

    }

    public void MovePlayersToShip()
    {
        foreach (var player in squad)
        {
            float angle = Random.Range(0.0f, Mathf.PI * 2); //laskee ympyrästä randomi luvun asteina
            Vector3 v = new Vector3(Mathf.Sin(angle), 0, Mathf.Cos(angle)); //laskee koordinaatit x,y,z joista x ja z on randomeita
            v *= 3;
            player.transform.position = gameObject.transform.position - v;
        }
    }

    //// Update is called once per frame
    //void Update()
    //{
        
    //}
}
