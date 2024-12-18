using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Media;


namespace MONOGAME_VERSION_5
{
    internal class Audio
    {
        // Vars
        private ContentManager Content;

        // Constructor
        public Audio(ContentManager mngr)
        {
            Content = mngr;
        }

        // Method
        public void PlaySound(string name, float volume)
        {
            SoundEffect sound = Content.Load<SoundEffect>("Audio/" + name);
            sound.Play(volume, 0.0f, 0.0f);
        }

        public void PlaySong(string name, float volume)
        {
            Song song = Content.Load<Song>("Audio/" + name);
            MediaPlayer.IsRepeating = true;
            MediaPlayer.Volume = volume;
            MediaPlayer.Play(song);
        }
    }
}

