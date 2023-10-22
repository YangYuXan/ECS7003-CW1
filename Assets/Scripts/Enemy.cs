// using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public enum ActionType
{
    Attack,
}

public class Enemy : MonoBehaviour
{
    // Enemy action
    public ActionType type;
    // UI
    public GameObject hpItemObj;
    public Text hpTxt;
    public Image hpImg;

    // status
    public int MaxHp;
    public int CurHp;

    // On select render
    SkinnedMeshRenderer _meshRenderer;

    // Start is called before the first frame update
    void Start()
    {
        hpItemObj = UIManager.Instance.CreateHpItem();
        hpTxt = hpItemObj.transform.Find("hpText").GetComponent<Text>();
        hpImg = hpItemObj.transform.Find("fill").GetComponent<Image>();
        CurHp = 3;
        MaxHp = CurHp;
        // Set health bar
        hpItemObj.transform.position = Camera.main.WorldToScreenPoint(transform.position) + Vector3.up * 20f;

        UpdateHp();
    }

    // Update is called once per frame
    void Update()
    {
        //hpItemObj.transform.position = Camera.main.WorldToScreenPoint(transform.position) + Vector3.up * 20f;

        //UpdateHp();
    }

    // Update Health
    public void UpdateHp()
    {
        hpTxt.text = CurHp + "/" + MaxHp;
        hpImg.fillAmount = (float)CurHp / (float)MaxHp;
    }

    int i = 0;
    // On select
    public void OnSelect()
    {
        
        // _meshRenderer.material.SetColor("_OtlColor", Color.red);
        Debug.Log(this.gameObject.name + i++);
    }

    // Unselect
    public void OnUnSelect()
    {
        // _meshRenderer.material.SetColor("_OtlColor", Color.black);
    }

    public void Hit(int val)
    {
        
        CurHp -= val;
        if (CurHp <= 0)
        {
            CurHp = 0;

            Destroy(gameObject, 1);
            Destroy(hpItemObj);
            EnemyManager.Instance.DeleteEnemy(this);
        }
        else
        {
            //ÊÜÉË
            Debug.Log("hit");
        }
        
        // Update UI
        UpdateHp();
    }

    // Enemy action
    public IEnumerator DoAction()
    {

        Debug.Log("DoAction " + type);
        // ActionType type = (ActionType)(Random.Range(1, 2));
        switch (type)
        {
            case ActionType.Attack:
                
                // Wait 0.5s
                yield return new WaitForSeconds(0.5f);
                // Shake camera
                // Camera.main.DOShakePosition(0.1f, 0.2f, 5, 45);
                // Get player hit
                BattleManager.Instance.GetPlayerHit(3);
                break;
        }
        // Wait 1s
        yield return new WaitForSeconds(1);

    }

}
