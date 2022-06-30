using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.GameContent;
using Terraria.Graphics.Effects;
using Terraria.ModLoader;
using Terraria.ID;
using SpiritMod.Mechanics.QuestSystem;
using SpiritMod.Mechanics.QuestSystem.Quests;
using Terraria.ModLoader.Utilities;

namespace SpiritMod.NPCs.Cystal
{
	public class Cystal : ModNPC
	{
		public int colorNumber = 510;
		public int dustTimer = 0;
		public int healTimer = 0;
		public bool check = false;
		public bool shieldSpawned = false;

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Cystal");
			Main.npcFrameCount[NPC.type] = 8;
		}

		public override void SetDefaults()
		{
			NPC.aiStyle = 0;
			NPC.lifeMax = 82;
			NPC.defense = 2;
			NPC.value = 500f;
			NPC.knockBackResist = 0.1f;
			NPC.width = 40;
			NPC.height = 40;
			NPC.buffImmune[BuffID.Confused] = true;
			NPC.damage = 20;
			NPC.lavaImmune = true;
			NPC.noGravity = true;
			NPC.noTileCollide = true;
			NPC.HitSound = new Terraria.Audio.LegacySoundStyle(42, 193);
			NPC.DeathSound = new Terraria.Audio.LegacySoundStyle(42, 182);
			Banner = NPC.type;
			BannerItem = ModContent.ItemType<Items.Banners.CystalBanner>();
		}

		public override void AI()
		{
			dustTimer++;
			healTimer++;

			NPC.TargetClosest(false);
			Player player = Main.player[NPC.target];
			NPC.spriteDirection = NPC.direction;

			Lighting.AddLight(NPC.position, 0.0149f / 4 * NPC.life, 0.0142f / 4 * NPC.life, 0.0207f / 4 * NPC.life);

			if (NPC.active && player.active && !shieldSpawned)
			{
				shieldSpawned = true;
				int shield1 = NPC.NewNPC(NPC.GetSource_FromAI(), (int)NPC.Center.X, (int)NPC.Center.Y + (NPC.height / 2), ModContent.NPCType<Cystal_Shield>());
				Main.npc[shield1].Center += new Vector2(0, 150).RotatedBy(MathHelper.ToRadians(90));
				Main.npc[shield1].ai[1] = NPC.whoAmI;

				int shield2 = NPC.NewNPC(NPC.GetSource_FromAI(), (int)NPC.Center.X, (int)NPC.Center.Y + (NPC.height / 2), ModContent.NPCType<Cystal_Shield>());
				Main.npc[shield2].Center += new Vector2(0, 150).RotatedBy(MathHelper.ToRadians(180));
				Main.npc[shield2].ai[1] = NPC.whoAmI;

				int shield3 = NPC.NewNPC(NPC.GetSource_FromAI(), (int)NPC.Center.X, (int)NPC.Center.Y + (NPC.height / 2), ModContent.NPCType<Cystal_Shield>());
				Main.npc[shield3].Center += new Vector2(0, 150).RotatedBy(MathHelper.ToRadians(270));
				Main.npc[shield3].ai[1] = NPC.whoAmI;

				int shield4 = NPC.NewNPC(NPC.GetSource_FromAI(), (int)NPC.Center.X, (int)NPC.Center.Y + (NPC.height / 2), ModContent.NPCType<Cystal_Shield>());
				Main.npc[shield4].Center += new Vector2(0, 150).RotatedBy(MathHelper.ToRadians(360));
				Main.npc[shield4].ai[1] = NPC.whoAmI;
			}

			if (NPC.DistanceSQ(player.Center) < 150 * 150 && Main.myPlayer == player.whoAmI)
			{
				Filters.Scene.Activate("CystalTower", player.position);
				Filters.Scene.Activate("CystalBloodMoon", player.position);
				player.AddBuff(ModContent.BuffType<Crystallization>(), 2);
			}
			else
			{
				Filters.Scene.Deactivate("CystalTower", player.position);
				Filters.Scene.Deactivate("CystalBloodMoon", player.position);
			}

			for (int i = 0; i < 200; i++)
			{
				NPC other = Main.npc[i];

				if (other.active && other.type != ModContent.NPCType<Cystal>() && other.type != ModContent.NPCType<Cystal_Shield>())
				{
					if (NPC.DistanceSQ(other.Center) < 150 * 150 && healTimer % 60 == 0)
					{
						if (other.life < other.lifeMax - 10)
						{
							other.life += 10;
							other.HealEffect(10, true);
						}
					}

					if (NPC.DistanceSQ(other.Center) < 1200 * 1200 && other.life < other.lifeMax - 10 && other.type == Main.npc[(int)NPC.ai[1]].type && other.active && !other.friendly)
					{
						NPC.SimpleFlyMovement(NPC.DirectionTo(Main.npc[(int)NPC.ai[1]].Center + new Vector2(0.0f, -100f)) * 5f, 0.9f);
						return;
					}
				}
			}

			float playerDistance = player.DistanceSQ(NPC.Center);
			if (playerDistance <= 150 * 150f && player.active)
			{
				float x = player.Center.X - NPC.Center.X;
				float y = player.Center.Y - NPC.Center.Y;
				float length = 8f / (float)Math.Sqrt(x * x + y * y);
				NPC.velocity.X = x * length * -1 * 0.3f;
				NPC.velocity.Y = y * length * -1 * 0.3f;

				NPC.spriteDirection = -1;
				NPC.direction = -1;
			}

			if (playerDistance <= 150 * 150f && playerDistance > 100 * 100f)
				NPC.velocity = Vector2.Zero;

			if (playerDistance > 150f && player.active)
			{
				const float MoveSpeed = 0.25f;

				float x = player.Center.X - NPC.Center.X;
				float y = player.Center.Y - NPC.Center.Y;
				float distance = x * x + y * y;

				if (player == Main.player[NPC.target])
				{
					float oldVelX = NPC.velocity.X;
					float oldVelY = NPC.velocity.Y;

					if (distance >= 20 * 20f)
					{
						float magnitude = 4f / distance;
						oldVelX = x * magnitude;
						oldVelY = y * magnitude;
					}

					if (NPC.velocity.X < oldVelX)
					{
						NPC.velocity.X += MoveSpeed;
						if (NPC.velocity.X < 0 && oldVelX > 0)
							NPC.velocity.X += MoveSpeed * 2f;
					}
					else if (NPC.velocity.X > oldVelX)
					{
						NPC.velocity.X -= MoveSpeed;
						if (NPC.velocity.X > 0 && oldVelX < 0)
							NPC.velocity.X -= MoveSpeed * 2f;
					}
					if (NPC.velocity.Y < oldVelY)
					{
						NPC.velocity.Y += MoveSpeed;
						if (NPC.velocity.Y < 0 && oldVelY > 0)
							NPC.velocity.Y += MoveSpeed * 2f;
					}
					else if (NPC.velocity.Y > oldVelY)
					{
						NPC.velocity.Y -= MoveSpeed;
						if (NPC.velocity.Y > 0 && oldVelY < 0)
							NPC.velocity.Y -= MoveSpeed * 2f;
					}
				}
			}
		}

		public override void HitEffect(int hitDirection, double damage)
		{
			if (NPC.life <= 0)
			{
				for (int i = 1; i < 5; ++i)
					Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, Mod.Find<ModGore>("Gores/CystalGore" + i).Type, 1f);

				Player player = Main.player[NPC.target];
				Filters.Scene.Deactivate("CystalTower", player.position);
				Filters.Scene.Deactivate("CystalBloodMoon", player.position);
			}

			for (int k = 0; k < 7; k++)
			{
				Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.ScourgeOfTheCorruptor, 2.5f * hitDirection, -2.5f, 0, default, 1.2f);
				Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.ScourgeOfTheCorruptor, 2.5f * hitDirection, -2.5f, 0, default, 0.5f);
				Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.PurpleCrystalShard, 2.5f * hitDirection, -2.5f, 0, default, 0.7f);
			}
		}

		public override float SpawnChance(NPCSpawnInfo spawnInfo)
		{
			bool valid = !NPC.AnyNPCs(ModContent.NPCType<Cystal>()) && (NPC.downedBoss1 || NPC.downedBoss2 || NPC.downedBoss3 || MyWorld.downedScarabeus || MyWorld.downedReachBoss || MyWorld.downedRaider || MyWorld.downedAncientFlier);
			if (!valid)
				return 0;
			return SpawnCondition.Corruption.Chance * 0.065f;
		}

		public override void OnKill()
		{
			Filters.Scene.Deactivate("CystalTower", Main.player[NPC.target].position);
			Filters.Scene.Deactivate("CystalBloodMoon", Main.player[NPC.target].position);

			//if (QuestManager.GetQuest<StylistQuestCorrupt>().IsActive)
			//	Item.NewItem(npc.Center, ModContent.ItemType<Items.Sets.MaterialsMisc.QuestItems.CorruptDyeMaterial>());
		}

		public override void ModifyNPCLoot(NPCLoot npcLoot) => npcLoot.AddCommon(68, 2, 1, 2);

		public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
		{
			if (colorNumber >= 510)
				check = true;
			else if (colorNumber <= 300)
				check = false;

			if (check)
				colorNumber -= 10;
			else if (!check)
				colorNumber += 10;

			var effects = NPC.direction == -1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
			spriteBatch.Draw(TextureAssets.Npc[NPC.type].Value, NPC.Center - Main.screenPosition + new Vector2(0, NPC.gfxOffY), NPC.frame, drawColor, NPC.rotation, NPC.frame.Size() / 2, NPC.scale, effects, 0);
			return false;
		}

		public override void PostDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
		{
			var effects = NPC.direction == -1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
			spriteBatch.Draw(Mod.Assets.Request<Texture2D>("NPCs/Cystal/Cystal_Glow").Value, NPC.Center - Main.screenPosition + new Vector2(0, NPC.gfxOffY), NPC.frame, new Color(colorNumber / 2, colorNumber / 2, colorNumber / 2), NPC.rotation, NPC.frame.Size() / 2, NPC.scale, effects, 0);
		}

		public override void FindFrame(int frameHeight)
		{
			if (NPC.active && Main.player[NPC.target].active)
			{
				NPC.frameCounter++;
				if (NPC.frameCounter < 6)
					NPC.frame.Y = 0 * frameHeight;
				else if (NPC.frameCounter < 12)
					NPC.frame.Y = 1 * frameHeight;
				else if (NPC.frameCounter < 18)
					NPC.frame.Y = 2 * frameHeight;
				else if (NPC.frameCounter < 24)
					NPC.frame.Y = 3 * frameHeight;
				else if (NPC.frameCounter < 30)
					NPC.frame.Y = 4 * frameHeight;
				else if (NPC.frameCounter < 36)
					NPC.frame.Y = 5 * frameHeight;
				else if (NPC.frameCounter < 42)
					NPC.frame.Y = 6 * frameHeight;
				else if (NPC.frameCounter < 48)
					NPC.frame.Y = 7 * frameHeight;
				else
					NPC.frameCounter = 0;
			}
		}
	}
}