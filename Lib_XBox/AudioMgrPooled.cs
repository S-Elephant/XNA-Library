using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using XNALib;
using Microsoft.Xna.Framework.Content;

namespace XNALib
{
    public class AudioMgrPooled
    {
        List<Pool<SoundEffectInstance>> SoundPool = new List<Pool<SoundEffectInstance>>();
        public static string Folder = "Audio/";

        public static AudioMgrPooled Instance;
        public List<SoundEffectInstance> InstancesForCleanup = new List<SoundEffectInstance>();

        public AudioMgrPooled()
        {
            Instance = this;
        }

        private static SoundEffectInstance PoolConstructor(string sound)
        {
            SoundEffect se = Global.Content.Load<SoundEffect>(Folder + sound);
            SoundEffectInstance sei = se.CreateInstance();
            AudioMgrPooled.Instance.InstancesForCleanup.Add(sei);
            sei.IsLooped = false;
            return sei;
        }

        public void AddSound(string sound, int poolSize)
        {
            SoundPool.Add(new Pool<SoundEffectInstance>(poolSize, true, s => s.State == SoundState.Playing, () => PoolConstructor(sound)));
        }

        public void PlaySound(int index)
        {
            SoundEffectInstance sei = SoundPool[index].New();
            sei.Play();
        }

        public void Update(GameTime gameTime)
        {
            for (int i = 0; i < SoundPool.Count; i++)
                SoundPool[i].CleanUp();
        }

        /// <summary>
        /// Call this before the game has exited.
        /// </summary>
        public void DisposeAll()
        {
            for (int i = 0; i < InstancesForCleanup.Count; i++)
            {
                InstancesForCleanup[i].Stop();
                InstancesForCleanup[i].Dispose();
            }
            InstancesForCleanup.Clear();
            SoundPool.Clear();
        }
    }
}