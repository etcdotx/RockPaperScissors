using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Random = UnityEngine.Random;

namespace Formulas
{
    public class Formula
    {
        //=====================================================================
        //				      VARIABLES 
        //=====================================================================
        //===== SINGLETON =====
        //===== STRUCT =====
        //===== PUBLIC =====
        //===== PRIVATES =====
        //=====================================================================
        //				    OTHER METHOD
        //=====================================================================

        public static int f_FormulaDistributionGacha(List<float> p_Probabiltys) {
            List<float> t_CumulativeProbabiltys = new List<float>();
            
            for (int i = 0; i < p_Probabiltys.Count; i++) {
                if (i == 0)
                {
                    t_CumulativeProbabiltys.Add(p_Probabiltys[i]);
                }
                else {
                    t_CumulativeProbabiltys.Add(t_CumulativeProbabiltys[i-1]+p_Probabiltys[i]);
                }             
            }

            float t_Seed = Random.Range(0, t_CumulativeProbabiltys[t_CumulativeProbabiltys.Count - 1]);

            for (int i = 0; i < t_CumulativeProbabiltys.Count; i++) {
                if (t_Seed <= t_CumulativeProbabiltys[i]) return i;
            }

            return 0;
        }

        public static bool f_RandomDistributionGacha(float p_Probabilty, float p_MaxProbabilty)
        {
            float t_Index = (Random.Range(0, p_MaxProbabilty));
            if (t_Index>= p_Probabilty) return false;
            else return true;
        }


        public static bool f_RandomDistributionGachaUsingInt(int p_Probabilty, int p_MaxProbabilty)
        {
            float t_Index = (Random.Range(0, p_MaxProbabilty + 1));
            if (t_Index >= p_Probabilty) return false;
            else return true;
        }
    }
}