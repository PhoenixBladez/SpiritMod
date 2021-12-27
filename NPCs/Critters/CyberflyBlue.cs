using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpiritMod.Items.Consumable;
using SpiritMod.Items.Material;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System;

namespace SpiritMod.NPCs.Critters
{
	public class CyberflyBlue : ModNPC
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Cyberfly");
			Main.npcFrameCount[npc.type] = 2;
		}

		public override void SetDefaults()
		{
			npc.width = 12;
			npc.height = 12;
			npc.damage = 0;
			npc.defense = 0;
			npc.lifeMax = 5;
			npc.dontCountMe = true;
			npc.HitSound = SoundID.NPCHit3;
			npc.DeathSound = SoundID.NPCDeath4;
			Main.npcCatchable[npc.type] = true;
			npc.catchItem = (short)ModContent.ItemType<CyberflyBlueItem>();
			npc.knockBackResist = .45f;
			npc.aiStyle = 64;
			npc.npcSlots = 0;
			npc.noGravity = true;
			aiType = NPCID.Firefly;
			Main.npcFrameCount[npc.type] = 2;
		}

		public override void AI()
		{
			npc.spriteDirection = npc.direction;

			Lighting.AddLight((int)((npc.position.X + (npc.width / 2f)) / 16f), (int)((npc.position.Y + (npc.height / 2f)) / 16f), .2f, .3f, .5f);
			if (Main.rand.Next(45) == 0)
			{
				int num622 = Dust.NewDust(new Vector2(npc.position.X, npc.position.Y), npc.width, npc.height, DustID.Electric, 0f, 0f, 100, default, .5f);
				Main.dust[num622].velocity *= .1f;
				Main.dust[num622].noGravity = true;
			}
		}

		public override bool PreDraw(SpriteBatch spriteBatch, Color drawColor)
		{
			var effects = npc.direction == -1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
			spriteBatch.Draw(Main.npcTexture[npc.type], npc.Center - Main.screenPosition + new Vector2(0, npc.gfxOffY), npc.frame,
							 drawColor, npc.rotation, npc.frame.Size() / 2, npc.scale, effects, 0);
			return false;
		}

		public override void PostDraw(SpriteBatch spriteBatch, Color drawColor)
		{
			GlowmaskUtils.DrawNPCGlowMask(spriteBatch, npc, Main.npcTexture[npc.type]);
			DrawSpecialGlow(spriteBatch, drawColor);
		}

		public override void HitEffect(int hitDirection, double damage)
		{
			if (npc.life <= 0) 
			{
				for (int num621 = 0; num621 < 14; num621++)
				{
					Dust.NewDust(npc.position, npc.width, npc.height, DustID.Electric, 2.5f * hitDirection, -2.5f, 0, default, 0.57f);
					int num = Dust.NewDust(npc.position, npc.width, npc.height, DustID.Electric, 0f, -2f, 0, new Color(0, 255, 142), .6f);
					Main.dust[num].noGravity = true;
					Dust dust = Main.dust[num];
					dust.position.X += ((Main.rand.Next(-50, 51) / 20) - 1.5f);
					dust.position.Y += ((Main.rand.Next(-50, 51) / 20) - 1.5f);
					if (Main.dust[num].position != npc.Center)
						Main.dust[num].velocity = npc.DirectionTo(Main.dust[num].position) * 3f;
				}
			}
		}

		public override float SpawnChance(NPCSpawnInfo spawnInfo)
		{
			Player player = spawnInfo.player;
			if (!(player.ZoneTowerSolar || player.ZoneTowerVortex || player.ZoneTowerNebula || player.ZoneTowerStardust) && !Main.pumpkinMoon && !Main.snowMoon && !Main.eclipse && (SpawnCondition.GoblinArmy.Chance == 0))
				return spawnInfo.player.GetSpiritPlayer().ZoneSynthwave ? .1f : 0f;
			return 0f;
		}

		public void DrawSpecialGlow(SpriteBatch spriteBatch, Color drawColor)
		{
			float num108 = 4;
			float num107 = (float)Math.Cos((Main.GlobalTime % 2.4f / 2.4f * 6.28318548f)) / 2f + 0.5f;
			float num106 = 0f;

			SpriteEffects spriteEffects3 = npc.direction == -1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
			Color color29 = new Color(127 - npc.alpha, 127 - npc.alpha, 127 - npc.alpha, 0).MultiplyRGBA(Color.LightBlue);
			for (int num103 = 0; num103 < 4; num103++)
			{
				Color color28 = color29;
				color28 = npc.GetAlpha(color28);
				color28 *= 1f - num107;
				Vector2 vector29 = new Vector2(npc.Center.X, npc.Center.Y - 2) + (num103 / num108 * 6.28318548f + npc.rotation + num106).ToRotationVector2() * (4f * num107 + 2f) - Main.screenPosition + new Vector2(0, npc.gfxOffY) - npc.velocity * (float)num103;
				spriteBatch.Draw(Main.npcTexture[npc.type], vector29, npc.frame, color28, npc.rotation, npc.frame.Size() / 2f, npc.scale, spriteEffects3, 0f);
			}
		}

		public override void FindFrame(int frameHeight)
		{
			npc.frameCounter += 0.15f;
			npc.frameCounter %= Main.npcFrameCount[npc.type];
			int frame = (int)npc.frameCounter;
			npc.frame.Y = frame * frameHeight;
		}
	}
}
