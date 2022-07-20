using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Enum;
public class GameLogic_Manager : MonoBehaviour{
    //=====================================================================
    //				      VARIABLES 
    //=====================================================================
    //===== SINGLETON =====
    public static GameLogic_Manager m_Instance;
    //===== STRUCT =====
    //===== PUBLIC =====
    public e_State m_PlayerChosenState;
    public e_State m_EnemyChosenState;
    public e_ChooseState m_ChooseState = e_ChooseState.NotChoosing;
    public c_Event m_WaitEvent;
    public c_Event m_AfterShuffle;
    //===== PRIVATES =====
    private e_Result m_Result;
    //=====================================================================
    //				MONOBEHAVIOUR METHOD 
    //=====================================================================
    void Awake(){
        m_Instance = this;
    }

    void Start(){
        
    }

    void Update(){
        
    }
    //=====================================================================
    //				    OTHER METHOD
    //=====================================================================

    public void f_GetPlayerInput(int p_PlayerID,e_State p_ChosenState) {
        if (p_PlayerID == 1) m_PlayerChosenState = p_ChosenState;
        else m_EnemyChosenState = p_ChosenState;

        if (GameManager_Manager.m_Instance.m_GameMode == e_Mode.Single)
        {
            AI_Gameobject.m_Instance.f_GetPlayerState();

        }
        else if (GameManager_Manager.m_Instance.m_GameMode == e_Mode.Multi) {
            if ((int)m_PlayerChosenState > -1 && (int)m_EnemyChosenState > -1) {
                f_CheckPlayerWin();
            }
        }
        m_ChooseState = e_ChooseState.NotChoosing;
    }

    public void f_CheckPlayerWin() {
        UI_Manager.m_Instance.m_Timer.gameObject.SetActive(false);
        if (m_PlayerChosenState == m_EnemyChosenState) {
            m_Result = e_Result.Draw;
        }
        else
        {
            if (((m_PlayerChosenState == e_State.Rock) && (m_EnemyChosenState == e_State.Scissors)) ||
               ((m_PlayerChosenState == e_State.Scissors) && (m_EnemyChosenState == e_State.Paper)) ||
               ((m_PlayerChosenState == e_State.Paper) && (m_EnemyChosenState == e_State.Rock)))
            {
                m_Result = e_Result.Win;
            }
            else if (((m_EnemyChosenState == e_State.Rock) && (m_PlayerChosenState == e_State.Scissors)) ||
              ((m_EnemyChosenState == e_State.Scissors) && (m_PlayerChosenState == e_State.Paper)) ||
              ((m_EnemyChosenState == e_State.Paper) && (m_PlayerChosenState == e_State.Rock)))
            {
                m_Result = e_Result.Lose;
            }
        }

        StartCoroutine(ie_WaitForAnimation());
    }


    IEnumerator ie_WaitForAnimation() {
        if (GameManager_Manager.m_Instance.m_GameMode == e_Mode.Multi) GameManager_Manager.m_Instance.f_StartShuffling();
        GameManager_Manager.m_Instance.m_AnimPlayer.SetBool("Shuffle", true);
        GameManager_Manager.m_Instance.m_AnimEnemy.SetBool("Shuffle", true);
        m_WaitEvent?.Invoke();
        yield return new WaitForSeconds(2f);
        GameManager_Manager.m_Instance.m_AnimPlayer.SetBool("Shuffle", false);
        GameManager_Manager.m_Instance.m_AnimEnemy.SetBool("Shuffle", false);
        GameManager_Manager.m_Instance.m_AnimPlayer.SetInteger("State", (int) m_PlayerChosenState);
        GameManager_Manager.m_Instance.m_AnimEnemy.SetInteger("State", (int)m_EnemyChosenState);
        yield return new WaitForSeconds(2f);
        m_AfterShuffle?.Invoke();
        f_CheckResult();
    }

    public void f_CheckResult() {
        if (m_Result == e_Result.Win)
        {
            if (GameManager_Manager.m_Instance.m_GameMode == e_Mode.Single) GameManager_Manager.m_Instance.f_IncreaseScore();
            else
            {
                GameManager_Manager.m_Instance.m_HostScore++;
                GameManager_Manager.m_Instance.f_UpdateRound();
                
                if (GameManager_Manager.m_Instance.m_HostScore >= 3)
                {
                    GameManager_Manager.m_Instance.f_LoseEvent();
                    GameManager_Manager.m_Instance.f_Win();
                }
                else {
                    GameManager_Manager.m_Instance.m_OnFinishEvent?.Invoke();
                }
            }
        }
        else if (m_Result == e_Result.Lose)
        {
            if (GameManager_Manager.m_Instance.m_GameMode == e_Mode.Single) GameManager_Manager.m_Instance.f_DecreaseLive();
            else
            {
                GameManager_Manager.m_Instance.m_ClientScore++;
                GameManager_Manager.m_Instance.f_UpdateRound();
                if (GameManager_Manager.m_Instance.m_ClientScore >= 3)
                {
                    GameManager_Manager.m_Instance.f_WinEvent();
                    GameManager_Manager.m_Instance.f_Lose();
                }
                else
                {
                    GameManager_Manager.m_Instance.m_OnFinishEvent?.Invoke();
                }
            }
        }
        else
        {
            GameManager_Manager.m_Instance.m_OnFinishEvent?.Invoke();
            if(GameManager_Manager.m_Instance.m_GameMode == e_Mode.Multi) GameManager_Manager.m_Instance.f_UpdateRound();
        }
    }

    public void f_ResetPlayerInput() {
        m_PlayerChosenState = e_State.Choosing;
        m_EnemyChosenState = e_State.Choosing;
        m_ChooseState = e_ChooseState.Choosing;
    }
}
