using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using SpiritMod.Items.Sets.BriarDrops;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using SpiritMod.Items.Consumable.Food;
using Terraria.Audio;
using System.IO;
using Terraria.ModLoader.Utilities;
using Terraria.GameContent.ItemDropRules;

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
			Main.npcFrameCount[NPC.type] = 16;
		}

		public override void SetDefaults()
		{
			NPC.width = 36;
			NPC.height = 52;
			NPC.damage = 22;
			NPC.defense = 8;
			NPC.lifeMax = 59;
			NPC.buffImmune[BuffID.Poisoned] = true;
			NPC.DeathSound = SoundID.NPCDeath2;
			NPC.value = 70f;
			NPC.knockBackResist = .34f;
			NPC.aiStyle = 3;
			AIType = NPCID.SnowFlinx;
			NPC.HitSound = SoundID.NPCHit2 with { PitchVariance = 0.2f };
			Banner = NPC.type;
            BannerItem = ModContent.ItemType<Items.Banners.ReachmanBanner>();
        }
		public override float SpawnChance(NPCSpawnInfo spawnInfo)
		{
			Player player = spawnInfo.Player;
			if (!(player.ZoneTowerSolar || player.ZoneTowerVortex || player.ZoneTowerNebula || player.ZoneTowerStardust) && !Main.pumpkinMoon && !Main.snowMoon && !Main.eclipse  && (SpawnCondition.GoblinArmy.Chance == 0))
				return spawnInfo.Player.GetSpiritPlayer().ZoneReach ? 1.7f : 0f;
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
			Lighting.AddLight((int)(NPC.Center.X / 16f), (int)(NPC.Center.Y / 16f), 0.23f, 0.16f, .05f);

			aiTimer++;
			frametimer++;

			if (NPC.life <= NPC.lifeMax - 20)
			{
				if (aiTimer == 180)
				{
					SoundEngine.PlaySound(SoundID.DD2_EtherianPortalSpawnEnemy, NPC.Center);
				}
				if (aiTimer > 180 && aiTimer < 360)
				{
					DoDustEffect(NPC.Center, 46f, 1.08f, 2.08f, NPC);
					NPC.velocity = Vector2.Zero;
					if (NPC.velocity == Vector2.Zero)
					{
						NPC.velocity.X = .008f * NPC.direction;
						NPC.velocity.Y = 12f;
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
						SoundEngine.PlaySound(new SoundStyle("SpiritMod/Sounds/EnemyHeal"), NPC.Center);
					NPC.life += 10;
					NPC.HealEffect(10, true);
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
			if (!NPC.collideY && NPC.velocity.Y > 0)
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
		public override void FindFrame(int frameHeight) => NPC.frame.Y = frameHeight * frame;

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

		public override void ModifyNPCLoot(NPCLoot npcLoot)
		{
			npcLoot.AddCommon<SanctifiedStabber>(20);
			npcLoot.AddFood<CaesarSalad>(33);

			LeadingConditionRule notDay = new LeadingConditionRule(new DropRuleConditions.NotDay());
			notDay.OnSuccess(ItemDropRule.Common(ModContent.ItemType<EnchantedLeaf>()));
			npcLoot.Add(notDay);
		}

		public override void OnHitPlayer(Player target, int damage, bool crit)
		{
			if (Main.rand.Next(10) == 0 && Main.expertMode)
				target.AddBuff(148, 2000);
		}

		public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
		{
			var effects = NPC.direction == -1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
			spriteBatch.Draw(TextureAssets.Npc[NPC.type].Value, NPC.Center - Main.screenPosition + new Vector2(0, NPC.gfxOffY), NPC.frame, drawColor, NPC.rotation, NPC.frame.Size() / 2, NPC.scale, effects, 0);
			return false;
		}

		public override void PostDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor) => GlowmaskUtils.DrawNPCGlowMask(spriteBatch, NPC, Mod.Assets.Request<Texture2D>("NPCs/Reach/Reachman_Glow").Value, screenPos);

		public override void HitEffect(int hitDirection, double damage)
		{
			for (int k = 0; k < 30; k++) {
				Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.GrassBlades, 2.5f * hitDirection, -2.5f, 0, Color.White, 0.7f);
				Dust.NewDust(NPC.position, NPC.width, NPC.height, 7, 2.5f * hitDirection, -2.5f, 0, default, .34f);
			}

			if (NPC.life <= 0) {
				Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, Mod.Find<ModGore>("Reach1").Type, 1f);
				Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, Mod.Find<ModGore>("Reach2").Type, 1f);
			}
		}
	}
}
