using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Data
{
    #region  Stat
    [System.Serializable]
    public class Stat
    {
        public int level;
        public int maxHp;
        public int attack;
        public int totalExp;
    }

    [System.Serializable]
    public class StatData : ILoader<int, Stat>
    {
        public List<Stat> stats = new List<Stat>();

        public Dictionary<int, Stat> MakeDict()
        {
            Dictionary<int, Stat> dict = new Dictionary<int, Stat>();
            foreach (Stat stat in stats)
            {
                dict.Add(stat.level, stat);
            }

            return dict;
        }
    }

    #endregion
}