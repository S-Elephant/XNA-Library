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
    public class PooledCue : IXNADispose
    {

        private Cue m_TheCue;
        public Cue TheCue
        {
            get { return m_TheCue; }
            private set { m_TheCue = value; }
        }
       

       private bool m_IsDisposed ;
       public bool IsDisposed
       {
           get { return m_IsDisposed; }
           set { m_IsDisposed = value; }
       }

       public PooledCue()
       {
           throw new Exception("Only the pool is allowed to create new instances of me.");
       }

       public void Play()
       {
           TheCue.Play();
       }

       private PooledCue(SoundBank sb, string sound)
       {
           TheCue = sb.GetCue(sound);
       }

       public static PooledCue PoolConstructor(SoundBank sb, string sound)
       {
           return new PooledCue(sb, sound);
       }
    }
    [Obsolete("WARNING not finished!! Does not work yet")]
    public class XactMgrPooled
    {
        AudioEngine AudioEngine;
        WaveBank WaveBank;
        SoundBank SoundBank;

        public List<Pool<PooledCue>> CuePool = new List<Pool<PooledCue>>(15);
        List<PooledCue> CuesInUse = new List<PooledCue>(100);

        public XactMgrPooled(string name)
        {
            string folder = "Audio";
            AudioEngine = new AudioEngine(string.Format(".\\Content\\{0}\\{1}.xgs", folder, name));
            WaveBank = new WaveBank(AudioEngine, string.Format(".\\Content\\{0}\\Wave Bank.xwb", folder));
            SoundBank = new SoundBank(AudioEngine, string.Format(".\\Content\\{0}\\Sound Bank.xsb", folder));
        }

        /// <summary>
        /// Adds a new pool of sounds
        /// </summary>
        /// <param name="sound"></param>
        /// <param name="poolSize"></param>
        public void AddSound(string sound, int poolSize)
        {
            CuePool.Add(new Pool<PooledCue>(poolSize, true, s => !s.IsDisposed, () => PooledCue.PoolConstructor(SoundBank, sound)));
        }

        public void PlaySound(int soundIdx)
        {
            PooledCue pc = CuePool[soundIdx].New();
            pc.Play();
            CuesInUse.Add(pc);
        }

        public void Update()
        {
            for (int i = 0; i < CuesInUse.Count; i++)
            {
                if (CuesInUse[i].TheCue.IsStopped)
                {
                    CuesInUse[i].IsDisposed = true;
                    CuesInUse.RemoveAt(i);
                    i--;
                }
            }
            for (int i = 0; i < CuePool.Count; i++)
                CuePool[i].CleanUp();       
        }
    }
}
