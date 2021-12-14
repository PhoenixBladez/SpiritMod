using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Linq;
using SpiritMod.Particles;

namespace SpiritMod.Mechanics.BoonSystem.AsclepiusBoon
{
	public class AsclepiusBoon : Boon
	{
		public override bool CanApply
		{
			get
			{
				var npcsWithBoon = Main.npc.Where(n => n.active && n.GetGlobalNPC<BoonNPC>().currentBoon is AsclepiusBoon);
				if (npcsWithBoon.Count() > 0)
					return false;
				return true;
			}
		}

		private int frameCounter;

		private int frameY;

		private float bloomCounter = 0;

		private int projectileCounter;

		public override void AI()
		{
			Lighting.AddLight(npc.Center, Color.Green.ToVector3() * 0.3f);

			frameCounter++;
			if (frameCounter % 7 == 0)
				frameY++;
			frameY %= 7;

			bloomCounter += 0.04f;

			projectileCounter++;
			if (projectileCounter % 160 == 0)
			{
				for (int i = 0; i < 15; i++)
				{
					Vector2 direction = Main.rand.NextVector2Circular(20, 20);
					StarParticle particle = new StarParticle(
					(npc.Center - new Vector2(0,40)) + direction,
					direction * 0.15f,
					new Color(48, 195, 43),
					Main.rand.NextFloat(0.08f, 0.23f),
					Main.rand.Next(20, 40));

					ParticleHandler.SpawnParticle(particle);
				}

				for (int i = 0; i < 3; i++)
					NPC.NewNPC((int)npc.Center.X, (int)npc.Center.Y - 40, ModContent.NPCType<AsclepiusBoonOrb>(), 0, npc.whoAmI, i * 2.08f);
			}
		}

		public override void PostDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			Texture2D tex = ModContent.GetTexture("SpiritMod/Mechanics/BoonSystem/AsclepiusBoon/AsclepiusBoon");
			Texture2D tex2 = ModContent.GetTexture("SpiritMod/Mechanics/BoonSystem/AsclepiusBoon/AsclepiusBoon_Glow");
			int frameHeight = tex.Height / 7;
			Rectangle frame = new Rectangle(0, frameHeight * frameY, tex.Width, frameHeight);

			Color glowColor = Color.Green;
			glowColor.A = 0;
			glowColor *= 0.75f;

			float glowScale = 1 + ((float)Math.Sin(bloomCounter) / 4);
			spriteBatch.Draw(tex2, ((npc.Top + new Vector2(0,15)) - new Vector2(0, frameHeight / 2)) - Main.screenPosition, null, glowColor * glowScale, 0, tex2.Size() / 2, npc.scale * 0.7f, SpriteEffects.None, 0f);
			spriteBatch.Draw(tex, (npc.Top - new Vector2(0, frameHeight / 2)) - Main.screenPosition, frame, Color.White, 0, new Vector2(tex.Width, frameHeight) / 2, npc.scale, SpriteEffects.None, 0f);
		}

		public override void OnDeath()
		{
			DropOlympium(Main.rand.Next(3, 6));
		}
	}
}