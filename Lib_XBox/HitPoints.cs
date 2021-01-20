using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using XNALib;

namespace XNALib
{
    public class HitPoints
    {
        
        private float m_MinHP = 0;
        public float MinHP
        {
            get { return m_MinHP; }
            set
            {
                if (value > MaxHP)
                    throw new Exception("");

                m_MinHP = value;

                if (CurrentHP < value)
                    CurrentHP = value;                        
            }
        }
       
        private float m_CurrentHP ;
        public float CurrentHP
        {
            get { return m_CurrentHP; }
            set { m_CurrentHP = MathHelper.Clamp(value, MinHP, MaxHP); }
        }

        private float m_MaxHP ;
        public float MaxHP
        {
            get { return m_MaxHP; }
            set
            {
                m_MaxHP = value;
                if (CurrentHP > value)
                    CurrentHP = value;
            }
        }

        /// <summary>
        /// Percentage from 0-100
        /// </summary>
        public float LifeLeftPercentage { get { return (CurrentHP / (float)MaxHP) * 100; } }
        /// <summary>
        /// Percentage from 0-100
        /// </summary>
        public float LifeLostPercentage { get { return MaxHP - LifeLeftPercentage; } }

        public HitPoints(float maxHP)
        {
            CurrentHP = MaxHP = maxHP;
        }

        public HitPoints(float maxHP, float currentHP, float minHP)
        {
            CurrentHP = MaxHP = maxHP;
            MinHP = minHP;
            CurrentHP = currentHP;
        }

        public void HealFull()
        {
            CurrentHP = MaxHP;
        }
    }
}
