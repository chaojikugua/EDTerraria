﻿/*
  _____                 ____                 
 | ____|_ __ ___  _   _|  _ \  _____   _____ 
 |  _| | '_ ` _ \| | | | | | |/ _ \ \ / / __|
 | |___| | | | | | |_| | |_| |  __/\ V /\__ \
 |_____|_| |_| |_|\__,_|____/ \___| \_/ |___/
          <http://emudevs.com>
             Terraria 1.3
*/

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.Graphics.Effects;

namespace Terraria.GameContent.Skies
{
    internal class MoonLordSky : CustomSky
    {
        private Random _random = new Random();
        private int _moonLordIndex = -1;
        private bool _isActive;

        public override void OnLoad() { }

        public override void Update() { }

        private float GetIntensity()
        {
            if (!UpdateMoonLordIndex())
                return 0.0f;

            float x = 0.0f;
            if (_moonLordIndex != -1)
                x = Vector2.Distance(Main.player[Main.myPlayer].Center, Main.npc[_moonLordIndex].Center);

            return 1f - Utils.SmoothStep(3000f, 6000f, x);
        }

        public override Color OnTileColor(Color inColor)
        {
            float intensity = GetIntensity();
            return new Color(Vector4.Lerp(new Vector4(0.5f, 0.8f, 1f, 1f), inColor.ToVector4(), 1f - intensity));
        }

        private bool UpdateMoonLordIndex()
        {
            if (_moonLordIndex >= 0 && Main.npc[_moonLordIndex].active && Main.npc[_moonLordIndex].type == 398)
                return true;

            int num = -1;
            for (int index = 0; index < Main.npc.Length; ++index)
            {
                if (Main.npc[index].active && Main.npc[index].type == 398)
                {
                    num = index;
                    break;
                }
            }

            _moonLordIndex = num;
            return num != -1;
        }

        public override void Draw(SpriteBatch spriteBatch, float minDepth, float maxDepth)
        {
            if (maxDepth < 0.0 || minDepth >= 0.0)
                return;

            float intensity = GetIntensity();
            spriteBatch.Draw(Main.blackTileTexture, new Rectangle(0, 0, Main.screenWidth, Main.screenHeight), Color.Black * intensity);
        }

        public override float GetCloudAlpha()
        {
            return 0.0f;
        }

        internal override void Activate(Vector2 position, params object[] args)
        {
            _isActive = true;
        }

        internal override void Deactivate(params object[] args)
        {
            _isActive = false;
        }

        public override void Reset()
        {
            _isActive = false;
        }

        public override bool IsActive()
        {
            return _isActive;
        }
    }
}
