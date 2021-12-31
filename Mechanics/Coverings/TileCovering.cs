using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.Graphics.Effects;

namespace SpiritMod.Mechanics.Coverings
{
    public abstract class TileCovering
    {
        protected internal Mod Mod { get; internal set; }
        protected internal CoveringsManager CoveringsManager { get; internal set; }

		public virtual RenderLayers Layer => RenderLayers.Entities;
        public virtual bool RequiresSaving => false;
        public virtual void StaticLoad() { }
        public virtual bool IsValidAt(int x, int y) 
        {
            return Framing.GetTileSafely(x, y).active();
        }
        public virtual void Update(GameTime gameTime, int x, int y, int variation, int orientation) { }
        public virtual void Draw(SpriteBatch spriteBatch, int x, int y, int variation, int orientation) { }
    }
}
