using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpiritMod.Effects.Stargoop;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;

namespace SpiritMod.Dusts
{
    public class StargoopDust : ModDust, IMetaball
    {
        protected Dictionary<int, Vector2> positions = new Dictionary<int, Vector2>();
        protected Dictionary<int, float> scales = new Dictionary<int, float>();

		public sealed override string Texture => "SpiritMod/Dusts/StargoopDust";

		protected void Attach(StargoopLayer layer)
		{
            if (!layer.Metaballs.Contains(this))
                layer.Metaballs.Add(this);
        }

        public void Reset()
        {
            positions = new Dictionary<int, Vector2>();
            scales = new Dictionary<int, float>();
        }

		public override Color? GetAlpha(Dust dust, Color lightColor) => Color.Transparent;

        public override bool Update(Dust dust)
        {
            positions[dust.dustIndex] = dust.position;
            scales[dust.dustIndex] = dust.scale;

            if (dust.scale <= DespawnScale)
            {
					positions.Remove(dust.dustIndex);
					scales.Remove(dust.dustIndex);
					dust.active = false;
            }
            return false;
        }

		internal virtual float DespawnScale => 0.25f;

        public void DrawOnMetaballLayer(SpriteBatch sB)
        {
            foreach (var k in positions.Keys)
            {
                if (Main.dust[k].active)
                    sB.Draw(SpiritMod.Metaballs.Mask, (positions[k] - Main.screenPosition) / 2, null, Color.White, 0f, Vector2.One * 256f, scales[k] / 64f, SpriteEffects.None, 0);
            }
        }
    }
}