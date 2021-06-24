using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;

namespace SpiritMod.Effects.Stargoop
{
	public class Metaball: IMetaball
	{
		public Metaball(Vector2 position, float scale)
		{
			Position = position;
			Scale = scale;
			rotationConst = (float)Main.rand.NextDouble() * 6.28f;
		}

		public Vector2 Position { get; set; }
		public Vector2 Velocity { get; set; }
		public float Scale { get; set; }
		private float rotationConst;


		public void Update()
		{
			Velocity = new Vector2((float)Math.Cos(Main.time * 0.05) * (float)Math.Sin(rotationConst) * 0.2f + rotationConst * 0.1f, (float)Math.Sin(Main.time * 0.05) * (float)Math.Cos(rotationConst) * 0.2f - rotationConst * 0.1f).RotatedBy(rotationConst);
			Scale  *= 0.995f;
			Scale -= 0.015f;
			if (Scale < 0.25f)
			{
				if (SpiritMod.Metaballs.EnemyLayer.Metaballs.Contains(this))
					SpiritMod.Metaballs.EnemyLayer.Metaballs.Remove(this);
				if (SpiritMod.Metaballs.FriendlyLayer.Metaballs.Contains(this))
					SpiritMod.Metaballs.FriendlyLayer.Metaballs.Remove(this);
			}
			Position += Velocity;
		}

		public void DrawOnMetaballLayer(SpriteBatch sB)
		{
			SpiritMod.Metaballs.borderNoise.Parameters["offset"].SetValue((float)Main.time / 1000f + rotationConst);

			sB.Draw(SpiritMod.Metaballs.Mask, (Position - Main.screenPosition) / 2, null, Color.White, 0f, Vector2.One * 256f, Scale / 32f, SpriteEffects.None, 0);
		}
	}
}
