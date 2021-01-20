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

    /// <summary>
    /// When you hear no sound after playing that particular sound once then you probably forgot to call the Update(GameTime gameTime);
    /// </summary>
    public class AudioMgrPooled
    {
        List<Pool<SoundEffectInstance>> SoundPool = new List<Pool<SoundEffectInstance>>();
        public static string Folder = "Audio/";

        public static AudioMgrPooled Instance;
        public List<SoundEffectInstance> InstancesForCleanup = new List<SoundEffectInstance>();
        const int MAX_UNIQUE_SOUNDS_PER_CYCLE = 25;
        /// <summary>
        /// Prevents the same sound from being played more than once in the same cycle.
        /// </summary>
        private List<int> SoundsPlayedThisCycle = new List<int>(MAX_UNIQUE_SOUNDS_PER_CYCLE);

        public AudioMgrPooled()
        {
            Instance = this;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sound"></param>
        /// <returns></returns>
        private static SoundEffectInstance PoolConstructor(params string[] sound)
        {
#warning randomizing the sounds doesnt work for some reason. It's always the same sound...
            SoundEffect se = Global.Content.Load<SoundEffect>(Folder + sound[Maths.RandomNr(0, sound.Length - 1)]);
            SoundEffectInstance sei = se.CreateInstance();
            AudioMgrPooled.Instance.InstancesForCleanup.Add(sei);
            sei.IsLooped = false;
            return sei;
        }

        public void AddSound(int poolSize, params string[] sound)
        {
            SoundPool.Add(new Pool<SoundEffectInstance>(poolSize, true, s => s.State == SoundState.Playing, () => PoolConstructor(sound)));
        }

        public void PlaySound(int index)
        {
            if (!SoundsPlayedThisCycle.Contains(index))
            {
                SoundsPlayedThisCycle.Add(index);
                SoundEffectInstance sei = SoundPool[index].New();
                sei.Play();
            }
        }



        public void SetSoundVolumeAll(float volume)
        {
            for (int i = 0; i < SoundPool.Count; i++)
            {
                for (int j = 0; j < SoundPool[i].InvalidCount; j++)
                {
                    // Sets the volume.
                    SoundPool[i][j, false].Volume = volume;
                }
            }
        }

        public void Update(GameTime gameTime)
        {
            SoundsPlayedThisCycle.Clear();
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