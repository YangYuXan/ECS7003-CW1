using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMode : MonoBehaviour
{
    private List<GameObject> battleQueue = new List<GameObject>();
    private Character determineRole;

    private bool _isBattle;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GenerateQueue()
    {
        for(int i=0;i<FindObjectsOfType<Character>().Length;i++)
        {
            if (Vector3.Distance(determineRole.transform.position, 
                    FindObjectsOfType<Character>()[i].transform.position)<50&&
            FindObjectsOfType<Character>()[i].camp==determineRole.camp)
            {
                battleQueue.Insert(i,FindObjectsOfType<Character>()[i].gameObject);
            }
        }

    }

    public void SetBattle(bool battle)
    {
        _isBattle = true;
    }

    public bool GetBattleStatus()
    {
        return _isBattle;
    }
}
