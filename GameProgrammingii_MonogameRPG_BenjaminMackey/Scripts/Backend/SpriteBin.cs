using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameProgrammingii_MonogameRPG_BenjaminMackey.Scripts.Backend
{
    public static class SpriteBin
    {
        public static List<SpriteLookupData> _sprites = new List<SpriteLookupData>();

        private static int _IDindex = 0;
        public static int Add(Texture2D texture, string Filename)
        {
            if(GetSprite(Filename) != null) return -1;

            _sprites.Add(new SpriteLookupData(texture, Filename, _IDindex));
            _IDindex++;
            return _IDindex - 1;
        }
        public static Texture2D GetSprite(string SpriteName)
        {
            foreach (SpriteLookupData item in _sprites)
            {
                if (item._fileName == SpriteName) return item._texture;
            }
            return null;
        }
        public static Texture2D GetSprite(int spriteID)
        {
            foreach (SpriteLookupData item in _sprites)
            {
                if (item._ID == spriteID) return item._texture;
            }
            return null;
        }
    }
    public struct SpriteLookupData
    {
        public string _fileName;
        public int _ID;

        public Texture2D _texture;
        public SpriteLookupData(Texture2D texture, string FileName, int ID)
        {
            _texture = texture;
            _fileName = FileName;
            _ID = ID;
        }
    }

}
