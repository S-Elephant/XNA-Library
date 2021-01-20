using System.Collections.Generic;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Storage;
using System;

namespace XNALib
{
    /// <summary>
    /// - ContentProject
    ///     - Audio
    ///         - [name].xap
    ///         - [add the wav files here]
    /// Note that this class will generate garbage for the GC
    /// </summary>
    public class XactMgr
    {
        #region Members
        private bool m_EnableMusic = true;
        public bool EnableMusic
        {
            get { return m_EnableMusic; }
            set { m_EnableMusic = value; if (!value) StopAllMusic(); }
        }

        private bool m_EnableSound = true;
        public bool EnableSound
        {
            get { return m_EnableSound; }
            set { m_EnableSound = value; if (!value) StopAllSound(); }
        }
       
        AudioEngine AudioEngine;
        WaveBank WaveBank;
        SoundBank SoundBank;

        AudioCategory MusicCategory;
        AudioCategory SoundCategory;
        string MusicCategoryName=null, SoundCategoryName=null;

        List<Cue> Sounds = new List<Cue>(MAX_SOUNDS);
        List<Cue> Musics = new List<Cue>(MAX_MUSICS);
        const int MAX_SOUNDS = 512;
        const int MAX_MUSICS = 10;
        public bool LoopMusic = true;
        #endregion

        void Init(string name, string folder, string soundCategoryName, string musicCategoryName)
        {
            try
            {
                AudioEngine = new AudioEngine(string.Format(".\\Content\\{0}\\{1}.xgs", folder, name));
                WaveBank = new WaveBank(AudioEngine, string.Format(".\\Content\\{0}\\Wave Bank.xwb", folder));
                SoundBank = new SoundBank(AudioEngine, string.Format(".\\Content\\{0}\\Sound Bank.xsb", folder));

                SoundCategoryName = soundCategoryName;
                MusicCategoryName = musicCategoryName;

                if(!string.IsNullOrEmpty(SoundCategoryName ))
                    SoundCategory = AudioEngine.GetCategory(SoundCategoryName);
                if (!string.IsNullOrEmpty(MusicCategoryName))
                    MusicCategory = AudioEngine.GetCategory(MusicCategoryName);
            }
            catch (Exception ex)
            {
                throw ex;
                //PopupMgr.CreatePopup(ex.Message);
            }
        }

        /// <summary>
        /// Default = 1.0f, 0.0f = silence
        /// </summary>
        /// <param name="volume"></param>
        public void SetMusicVolume(float volume)
        {
            MusicCategory.SetVolume(volume);
        }

        /// <summary>
        /// default = 1.0f, 0.0f = silence
        /// </summary>
        /// <param name="volume"></param>
        /// <returns></returns>
        public void SetSoundVolume(float volume)
        {
            SoundCategory.SetVolume(volume);
        }

        public XactMgr(string name, string folder, string soundCategoryName, string musicCategoryName)
        {
            Init(name, folder, soundCategoryName, musicCategoryName);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name">Name of the XACT project (or the file's name w/o extension)</param>
        public XactMgr(string name, string soundCategoryName, string musicCategoryName)
        {
            Init(name, "Audio", soundCategoryName, musicCategoryName);

        }

        public void Update()
        {
            // Music looping
            if (LoopMusic)
            {
                for (int i = 0; i < Musics.Count; i++)
                {
                    if (Musics[i].IsStopped)
                        PlayMusic(Musics[i].Name);
                }
            }
        }

        public void PlaySound(string name)
        {
            if (EnableSound)
            {
                Cue newCue = SoundBank.GetCue(name);
                newCue.Play();
                Sounds.Add(newCue);
            }
        }
        public void PlayMusic(string name)
        {
            PlayMusic(name, true);
        }
        public void PlayMusic(string name, bool stopAllOtherMusic)
        {
            if (EnableMusic)
            {
                if (stopAllOtherMusic)
                    StopAllMusic();

                Cue newCue = SoundBank.GetCue(name);
                newCue.Play();
                Musics.Add(newCue);
            }
        }

        public void StopSound(string name)
        {
            foreach (Cue c in Sounds)
            {
                if (c.Name == name)
                {
                    c.Stop(AudioStopOptions.Immediate);
                    Sounds.Remove(c);
                    break;
                }
            }
        }
        public void StopMusic(string name)
        {
            foreach (Cue c in Musics)
            {
                if (c.Name == name)
                {
                    c.Stop(AudioStopOptions.Immediate);
                    Musics.Remove(c);
                    break;
                }
            }
        }

        public void StopAllSound(string name)
        {
            int i = 0;
            while (i < Sounds.Count)
            {
                if (Sounds[i].Name == name)
                {
                    Sounds[i].Stop(AudioStopOptions.Immediate);
                    Sounds.Remove(Sounds[i]);
                    i--;
                }
                i++;
            }
        }
        public void StopAllSound()
        {
            Sounds.ForEach(s => s.Stop(AudioStopOptions.Immediate));
            Sounds.Clear();
        }
        public void StopAllMusic(string name)
        {
            int i = 0;
            while (i < Musics.Count)
            {
                if (Musics[i].Name == name)
                {
                    Musics[i].Stop(AudioStopOptions.Immediate);
                    Musics.Remove(Musics[i]);
                    i--;
                }
                i++;
            }
        }
        public void StopAllMusic()
        {
            Musics.ForEach(m => m.Stop(AudioStopOptions.Immediate));
            Musics.Clear();
        }
    }
}
