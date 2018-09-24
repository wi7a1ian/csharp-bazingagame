using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using BazingaGame.Prefabs;
using BazingaGame.States.Player;
using FarseerPhysics.Dynamics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;

namespace BazingaGame.Sounds
{
    public class SoundManager
    {
        private ContentManager _content;
        private Dictionary<string, SoundEffectInstance> _soundCache = new Dictionary<string, SoundEffectInstance>();

        // http://rbwhitaker.wikidot.com/playing-sound-effects
        // TODO: http://rbwhitaker.wikidot.com/using-xactpublic 
        public SoundManager(ContentManager content)
        {
            _content = content;
        }

        public void PlaySound(string soundName, bool loop = false)
        {
            soundName = soundName.ToLower();

            if (!_soundCache.ContainsKey(soundName))
            {
                _soundCache.Add(soundName, _content.Load<SoundEffect>(soundName).CreateInstance());
            }

            var aaa = _content.Load<SoundEffect>(soundName);
            aaa.Play();

            var sound = _soundCache[soundName];
            sound.IsLooped = loop;
            sound.Play();
        }
    }
}
