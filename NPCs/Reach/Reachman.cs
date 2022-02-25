using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using SpiritMod.Items.Sets.BriarDrops;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using SpiritMod.Items.Consumable.Food;
using Terraria.Audio;
using System.IO;

namespace SpiritMod.NPCs.Reach
{
	public class Reachman : ModNPC
	{
		int frame = 0;
		int frametimer = 0;
		int aiTimer = 0;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Feral Shambler");
			Main.npcFrameCount[npc.type] = 16;
		}

		public override void SetDefaults()
		{
			npc.width = 36;
			npc.height = 52;
			npc.damage = 22;
			npc.defense = 8;
			npc.lifeMax = 59;
			npc.buffImmune[BuffID.Poisoned] = true;
			npc.DeathSound = SoundID.NPCDeath2;
			npc.value = 70f;
			npc.knockBackResist = .34f;
			npc.aiStyle = 3;
			aiType = NPCID.SnowFlinx;
			npc.HitSound = new LegacySoundStyle(SoundID.NPCHit, 2).WithPitchVariance(0.2f);
			banner = npc.type;
            bannerItem = ModContent.ItemType<Items.Banners.ReachmanBanner>();
        }
		public override float SpawnChance(NPCSpawnInfo spawnInfo)
		{
			Player player = spawnInfo.player;
			if (!(player.ZoneTowerSolar || player.ZoneTowerVortex || player.ZoneTowerNebula || player.ZoneTowerStardust) && !Main.pumpkinMoon && !Main.snowMoon && !Main.eclipse  && (SpawnCondition.GoblinArmy.Chance == 0))
				return spawnInfo.player.GetSpiritPlayer().ZoneReach ? 1.7f : 0f;
			return 0f;
		}
		public override void SendExtraAI(BinaryWriter writer)
		{
			writer.Write(frame);
			writer.Write(frametimer);
			writer.Write(aiTimer);
		}

		public override void ReceiveExtraAI(BinaryReader reader)
		{
			frame = reader.ReadInt32();
			frametimer = reader.ReadInt32();
			aiTimer = reader.ReadInt32();
		}
		public override void AI()
		{
			Lighting.AddLight((int)(npc.Center.X / 16f), (int)(npc.Center.Y / 16f), 0.23f, 0.16f, .05f);

			aiTimer++;
			frametimer++;

			if (npc.life <= npc.lifeMax - 20)
			{
				if (aiTimer == 180)
				{
					Main.PlaySound(SoundID.DD2_EtherianPortalSpawnEnemy, npc.Center);
				}
				if (aiTimer > 180 && aiTimer < 360)
				{
					DoDustEffect(npc.Center, 46f, 1.08f, 2.08f, npc);
					npc.velocity = Vector2.Zero;
					if (npc.velocity == Vector2.Zero)
					{
						npc.velocity.X = .008f * npc.direction;
						npc.velocity.Y = 12f;
					}
					HealingFrames();
				}
				else
				{
					WalkingFrames();
				}
				if (aiTimer == 360)
				{
					if (Main.netMode != NetmodeID.Server)
						Main.PlaySound(SoundLoader.customSoundType, npc.position, mod.GetSoundSlot(Terraria.ModLoader.SoundType.Custom, "Sounds/EnemyHeal"));
					npc.life += 10;
					npc.HealEffect(10, true);
				}
			}
			else
			{
				WalkingFrames();
			}
			if (aiTimer >= 360)
			{
				aiTimer = 0;
			}
		}
		public void WalkingFrames()
		{
			if (!npc.collideY && npc.velocity.Y > 0)
			{
				frame = 0;
			}
			else
			{
				if (frametimer >= 4)
				{
					frame++;
					frametimer = 0;
				}
				if (frame >= 10)
					frame = 0;
			}
		}
		public void HealingFrames()
		{
			if (frametimer >= 4)
			{
				frame++;
				frametimer = 0;
			}
			if (frame >= 16 || frame < 10)
				frame = 10;
		}
		public override void FindFrame(int frameHeight) => npc.frame.Y = frameHeight * frame;

		private void DoDustEffect(Vector2 position, float distance, float minSpeed = 2f, float maxSpeed = 3f, object follow = null)
		{
			float angle = Main.rand.NextFloat(-MathHelper.Pi, MathHelper.Pi);
			Vector2 vec = new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle));
			Vector2 vel = vec * Main.rand.NextFloat(minSpeed, maxSpeed);

			int dust = Dust.NewDust(position - vec * distance, 0, 0, DustID.TreasureSparkle);
			Main.dust[dust].noGravity = true;
			Main.dust[dust].scale *= .6f;
			Main.dust[dust].velocity = vel;
			Main.dust[dust].customData = follow;
		}
		public override void NPCLoot()
		{
			if (Main.rand.Next(20) == 1)
				Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ModContent.ItemType<SanctifiedStabber>());
            if (Main.rand.NextBool(33))
                Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ModContent.ItemType<CaesarSalad>());
            if (!Main.dayTime)
				Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ModContent.ItemType<EnchantedLeaf>());
		}

		public override void OnHitPlayer(Player target, int damage, bool crit)
		{
			if (Main.rand.Next(10) == 0 && Main.expertMode)
				target.AddBuff(148, 2000);
		}

		public override bool PreDraw(SpriteBatch spriteBatch, Color drawColor)
		{
			var effects = npc.direction == -1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
			spriteBatch.Draw(Main.npcTexture[npc.type], npc.Center - Main.screenPosition + new Vector2(0, npc.gfxOffY), npc.frame, drawColor, npc.rotation, npc.frame.Size() / 2, npc.scale, effects, 0);
			return false;
		}

		public override void PostDraw(SpriteBatch spriteBatch, Color drawColor) => GlowmaskUtils.DrawNPCGlowMask(spriteBatch, npc, mod.GetTexture("NPCs/Reach/Reachman_Glow"));

		public override void HitEffect(int hitDirection, double damage)
		{
			for (int k = 0; k < 30; k++) {
				Dust.NewDust(npc.position, npc.width, npc.height, DustID.GrassBlades, 2.5f * hitDirection, -2.5f, 0, Color.White, 0.7f);
				Dust.NewDust(npc.position, npc.width, npc.height, 7, 2.5f * hitDirection, -2.5f, 0, default, .34f);
			}

			if (npc.life <= 0) {
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/Reach1"), 1f);
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/Reach2"), 1f);
			}
		}
	}
}
