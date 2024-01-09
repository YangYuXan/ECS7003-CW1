using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NewUI : MonoBehaviour
{
    public TextMeshProUGUI PlayerHP;
    public TextMeshProUGUI Ammo;
    public Button AttackButton;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        PlayerHP.text = "HP: "+ GetComponent<NewCharacter>().HP;
        Ammo.text = "Ammo: " + GetComponent<NewCharacter>().ammo;
    }

    public void SetPlayerAttackBehaviour()
    {
        GetComponent<NewCharacter>().behaviour = "attack";
    }

    public void SetPlayer2AttackBehaviour()
    {
        GetComponent<NewCharacter>().behaviour = "attack2";
    }
}
