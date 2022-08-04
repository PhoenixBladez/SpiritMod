using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpiritMod.Items.Accessory;
using SpiritMod.Items.DonatorItems.FrostTroll;
using System.IO;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent.Bestiary;

namespace SpiritMod.NPCs.Boss.FrostTroll
{
	[AutoloadBossHead]
	public class FrostSaucer : ModNPC
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Snow Monger");
			NPCID.Sets.TrailCacheLength[NPC.type] = 5;
			NPCID.Sets.TrailingMode[NPC.type] = 0;
			Main.npcFrameCount[NPC.type] = 5;
		}

		public override void SetDefaults()
		{
			NPC.width = 344;
			NPC.height = 298;
			NPC.damage = 45;
			NPC.lifeMax = 5600;
			NPC.knockBackResist = 0;
			NPC.noGravity = true;
			NPC.noTileCollide = true;
			NPC.HitSound = SoundID.NPCHit4;
			NPC.DeathSound = SoundID.NPCDeath5;

			for (int k = 0; k < NPC.buffImmune.Length; k++)
				NPC.buffImmune[k] = true;
		}

		public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
		{
			bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Snow,
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Invasions.FrostLegion,
				new FlavorTextBestiaryInfoElement("A dangerous hovercraft made of spine-chillingly cold metal, and powered by Frost Cores. Playtime’s over, no more mister ice guy!"),
			});
		}

		public override void FindFrame(int frameHeight)
		{
			NPC.frameCounter += .15f;
			NPC.frameCounter %= Main.npcFrameCount[NPC.type];
			int frame = (int)NPC.frameCounter;
			NPC.frame.Y = frame * frameHeight;
		}

		bool trailBehind;
		bool canHitPlayer;

		public override void HitEffect(int hitDirection, double damage)
		{
			if (NPC.life <= 0)
			{
				SoundEngine.PlaySound(SoundID.Item14, NPC.Center);
				for (int num625 = 0; num625 < 10; num625++)
				{
					float scaleFactor10 = 0.2f;
					if (num625 == 1)
						scaleFactor10 = 0.5f;
					if (num625 == 2)
						scaleFactor10 = 1f;
					int num626 = Gore.NewGore(NPC.GetSource_OnHit(NPC), new Vector2(NPC.Center.X + Main.rand.Next(-100, 100), NPC.Center.Y + Main.rand.Next(-100, 100)), default, Main.rand.Next(61, 64), 1f);
					Main.gore[num626].velocity *= scaleFactor10;
					Gore expr_13AB6_cp_0 = Main.gore[num626];
					expr_13AB6_cp_0.velocity.X += 1f;
					Gore expr_13AD6_cp_0 = Main.gore[num626];
					expr_13AD6_cp_0.velocity.Y += 1f;
					num626 = Gore.NewGore(NPC.GetSource_OnHit(NPC), new Vector2(NPC.Center.X + Main.rand.Next(-100, 100), NPC.Center.Y + Main.rand.Next(-100, 100)), default, Main.rand.Next(61, 64), 1f);
					Main.gore[num626].velocity *= scaleFactor10;
					Gore expr_13B79_cp_0 = Main.gore[num626];
					expr_13B79_cp_0.velocity.X -= 1f;
					Gore expr_13B99_cp_0 = Main.gore[num626];
					expr_13B99_cp_0.velocity.Y += 1f;
				}

				for (int j = 0; j < 17; j++)
					Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Electric, 2.5f * hitDirection, -2.5f, 0, default, 0.75f);
			}

			for (int k = 0; k < 7; k++)
				Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Electric, 2.5f * hitDirection, -2.5f, 0, default, 0.5f);
		}

		public override void AI()
		{
			bool expertMode = Main.expertMode;
			float numYPos = -280f;
			NPC.ai[0]++;
			Lighting.AddLight(new Vector2(NPC.Center.X, NPC.Center.Y), 0.075f, 0.231f, 0.255f);
			Player player = Main.player[NPC.target];
			if (NPC.ai[0] < 420 || NPC.ai[0] > 490)
			{
				if (player.position.X > NPC.position.X)
					NPC.spriteDirection = 1;
				else
					NPC.spriteDirection = -1;

				NPC.aiStyle = -1;
				float num1 = 4.7f;
				float moveSpeed = 0.13f;
				if (NPC.ai[0] > 320 && NPC.ai[0] < 400)
				{
					num1 = 14f;
					moveSpeed = .5f;
				}
				if (NPC.ai[0] > 580 && NPC.ai[0] < 638)
				{
					trailBehind = true;
					canHitPlayer = true;
					numYPos = 0f;
					num1 = 10.5f;
					moveSpeed = .4f;
				}
				else
				{
					canHitPlayer = false;
					trailBehind = false;
				}
				NPC.TargetClosest(true);
				Vector2 vector2_1 = Main.player[NPC.target].Center - NPC.Center + new Vector2(0.0f, numYPos);
				float num2 = vector2_1.Length();
				Vector2 desiredVelocity;
				if (num2 < 20.0)
					desiredVelocity = NPC.velocity;
				else if (num2 < 40.0)
				{
					vector2_1.Normalize();
					desiredVelocity = vector2_1 * (num1 * 0.45f);
				}
				else if (num2 < 80.0)
				{
					vector2_1.Normalize();
					desiredVelocity = vector2_1 * (num1 * 0.75f);
				}
				else
				{
					vector2_1.Normalize();
					desiredVelocity = vector2_1 * num1;
				}
				NPC.SimpleFlyMovement(desiredVelocity, moveSpeed);
			}

			if (NPC.ai[0] == 60 || NPC.ai[0] == 150 || NPC.ai[0] == 220 || NPC.ai[0] == 280)
			{
				for (int i = 0; i < 12; i++)
				{
					Dust dust = Dust.NewDustDirect(NPC.Center + new Vector2(-80, 70), NPC.width, NPC.height, DustID.BlueCrystalShard);
					dust.velocity *= -1f;
					dust.scale *= .8f;
					dust.noGravity = true;
					Vector2 vector2_1 = new Vector2(Main.rand.Next(-80, 81), Main.rand.Next(-80, 81));
					vector2_1.Normalize();
					Vector2 vector2_2 = vector2_1 * (Main.rand.Next(50, 100) * 0.04f);
					dust.velocity = vector2_2;
					vector2_2.Normalize();
					Vector2 vector2_3 = vector2_2 * 34f;
					dust.position = NPC.Center + new Vector2(-80, 70) - vector2_3;

					Dust dust1 = Dust.NewDustDirect(NPC.Center + new Vector2(80, 70), NPC.width, NPC.height, DustID.BlueCrystalShard);
					dust1.velocity *= -1f;
					dust1.scale *= .8f;
					dust1.noGravity = true;
					dust1.velocity = vector2_2;
					dust1.position = NPC.Center + new Vector2(80, 70) - vector2_3;
				}

				if (Main.netMode != NetmodeID.MultiplayerClient)
				{
					Vector2 direction = Main.player[NPC.target].Center - (NPC.Center + new Vector2(80, 70));
					direction.Normalize();
					direction.X *= 7.5f;
					direction.Y *= 7.5f;
					int somedamage = expertMode ? 25 : 38;
					int p = Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center.X + 80, NPC.Center.Y + 70, direction.X, direction.Y, ProjectileID.IceBolt, somedamage, 1, Main.myPlayer, 0, 0);
					Main.projectile[p].hostile = true;
					Main.projectile[p].friendly = false;
					Main.projectile[p].timeLeft = 280;

					Vector2 direction1 = Main.player[NPC.target].Center - (NPC.Center + new Vector2(-80, 70));
					direction1.Normalize();
					direction1.X *= 7.5f;
					direction1.Y *= 7.5f;
					int p1 = Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center.X - 80, NPC.Center.Y + 70, direction1.X, direction1.Y, ProjectileID.IceBolt, somedamage, 1, Main.myPlayer, 0, 0);
					Main.projectile[p1].hostile = true;
					Main.projectile[p1].friendly = false;
					Main.projectile[p1].timeLeft = 280;
				}
			}

			if (NPC.ai[0] == 435)
			{
				NPC.velocity = Vector2.Zero;
				NPC.netUpdate = true;
			}

			if (NPC.ai[0] > 435 && NPC.ai[0] < 445)
			{
				if (Main.netMode != NetmodeID.Server)
				{
					Dust dust = Dust.NewDustDirect(new Vector2(NPC.Center.X, NPC.Center.Y + 120), NPC.width, NPC.height, DustID.Electric);
					dust.velocity *= -1f;
					dust.scale *= .8f;
					dust.noGravity = true;
					Vector2 vector2_1 = new Vector2(Main.rand.Next(-80, 81), Main.rand.Next(-80, 81));
					vector2_1.Normalize();
					Vector2 vector2_2 = vector2_1 * (Main.rand.Next(50, 100) * 0.04f);
					dust.velocity = vector2_2;
					vector2_2.Normalize();
					Vector2 vector2_3 = vector2_2 * 34f;
					dust.position = new Vector2(NPC.Center.X, NPC.Center.Y + 120) - vector2_3;
				}
			}

			if (NPC.ai[0] == 445 || NPC.ai[0] == 455 || NPC.ai[0] == 465)
			{
				SoundEngine.PlaySound(SoundID.Item91, NPC.Center);
				if (Main.netMode != NetmodeID.MultiplayerClient)
					Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center.X, NPC.Center.Y + 70, 0, 26, ModContent.ProjectileType<SnowMongerBeam>(), 70, 1, Main.myPlayer, 0, 0);
			}

			if (NPC.ai[0] == 570)
				SoundEngine.PlaySound(SoundID.Roar, NPC.Center);

			if (NPC.ai[0] >= 650)
			{
				NPC.ai[0] = 0;
				NPC.netUpdate = true;
			}
		}

		public override bool CanHitPlayer(Player target, ref int cooldownSlot) => canHitPlayer;
		public override void SendExtraAI(BinaryWriter writer) => writer.Write(trailBehind);
		public override void ReceiveExtraAI(BinaryReader reader) => trailBehind = reader.ReadBoolean();

		public override void OnKill()
		{
			if (Main.invasionType == 2)
			{
				Main.invasionSize -= 10;
				if (Main.invasionSize < 0)
					Main.invasionSize = 0;
				if (Main.netMode != NetmodeID.MultiplayerClient)
					Main.ReportInvasionProgress(Main.invasionSizeStart - Main.invasionSize, Main.invasionSizeStart, 1, 0);
				if (Main.netMode == NetmodeID.Server)
					NetMessage.SendData(MessageID.InvasionProgressReport, -1, -1, null, Main.invasionProgress, Main.invasionProgressMax, Main.invasionProgressIcon, 0f, 0, 0, 0);
			}

			int[] lootTable = {
				ModContent.ItemType<Bauble>(),
				ModContent.ItemType<BlizzardEdge>(),
				ModContent.ItemType<Chillrend>(),
				ModContent.ItemType<ShiverWind>()
			};

			int loot = Main.rand.Next(lootTable.Length);
			Item.NewItem(NPC.GetSource_Death(), (int)NPC.position.X, (int)NPC.position.Y, NPC.width, NPC.height, lootTable[loot]);

			for (int i = 0; i < 15; ++i)
			{
				if (Main.rand.NextBool(8))
				{
					int newDust = Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Snow, 0f, 0f, 100, default, 2.5f);
					Main.dust[newDust].noGravity = true;
					Main.dust[newDust].velocity *= 5f;
				}
			}
		}

		public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
		{
			var effects = NPC.direction == -1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
			Vector2 drawOrigin = new Vector2(TextureAssets.Npc[NPC.type].Value.Width * 0.5f, (NPC.height / Main.npcFrameCount[NPC.type]) * 0.5f);

			if (trailBehind)
			{
				for (int k = 0; k < NPC.oldPos.Length; k++)
				{
					Vector2 drawPos = NPC.oldPos[k] - screenPos + new Vector2(NPC.width / 2, NPC.height / 2) + new Vector2(-10 * NPC.spriteDirection, NPC.gfxOffY - 90).RotatedBy(NPC.rotation);
					Color color = NPC.GetAlpha(drawColor) * (float)(((float)(NPC.oldPos.Length - k) / (float)NPC.oldPos.Length) / 2);
					Main.EntitySpriteDraw(TextureAssets.Npc[NPC.type].Value, drawPos, NPC.frame, color, NPC.rotation, drawOrigin, NPC.scale, effects, 0);
				}
			}

			Main.EntitySpriteDraw(TextureAssets.Npc[NPC.type].Value, NPC.Center - screenPos + new Vector2(0, NPC.gfxOffY), NPC.frame, drawColor, NPC.rotation, NPC.frame.Size() / 2, NPC.scale, effects, 0);
			return false;
		}

		public override float SpawnChance(NPCSpawnInfo spawnInfo) => Main.invasionType == 2 && !NPC.AnyNPCs(ModContent.NPCType<FrostSaucer>()) && spawnInfo.Player.ZoneOverworldHeight ? 0.018f : 0f;
		public override void BossLoot(ref string name, ref int potionType) => potionType = ItemID.GreaterHealingPotion;
	}
}