using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Enum;
public class Player_Gameobject : MonoBehaviour{
    //=====================================================================
    //				      VARIABLES 
    //=====================================================================
    //===== SINGLETON =====
    //===== STRUCT =====
    //===== PUBLIC =====
    public int m_PlayerID = 1;
    private e_State t_ChosenState;
    //===== PRIVATES =====

    //=====================================================================
    //				MONOBEHAVIOUR METHOD 
    //=====================================================================
    void Start(){
        
    }

    void Update(){
        
    }
    //=====================================================================
    //				    OTHER METHOD
    //=====================================================================
    public void f_ChooseState(int p_ChoosenState) {
        t_ChosenState = (e_State) p_ChoosenState;
        UI_Manager.m_Instance.m_PlayerInput.gameObject.SetActive(false);
        if (GameManager_Manager.m_Instance.m_GameMode == e_Mode.Single)
        {
            GameLogic_Manager.m_Instance.f_GetPlayerInput(m_PlayerID, t_ChosenState);
        }
        else {
            if (Photon.Pun.PhotonNetwork.IsMasterClient)
            {
                GameLogic_Manager.m_Instance.f_GetPlayerInput(m_PlayerID, t_ChosenState);
            }
            else { 
                GameLogic_Manager.m_Instance.m_ChooseState = e_ChooseState.NotChoosing;
                GameManager_Manager.m_Instance.f_ChooseEvent(t_ChosenState);
            }
        }
    }
}
