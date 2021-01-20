using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace XNALib
{
    public struct Range
    {
        public int Min, Max;
        public Range(int min, int max)
        {
            Min = min;
            Max = max;
        }
    }

    public class CharacterInput
    {
        public delegate void OnEnd(CharacterInput CI);
        public event OnEnd End;

        public string Text = string.Empty;
        public char CurrentChar { get { return Convert.ToChar(currentRangeItem); } }
        public List<Range> Ranges = new List<Range>();
        int CurrentRange = 0;
        int currentRangeItem;
        public int MaxLength = -1;
        public Vector2 Location;
        public int EndCharacterInASCII = 46; // 32 = space, 46 = dot
        public bool IsEnded = false;

        public CharacterInput(int maxLength, Vector2 location)
        {
            SetRangeToCapitalOnly();
            MaxLength = maxLength;
            Location = location;
        }

        public void SetRangeToCapitalOnly()
        {
            Ranges.Clear();
            Ranges.Add(new Range(EndCharacterInASCII, EndCharacterInASCII));
            Ranges.Add(new Range(65, 90));
            currentRangeItem = Ranges[0].Min;
        }
        public void SetRangeToTextOnly()
        {
            Ranges.Clear();
            Ranges.Add(new Range(EndCharacterInASCII, EndCharacterInASCII));
            Ranges.Add(new Range(65, 90));
            Ranges.Add(new Range(97, 122));
            currentRangeItem = Ranges[0].Min;
        }

        public void Next()
        {
            if (currentRangeItem+1 > Ranges[CurrentRange].Max)
            {
                if (CurrentRange+1 > Ranges.Count - 1)
                    CurrentRange = 0;
                else
                    CurrentRange++;
                currentRangeItem = Ranges[CurrentRange].Min;
            }
            else
                currentRangeItem++;
        }
        public void Previous()
        {
            if (currentRangeItem-1 < Ranges[CurrentRange].Min)
            {
                if (CurrentRange-1 < 0)
                    CurrentRange = Ranges.Count - 1;
                else
                    CurrentRange--;
                currentRangeItem = Ranges[CurrentRange].Max;
            }
            else
                currentRangeItem--;
        }
        public void Accept()
        {
            if (Text.Length < MaxLength)
            {
                if (currentRangeItem == EndCharacterInASCII)
                {
                    IsEnded = true;
                    End(this);
                }
                else
                    Text += CurrentChar;
            }
            else
                End(this);
        }

        public void Draw(SpriteBatch spriteBatch, SpriteFont font)
        {
            spriteBatch.DrawString(font, Text, Location, Color.White);
            if (Text.Length < MaxLength && !IsEnded)
                spriteBatch.DrawString(font, CurrentChar.ToString(), Location + new Vector2(font.MeasureString(Text).X, 0), Color.White);
        }
    }
}
