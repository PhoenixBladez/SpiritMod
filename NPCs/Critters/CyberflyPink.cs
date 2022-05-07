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
	public class CyberflyPink : ModNPC
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Cyberfly");
			Main.npcFrameCount[npc.type] = 2;
			Main.npcCatchable[npc.type] = true;
		}

		public override void SetDefaults()
		{
			npc.width = 12;
			npc.height = 12;
			npc.damage = 0;
			npc.defense = 0;
			npc.lifeMax = 5;
			npc.dontCountMe = true;
			npc.HitSound = SoundID.NPCHit1;
			npc.DeathSound = SoundID.NPCDeath1;
			npc.catchItem = (short)ModContent.ItemType<CyberflyPinkItem>();
			npc.knockBackResist = .45f;
			npc.aiStyle = 64;
			npc.npcSlots = 0;
			npc.noGravity = true;

			aiType = NPCID.Firefly;
		}

		public override void AI()
		{
			npc.spriteDirection = npc.direction;
			Lighting.AddLight((int)((npc.position.X + (npc.width / 2f)) / 16f), (int)((npc.position.Y + (npc.height / 2f)) / 16f), .4f, .2f, .27f);


			if (Main.rand.Next(45) == 0)
			{
				int num622 = Dust.NewDust(new Vector2(npc.position.X, npc.position.Y), npc.width, npc.height, DustID.VenomStaff, 0f, 0f, 100, default, .85f);
				Main.dust[num622].velocity *= .1f;
				Main.dust[num622].noGravity = true;
			}
		}

		public override bool PreDraw(SpriteBatch spriteBatch, Color drawColor)
		{
			var effects = npc.direction == -1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
			spriteBatch.Draw(Main.npcTexture[npc.type], npc.Center - Main.screenPosition + new Vector2(0, npc.gfxOffY), npc.frame, drawColor, npc.rotation, npc.frame.Size() / 2, npc.scale, effects, 0);
			return false;
		}

		public override void PostDraw(SpriteBatch spriteBatch, Color drawColor)
		{
			GlowmaskUtils.DrawNPCGlowMask(spriteBatch, npc, Main.npcTexture[npc.type]);
			DrawSpecialGlow(spriteBatch);
		}

		public override void HitEffect(int hitDirection, double damage)
		{
			if (npc.life <= 0) 
			{
				for (int i = 0; i < 14; i++)
				{
					Dust.NewDust(npc.position, npc.width, npc.height, DustID.VenomStaff, 2.5f * hitDirection, -2.5f, 0, default, 0.67f);
					int num = Dust.NewDust(npc.position, npc.width, npc.height, DustID.VenomStaff, 0f, -2f, 0, new Color(0, 255, 142), .6f);
					Dust dust = Main.dust[num];
					dust.noGravity = true;
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
				return spawnInfo.player.GetSpiritPlayer().ZoneSynthwave ? .15f : 0f;
			return 0f;
		}

		public void DrawSpecialGlow(SpriteBatch spriteBatch)
        {
			float num107 = (float)Math.Cos((Main.GlobalTime % 2.4f / 2.4f * 6.28318548f)) / 2f + 0.5f;

			SpriteEffects spriteEffects3 = npc.direction == -1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
			Color drawCol = new Color(127 - npc.alpha, 127 - npc.alpha, 127 - npc.alpha, 0).MultiplyRGBA(Color.LightPink);

			for (int i = 0; i < 4; i++)
			{
				drawCol = npc.GetAlpha(drawCol) * (1f - num107);
				Vector2 vector29 = npc.Center + (i / 4f * MathHelper.TwoPi + npc.rotation).ToRotationVector2() * (4f * num107 + 2f) - Main.screenPosition + new Vector2(0, npc.gfxOffY - 2) - npc.velocity * i;
				spriteBatch.Draw(Main.npcTexture[npc.type], vector29, npc.frame, drawCol, npc.rotation, npc.frame.Size() / 2f, npc.scale, spriteEffects3, 0f);
			}
		}

		public override void FindFrame(int frameHeight)
		{
			npc.frameCounter += 0.15f;
			npc.frameCounter %= Main.npcFrameCount[npc.type];
			npc.frame.Y = (int)npc.frameCounter * frameHeight;
		}
	}
}
