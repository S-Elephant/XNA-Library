using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Audio;

namespace XNALib
{
    [Obsolete("use audiomgr instead")]
    public class Audio
    {
        #region Members
        
        private bool m_EnableMusic = true;
        public bool EnableMusic
        {
            get { return m_EnableMusic; }
            set
            {
                m_EnableMusic = value;
                if (!value)
                    StopAllMusic();
            }
        }

        private bool m_EnableSound = true;
        public bool EnableSound
        {
            get { return m_EnableSound; }
            set
            {
                m_EnableSound = value;
                if (!value) StopAllFX();
            }
        }

        private List<SoundEffectInstance> m_SoundEffects = new List<SoundEffectInstance>();
        public List<SoundEffectInstance> SoundEffects
        {
            get { return m_SoundEffects; }
            set { m_SoundEffects = value; }
        }

        private Dictionary<string, string> m_Lookup = new Dictionary<string, string>();
        public Dictionary<string, string> Lookup
        {
            get { return m_Lookup; }
            set { m_Lookup = value; }
        }

        private Dictionary<string,SoundEffectInstance> m_Musics = new Dictionary<string,SoundEffectInstance>();
        public Dictionary<string,SoundEffectInstance> Musics
        {
            get { return m_Musics; }
            set { m_Musics = value; }
        }
        #endregion

        #region Music
        public enum eMusicOptions { Ignore, Override, ThrowException }
        public void PlayMusic(string name, bool loop, eMusicOptions musicOptions)
        {
            if (EnableMusic)
            {
                if (Musics.ContainsKey(name) && musicOptions == eMusicOptions.ThrowException)
                    throw new Exception("music was already added");

                if (!(Musics.ContainsKey(name) && musicOptions == eMusicOptions.Ignore))
                {
                    if (Musics.ContainsKey(name) && musicOptions == eMusicOptions.Override)
                    {
                        Musics[Lookup[name]].Stop();
                        Musics.Remove(Lookup[name]);
                    }

                    SoundEffect music = Global.Content.Load<SoundEffect>(@"Music\" + Lookup[name]);
                    SoundEffectInstance effectInstance = music.CreateInstance();
                    effectInstance.IsLooped = loop;
                    effectInstance.Play();
                    Musics.Add(name, effectInstance);
                }
            }
        }

        public void StopAllMusic()
        {
            foreach (KeyValuePair<string,SoundEffectInstance> kvp in Musics)
                kvp.Value.Stop();
            Musics.Clear();
        }
        public void StopMusic(string name)
        {
            if (Musics.ContainsKey(name))
                Musics[name].Stop(true);
        }
        #endregion
        #region FX
        public void PlayFX(string lookupKey)
        {
            PlayFXDirectly(Lookup[lookupKey]);
        }

        public void StopAllFX()
        {
            SoundEffects.ForEach(f => f.Stop(true));
            SoundEffects.Clear();
        }

        public void PlayFXDirectly(string name)
        {
            if (EnableSound)
            {
                SoundEffect effect = Global.Content.Load<SoundEffect>(@"SoundFX\" + name);
                SoundEffectInstance effectInstance = effect.CreateInstance();
                effectInstance.Play();
                SoundEffects.Add(effectInstance);
            }
        }
        #endregion
    }
}
