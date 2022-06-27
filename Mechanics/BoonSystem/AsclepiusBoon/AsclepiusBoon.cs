using Terraria;
using Terraria.Audio;
using Terraria.ID;
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

		public override void SetStats() => npc.lifeMax = npc.life = (int)(npc.lifeMax * 1.5f);

		public override string TexturePath => "SpiritMod/Mechanics/BoonSystem/AsclepiusBoon/AsclepiusBoon";
		public override Vector2 SigilSize => new Vector2(42, 490);

		private int frameCounter;

		private int frameY;
		private const int NUM_FRAMES = 7;

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
				//First, check if there's a potential damaged target to heal nearby
				bool targetNearby = false;
				for (int i = 0; i < Main.npc.Length; i++)
				{
					NPC n = Main.npc[i];
					if (n.CanBeChasedBy(this) && n.active && n.Distance(npc.Center) < AsclepiusBoonOrb.HOME_DISTANCE && n.life < n.lifeMax && n.whoAmI != npc.whoAmI)
						targetNearby = true;
				}

				//If there is, proceed and do visual effects
				if (targetNearby)
				{
					if (!Main.dedServ)
					{
						for (int i = 0; i < 15; i++)
						{
							Vector2 direction = Main.rand.NextVector2Circular(20, 20);
							StarParticle particle = new StarParticle(
							(npc.Center - new Vector2(0, 40)) + direction,
							direction * 0.15f,
							new Color(49, 212, 76),
							Main.rand.NextFloat(0.08f, 0.23f),
							Main.rand.Next(20, 40));

							ParticleHandler.SpawnParticle(particle);
						}
						SoundEngine.PlaySound(SoundID.DD2_DarkMageCastHeal, npc.Center);
					}

					for (int i = 0; i < 3; i++)
					{
						int n = NPC.NewNPC((int)npc.Center.X, (int)npc.Center.Y - 40, ModContent.NPCType<AsclepiusBoonOrb>(), 0, npc.whoAmI, i * 2.08f);
						if (Main.netMode != NetmodeID.SinglePlayer)
							NetMessage.SendData(MessageID.SyncNPC, -1, -1, null, n);
					}
				}
			}
		}

		//Shift the position downwards, to adjust for it using a vertical spritesheet
		public override Vector2 SigilPosition => base.SigilPosition + (Vector2.UnitY * Texture.Height * 0.5f * (NUM_FRAMES - 1) / NUM_FRAMES);

		public override void PostDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			DrawBeam(new Color(84, 247, 149, 0), new Color(15, 252, 74, 0));

			DrawBloom(spriteBatch, Color.Green * 0.5f, 0.7f);

			Color sigilColor = Color.White;
			sigilColor.A = 180;
			//Hardcoded origin point to center the texture properly
			Vector2 origin = new Vector2(24, 48);

			int frameHeight = Texture.Height / NUM_FRAMES;
			Rectangle frame = new Rectangle(0, frameHeight * frameY, Texture.Width, frameHeight);
			spriteBatch.Draw(Texture, SigilPosition - Main.screenPosition, frame, sigilColor, 0,
				origin, npc.scale, SpriteEffects.None, 0f);
		}

		public override void OnDeath() => DropOlympium(Main.rand.Next(3, 6));
	}
}