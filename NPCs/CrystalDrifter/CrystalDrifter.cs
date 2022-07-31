using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpiritMod.Items.Sets.CryoliteSet;
using SpiritMod.Mechanics.QuestSystem;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent.Bestiary;

namespace SpiritMod.NPCs.CrystalDrifter
{
	public class CrystalDrifter : ModNPC
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Crystal Drifter");
			Main.npcFrameCount[NPC.type] = 12;
			NPCID.Sets.TrailCacheLength[NPC.type] = 3;
			NPCID.Sets.TrailingMode[NPC.type] = 0;
		}

		public override void SetDefaults()
		{
			NPC.width = 44;
			NPC.height = 88;
			NPC.damage = 27;
			NPC.defense = 17;
			NPC.lifeMax = 200;
			NPC.HitSound = SoundID.NPCDeath15;
			NPC.DeathSound = SoundID.NPCDeath6;
			NPC.buffImmune[BuffID.OnFire] = true;
			NPC.buffImmune[BuffID.Frostburn] = true;
			NPC.buffImmune[BuffID.Poisoned] = true;
			NPC.buffImmune[BuffID.Venom] = true;
			NPC.value = 200f;
			NPC.knockBackResist = 0f;
			NPC.alpha = 100;
			NPC.noGravity = true;
			NPC.noTileCollide = true;
			NPC.aiStyle = 22;
			NPC.aiStyle = -1;
			Banner = NPC.type;
			BannerItem = ModContent.ItemType<Items.Banners.CrystalDrifterBanner>();
		}

		public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
		{
			bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Snow,
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Events.Blizzard,
				new FlavorTextBestiaryInfoElement("The howling winds of a blizzard give flight to this otherwise cumbersome entity composed of ice."),
			});
		}

		public override void FindFrame(int frameHeight)
		{
			NPC.frameCounter += 0.08f;
			NPC.frameCounter %= Main.npcFrameCount[NPC.type];
			int frame = (int)NPC.frameCounter;
			NPC.frame.Y = frame * frameHeight;
		}

		public override bool PreAI()
		{
			NPC.TargetClosest(true);
			NPC.spriteDirection = -NPC.direction;
			NPC.spriteDirection = NPC.direction;
			Lighting.AddLight((int)(NPC.Center.X / 16f), (int)(NPC.Center.Y / 16f), .5f, .36f, .14f);

			Player target = Main.player[NPC.target];
			MyPlayer modPlayer = target.GetSpiritPlayer();

			float distance = NPC.DistanceSQ(target.Center);
			if (distance < 500 * 500 && Main.myPlayer == target.whoAmI)
			{
				target.AddBuff(BuffID.WindPushed, 90);

				if (Main.netMode != NetmodeID.MultiplayerClient)
					modPlayer.windEffect2 = true;
			}

			NPC.ai[0]++;

			const float VelMagnitude = 5f;

			Vector2 vel = Main.player[NPC.target].Center - NPC.Center + new Vector2(0f, Main.rand.NextFloat(-200f, -150f));
			float length = vel.Length();

			Vector2 desiredVelocity;
			if (length < 20)
				desiredVelocity = NPC.velocity;
			else if (length < 40)
				desiredVelocity = Vector2.Normalize(vel) * (VelMagnitude * 0.35f);
			else if (length < 80)
				desiredVelocity = Vector2.Normalize(vel) * (VelMagnitude * 0.65f);
			else
				desiredVelocity = Vector2.Normalize(vel) * VelMagnitude;

			NPC.SimpleFlyMovement(desiredVelocity, 0.08f);
			NPC.rotation = NPC.velocity.X * 0.1f;

			if (NPC.ai[0] >= 90 && Main.netMode != NetmodeID.MultiplayerClient)
			{
				Vector2 velocity = Vector2.UnitY.RotatedByRandom(MathHelper.PiOver2) * new Vector2(5f, 3f);
				int damage = Main.expertMode ? 12 : 18;
				int p = Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center.X, NPC.Center.Y, velocity.X, velocity.Y, ModContent.ProjectileType<FrostOrbiterHostile>(), damage, 0.0f, Main.myPlayer, 0.0f, NPC.whoAmI);
				Main.projectile[p].hostile = true;

				NPC.ai[0] = 0;
				NPC.netUpdate = true;
			}
			return false;
		}

		public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
		{
			Vector2 drawOrigin = new Vector2(TextureAssets.Npc[NPC.type].Value.Width * 0.5f, NPC.height * 0.5f);
			for (int k = 0; k < NPC.oldPos.Length; k++)
			{
				var effects = NPC.direction == -1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
				Vector2 drawPos = NPC.oldPos[k] - Main.screenPosition + drawOrigin + new Vector2(0f, NPC.gfxOffY);
				Color color = NPC.GetAlpha(drawColor) * (((NPC.oldPos.Length - k) / (float)NPC.oldPos.Length) / 2);
				spriteBatch.Draw(TextureAssets.Npc[NPC.type].Value, drawPos, new Microsoft.Xna.Framework.Rectangle?(NPC.frame), color, NPC.rotation, drawOrigin, NPC.scale, effects, 0f);
			}
			return true;
		}

		public override float SpawnChance(NPCSpawnInfo spawnInfo) => spawnInfo.Player.ZoneOverworldHeight && spawnInfo.Player.ZoneSnow && Main.raining && !spawnInfo.PlayerSafe && !NPC.AnyNPCs(ModContent.NPCType<CrystalDrifter>()) && NPC.downedBoss3 ? 0.09f : 0f;

		public override void HitEffect(int hitDirection, double damage)
		{
			SoundEngine.PlaySound(SoundID.Item51, NPC.Center);

			for (int k = 0; k < 20; k++)
				Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.BlueCrystalShard, hitDirection * 2f, -1f, 0, default, 1f);

			if (NPC.life <= 0)
			{
				for (int i = 1; i < 5; ++i)
					Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, Mod.Find<ModGore>("Drifter" + i).Type, .5f);

				SoundEngine.PlaySound(SoundID.NPCHit41, NPC.Center);
				NPC.position.X = NPC.position.X + (NPC.width / 2);
				NPC.position.Y = NPC.position.Y + (NPC.height / 2);
				NPC.width = 30;
				NPC.height = 30;
				NPC.position.X = NPC.position.X - (NPC.width / 2);
				NPC.position.Y = NPC.position.Y - (NPC.height / 2);
				for (int num621 = 0; num621 < 20; num621++)
				{
					int num622 = Dust.NewDust(new Vector2(NPC.position.X, NPC.position.Y), NPC.width, NPC.height, DustID.BlueCrystalShard, 0f, 0f, 100, default, 1f);
					Main.dust[num622].velocity *= 3f;
					if (Main.rand.NextBool(2))
					{
						Main.dust[num622].scale = 0.5f;
						Main.dust[num622].fadeIn = 1f + Main.rand.Next(10) * 0.1f;
					}
				}
				for (int num623 = 0; num623 < 40; num623++)
				{
					int dust = Dust.NewDust(new Vector2(NPC.position.X, NPC.position.Y), NPC.width, NPC.height, DustID.BlueCrystalShard, 0f, 0f, 100, default, 1f);
					Main.dust[dust].noGravity = true;
					Main.dust[dust].velocity *= 5f;
					dust = Dust.NewDust(new Vector2(NPC.position.X, NPC.position.Y), NPC.width, NPC.height, DustID.BlueCrystalShard, 0f, 0f, 100, default, 1f);
					Main.dust[dust].velocity *= 2f;
				}
			}
		}

		public override void OnHitPlayer(Player target, int damage, bool crit)
		{
			if (Main.rand.NextBool(2))
				target.AddBuff(BuffID.Frostburn, 150);
		}

		public override void OnKill()
		{
			if (QuestManager.GetQuest<Mechanics.QuestSystem.Quests.IceDeityQuest>().IsActive) //Quest item not a loot item
				Item.NewItem(NPC.GetSource_Death(), NPC.Center, ModContent.ItemType<Items.Sets.MaterialsMisc.QuestItems.IceDeityShard2>());
		}

		public override void ModifyNPCLoot(NPCLoot npcLoot) => npcLoot.AddCommon<CryoliteOre>(1, 9, 14);
	}
}