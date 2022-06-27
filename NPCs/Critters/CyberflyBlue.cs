using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpiritMod.Items.Consumable;
using SpiritMod.Items.Material;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using System;
using Terraria.ModLoader.Utilities;

namespace SpiritMod.NPCs.Critters
{
	public class CyberflyBlue : ModNPC
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Cyberfly");
			Main.npcFrameCount[NPC.type] = 2;
			Main.npcCatchable[NPC.type] = true;
		}

		public override void SetDefaults()
		{
			NPC.width = 18;
			NPC.height = 18;
			NPC.damage = 0;
			NPC.defense = 0;
			NPC.lifeMax = 5;
			NPC.dontCountMe = true;
			NPC.HitSound = SoundID.NPCHit1;
			NPC.DeathSound = SoundID.NPCDeath1;
			NPC.catchItem = (short)ModContent.ItemType<CyberflyBlueItem>();
			NPC.knockBackResist = .45f;
			NPC.aiStyle = 64;
			NPC.npcSlots = 0;
			NPC.noGravity = true;

			AIType = NPCID.Firefly;
		}

		public override void AI()
		{
			NPC.spriteDirection = NPC.direction;

			Lighting.AddLight((int)((NPC.position.X + (NPC.width / 2f)) / 16f), (int)((NPC.position.Y + (NPC.height / 2f)) / 16f), .2f, .3f, .5f);

			if (Main.rand.Next(45) == 0)
			{
				int num622 = Dust.NewDust(new Vector2(NPC.position.X, NPC.position.Y), NPC.width, NPC.height, DustID.Electric, 0f, 0f, 100, default, .5f);
				Main.dust[num622].velocity *= .1f;
				Main.dust[num622].noGravity = true;
			}
		}

		public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
		{
			var effects = NPC.direction == -1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
			spriteBatch.Draw(TextureAssets.Npc[NPC.type].Value, NPC.Center - Main.screenPosition + new Vector2(0, NPC.gfxOffY), NPC.frame, drawColor, NPC.rotation, NPC.frame.Size() / 2, NPC.scale, effects, 0);
			return false;
		}

		public override void PostDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
		{
			GlowmaskUtils.DrawNPCGlowMask(spriteBatch, NPC, TextureAssets.Npc[NPC.type].Value);
			DrawSpecialGlow(spriteBatch, drawColor);
		}

		public override void HitEffect(int hitDirection, double damage)
		{
			if (NPC.life <= 0) 
			{
				for (int i = 0; i < 14; i++)
				{
					Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Electric, 2.5f * hitDirection, -2.5f, 0, default, 0.57f);
					int num = Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Electric, 0f, -2f, 0, new Color(0, 255, 142), .6f);
					Dust dust = Main.dust[num];
					dust.noGravity = true;
					dust.position.X += ((Main.rand.Next(-50, 51) / 20) - 1.5f);
					dust.position.Y += ((Main.rand.Next(-50, 51) / 20) - 1.5f);
					if (Main.dust[num].position != NPC.Center)
						Main.dust[num].velocity = NPC.DirectionTo(Main.dust[num].position) * 3f;
				}
			}
		}

		public override float SpawnChance(NPCSpawnInfo spawnInfo)
		{
			Player player = spawnInfo.Player;
			if (!(player.ZoneTowerSolar || player.ZoneTowerVortex || player.ZoneTowerNebula || player.ZoneTowerStardust) && !Main.pumpkinMoon && !Main.snowMoon && !Main.eclipse && (SpawnCondition.GoblinArmy.Chance == 0))
				return spawnInfo.Player.GetSpiritPlayer().ZoneSynthwave ? .15f : 0f;
			return 0f;
		}

		public void DrawSpecialGlow(SpriteBatch spriteBatch, Color drawColor)
		{
			float num107 = (float)Math.Cos((Main.GlobalTimeWrappedHourly % 2.4f / 2.4f * 6.28318548f)) / 2f + 0.5f;

			SpriteEffects spriteEffects3 = NPC.direction == -1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
			Color drawCol = new Color(127 - NPC.alpha, 127 - NPC.alpha, 127 - NPC.alpha, 0).MultiplyRGBA(Color.LightBlue);

			for (int i = 0; i < 4; i++)
			{
				drawCol = NPC.GetAlpha(drawCol) * (1f - num107);
				Vector2 vector29 = NPC.Center + (i / 4f * MathHelper.TwoPi + NPC.rotation).ToRotationVector2() * (4f * num107 + 2f) - Main.screenPosition + new Vector2(0, NPC.gfxOffY - 2) - NPC.velocity * i;
				spriteBatch.Draw(TextureAssets.Npc[NPC.type].Value, vector29, NPC.frame, drawCol, NPC.rotation, NPC.frame.Size() / 2f, NPC.scale, spriteEffects3, 0f);
			}
		}

		public override void FindFrame(int frameHeight)
		{
			NPC.frameCounter += 0.15f;
			NPC.frameCounter %= Main.npcFrameCount[NPC.type];
			NPC.frame.Y = (int)NPC.frameCounter * frameHeight;
		}
	}
}
