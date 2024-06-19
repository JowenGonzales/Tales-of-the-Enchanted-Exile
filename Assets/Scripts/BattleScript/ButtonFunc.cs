using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ButtonFunc : MonoBehaviour
{
    [SerializeField]
    private bool physical;

    private GameObject hero;

    //Function for listening to the action buttons (attack, range)
    void Start()
    {
        string temp = gameObject.name;
        gameObject.GetComponent<Button>().onClick.AddListener(() => AttachCallBack(temp));
        hero = GameObject.FindGameObjectWithTag("Player");
    }
    //Depending on which btutton is clicked, it will play the corresponding action
    private void AttachCallBack(string btn)
    {
        Debug.Log($"Button {btn} clicked");
        if (btn.CompareTo("AttackBtn") == 0) 
        {
            Debug.Log("Attacks");
            hero.GetComponent<EnemyAction>().SelectAttack("attack");
        }
        else if (btn.CompareTo("RangeBtn") == 0)
        {
            hero.GetComponent<EnemyAction>().SelectAttack("range");
        } else if (btn.CompareTo("RunBtn") == 0)
        {
            hero.GetComponent<EnemyAction>().SelectAttack("run");
        } else
        {
            SceneManager.LoadScene(3);
        }
    }
}
