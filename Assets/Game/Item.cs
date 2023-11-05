using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public  enum BuffType
    {
        AddHP,
        AddSpeed,
        AddAttackValue
    }

    public BuffType bufftype;
}
