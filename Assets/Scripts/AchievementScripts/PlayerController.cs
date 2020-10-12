using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public DataHandler d;

    int Val;

    public Text LevelTxt;

    private void Start()
    {

        Invoke("LoadLevel", 1f);
        /*
        if(d.LevelBool == true)
        {
            d.playData.achievements[d.LevelId].Done = d.playData.level;
        }
        */
    }

    public void LoadLevel()
    {
        LevelTxt.text = d.playData.level.ToString();
    }

    //INPUT FOR JUMP
    public void JumpAction()
    {
        Debug.Log("JUMP ACTION");
        CheckAchieve("Jump");
    }

    //INPUT FOR LEVEL
    public void LevelAction()
    {
        d.playData.level += 1;
        LevelTxt.text = d.playData.level.ToString();
        Debug.Log("LEVEL ACTION");
        CheckAchieve("Level");
    }

    //INPUT FOR ATTACK
    public void AttackAction()
    {
        Debug.Log("ATTACK ACTION");
        CheckAchieve("Attack");
    }

    public void DieAction()
    {
        Debug.Log("DIE ACTION");
        CheckAchieve("Die");
    }

    public void KillAction()
    {
        Debug.Log("KILL ACTION");
        CheckAchieve("Kill");
    }

    public void CheckAchieve(string Type)
    {
        switch (Type)
        {
            case "Jump":
                Debug.Log("JUMPEVENT " + d.playData.achievements[d.JumpId].State + " " + d.playData.achievements[d.JumpId].Done);
                if (d.playData.achievements[d.JumpId].State == false)
                {
                    Debug.Log("JUMP_OPEN " + d.playData.achievements[d.JumpId].State);
                    d.playData.achievements[d.JumpId].Done++;
                    LoadAchieve();
                    if (d.playData.achievements[d.JumpId].Done >= d.JumpTarget)
                    {
                        d.FireJump(true);//firing event
                        
                    }
                }

                break;
            case "Level":
                Debug.Log("LEVEL " + d.playData.achievements[d.LevelId].State);
                if (d.playData.achievements[d.LevelId].State == false)
                {
                    d.playData.achievements[d.LevelId].Done = d.playData.level;
                    LoadAchieve();
                    if (d.playData.achievements[d.LevelId].Done >= d.LevelTarget)
                    {
                        d.FireLevel(true);//firing event
                        
                    }
                }

                break;
            case "Attack":
                Debug.Log("Attack " + d.playData.achievements[d.AttackId].State);
                if (d.playData.achievements[d.AttackId].State == false)
                {
                    d.playData.achievements[d.AttackId].Done++;
                    LoadAchieve();
                    if (d.playData.achievements[d.AttackId].Done >= d.AttackTarget)
                    {
                        d.FireAttack(true);//firing event
                    }
                }
                break;
            case "Die":
                Debug.Log("Die " + d.playData.achievements[d.DieId].State);
                if (d.playData.achievements[d.DieId].State == false)
                {
                    d.playData.achievements[d.DieId].Done++;
                    LoadAchieve();
                    if (d.playData.achievements[d.DieId].Done >= d.DieTarget)
                    {
                        d.FireDie(true);//firing event
                    }
                }
                break;
            case "Kill":
                Debug.Log("Kill " + d.playData.achievements[d.KillId].State);
                if (d.playData.achievements[d.KillId].State == false)
                {
                    d.playData.achievements[d.KillId].Done++;
                    LoadAchieve();
                    if (d.playData.achievements[d.KillId].Done >= d.KillTarget)
                    {
                        d.FireKill(true);//firing event
                    }
                }
                break;
        }
    }

    //For Loading   
    public void LoadAchieve()
    {
        d.LoadAchievements();
    }


    //For Saving
    public void OnDisable()
    {
        d.ObjectToJson();

        if(d.JumpBool == true)
        {
            d.JumpEvent -= d.JumpMethod;
        }

        if (d.LevelBool == true)
        {
            d.LevelEvent -= d.LevelMethod;
        }

        if (d.AttackBool == true)
        {
            d.AttackEvent -= d.AttackMethod;
        }

        if (d.DieBool == true)
        {
            d.DieEvent -= d.DieMethod;
        }

        if (d.KillBool == true)
        {
            d.KillEvent -= d.KillMethod;
        }
    }




}
