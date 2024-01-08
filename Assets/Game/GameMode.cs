using System.Collections;
using System.Collections.Generic;
using System.Linq;
using CharacterCampSpace;
using UnityEngine;
using UnityEngine.UI;

public class GameMode : MonoBehaviour
{
    public Button turnEndButton;

    public List<Character> battleQueue = new();
    //private Character[] canOperateRoles;
    private int _currentAttackIndex=0;
    public bool _isPlayerTurn=false;
    public bool _isBattle;
    public GameObject player;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /*
     *  When an NPC is injured, it will prepare to enter combat mode. Before the battle begins,
     *  all characters that meet the conditions need to enter the combat sequence. The specific conditions are:
     *  within a radius of 50 from the attacked person, and in the same camp as the attacked person.
     *  People, as well as all people of the same alignment as the attacker, will enter the order of battle.
     *
     *  @param position : The location of the attacker
     */
    public void GenerateQueue(Vector3 position, CharacterCamp camp,GameObject inital )
    {
        for(int i=0;i<FindObjectsOfType<Character>().Length;i++)
        {
            if (Vector3.Distance(position, 
                    FindObjectsOfType<Character>()[i].transform.position)<1)
            {
                battleQueue.Add(FindObjectsOfType<Character>()[i]);
            }
            /*
            if (FindObjectsOfType<Character>()[i]._isPlayPawn&& Vector3.Distance(position,
                    FindObjectsOfType<Character>()[i].transform.position) >= 50)
            {
                battleQueue.Add(FindObjectsOfType<Character>()[i]);
            }*/
            battleQueue.Add(player.GetComponent<Character>());
        }
    }

    //生成目标队列
    public void GenerateHateTarget(CharacterCamp selfCamp,GameObject Enermy)
    {
        for (int i = 0; i < battleQueue.Count; i++)
        {
            if (battleQueue[i].camp == selfCamp)
            {
                battleQueue[i].hateTarget = Enermy;
            }
        }
    }


    public void CalcAttackOrder()
    {
        //Sort combatable characters in order of agility from high to low
        battleQueue.Sort((a,b)=>b.speed.CompareTo(a.speed));

        //Determine whether the character in the starting round is the player's turn
        if (battleQueue[0]._isPlayPawn)
        {
            _isPlayerTurn = true;
            turnEndButton.interactable = true;
        }

        for (int i = 0; i < battleQueue.Count; i++)
        {

            //Assign attack order to characters
            battleQueue[i].attackOrder = i;

            //Let the first character gain the right to act
            if (i != 0)
            {
                battleQueue[i].SetCanOperate(false);
                
            }
            
        }
        //When first is not controlled by the player, let first start using AiStrategy()
        if (!battleQueue[0]._isPlayPawn)
        {
            battleQueue[0].AiStrategy();
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

    public void SetAttackIndex(int index)
    {
        _currentAttackIndex = index;
    }

    public int GetCurrentAttackIndex()
    {
        return _currentAttackIndex;
    }

    /*
     *  At the end of the round, switch to the next character
     */
    public void SwitchNextCharacter()
    {
        //Handle UI effects
        if (_isPlayerTurn)
        {
            turnEndButton.interactable = false;
            battleQueue[_currentAttackIndex].AttackButton.interactable = false;
        }

        //The current settlement character is set to be immovable.
        battleQueue[_currentAttackIndex]._canOperate = false;


        
        if (_currentAttackIndex+1 > battleQueue.Count-1)
        {
            _currentAttackIndex = 0;
        }
        _currentAttackIndex++;

        //Get the next character in the queue who can fight
        for (int i = _currentAttackIndex; i < battleQueue.Count; i++)
        {
            //If the character does not fall to the ground or dies, it complies with the action.

            if (!battleQueue[i].GetDeath() && !battleQueue[i].GetNearDeath())
            {
                if (battleQueue[i]._isPlayPawn)
                {
                    _isPlayerTurn = true;
                    turnEndButton.interactable=true;
                    battleQueue[i].AttackButton.interactable = true;
                }
                else
                {
                    _isPlayerTurn = false;


                }

                battleQueue[i].MaxMoveDistance = 20;
                battleQueue[i].RemainMoveDistance = 20;
                battleQueue[i].totalDistance = 0;
                battleQueue[i]._canOperate = true;
                if (battleQueue[i].fireBomb != null && battleQueue[i].freezeBomb != null &&
                    battleQueue[i].AddHp != null)
                {
                    battleQueue[i].fireBomb.interactable = true;
                    battleQueue[i].freezeBomb.interactable = true;
                    battleQueue[i].AddHp.interactable = true;
                    battleQueue[i].cardLimited = 2;
                }
                
                battleQueue[i].AiStrategy();

                break;
            }
        }
    }

    /*
     * When all enemies are eliminated, exit combat mode
     */
    public void LeaveBattle()
    {
        if (battleQueue.Count == 1)
        {
            if (battleQueue[0]._isPlayPawn)
            {
                _isBattle = false;
                battleQueue[0].attackOrder = -1;
                battleQueue[0]._canOperate = true;
                battleQueue[0].UI_RemainMoveDistance.text = "";
                battleQueue[0].UI_MaxMoveDistance.text = "";
                battleQueue[0].fireBomb.interactable = true;
                battleQueue[0].freezeBomb.interactable = true;
                battleQueue[0].AddHp.interactable = true;
                battleQueue[0].cardLimited = 2;
                turnEndButton.interactable = false;
                battleQueue[0].AttackButton.interactable = true;
                print("end battle");
                PathFound pathFound = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<PathFound>();
                pathFound.moveMode = PathFound.MoveMode.Normal;
            }
            
        }
    }

    public void RestoreToinitialState(Character character)
    {

    }
}
