using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Enum;
using Formulas;
using Random = UnityEngine.Random;

public class AI_Gameobject : MonoBehaviour{
    //=====================================================================
    //				      VARIABLES 
    //=====================================================================
    //===== SINGLETON =====
    public static AI_Gameobject m_Instance;
    //===== STRUCT =====
    public class c_Probabilty {
        public e_State m_ChosenState;
        public float m_Weight;

        public c_Probabilty(e_State p_State, float p_Weight) {
            m_ChosenState = p_State;
            m_Weight = p_Weight;
        }
    }
    //===== PUBLIC =====
    //===== PRIVATES =====
    private e_State m_Answer;
    private List<c_Probabilty> t_States = new List<c_Probabilty>();
    private List<float> t_Probabilty = new List<float>();
    //=====================================================================
    //				MONOBEHAVIOUR METHOD 
    //=====================================================================
    void Awake(){
        m_Instance = this;
    }

    private void Start()
    {
        for (int i = 0; i < 3; i++)
        {
            t_States.Add(new c_Probabilty((e_State)i,1));
        }
    }
    //=====================================================================
    //				    OTHER METHOD
    //=====================================================================
    public void f_GetPlayerState()
    {
        t_Probabilty.Clear();
        if (GameManager_Manager.m_Instance.m_Level == e_AIDifficulty.Easy)
        {
            GameLogic_Manager.m_Instance.m_EnemyChosenState = (e_State)  Random.Range(0,3);
        }
        else if (GameManager_Manager.m_Instance.m_Level == e_AIDifficulty.Medium)
        {
            f_InsertProbabilty(4,3);
        }
        else if (GameManager_Manager.m_Instance.m_Level == e_AIDifficulty.Hard)
        {
            f_InsertProbabilty(6,2);
        }
        GameLogic_Manager.m_Instance.f_CheckPlayerWin();
    }

    public void f_InsertProbabilty(float p_CorrectProbabilty,float p_WrongProbabilty) {
        if (GameLogic_Manager.m_Instance.m_PlayerChosenState == e_State.Rock) m_Answer = e_State.Paper;
        else if (GameLogic_Manager.m_Instance.m_PlayerChosenState == e_State.Paper) m_Answer = e_State.Scissors;
        else m_Answer = e_State.Rock;

        for (int i = 0; i < t_States.Count; i++)
        {
            if (m_Answer == t_States[i].m_ChosenState) t_States[i].m_Weight = p_CorrectProbabilty;
            else t_States[i].m_Weight = p_WrongProbabilty;
            t_Probabilty.Add(t_States[i].m_Weight);
        }

        GameLogic_Manager.m_Instance.m_EnemyChosenState = t_States[Formula.f_FormulaDistributionGacha(t_Probabilty)].m_ChosenState;
    }
}
