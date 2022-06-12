using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.Graphics.Effects;
using Terraria.ModLoader;
using Terraria.ID;
using SpiritMod.Mechanics.QuestSystem;
using SpiritMod.Mechanics.QuestSystem.Quests;

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
			Main.npcFrameCount[npc.type] = 8;
		}

		public override void SetDefaults()
		{
			npc.aiStyle = 0;
			npc.lifeMax = 82;
			npc.defense = 2;
			npc.value = 500f;
			npc.knockBackResist = 0.1f;
			npc.width = 40;
			npc.height = 40;
			npc.buffImmune[BuffID.Confused] = true;
			npc.damage = 20;
			npc.lavaImmune = true;
			npc.noGravity = true;
			npc.noTileCollide = true;
			npc.HitSound = new Terraria.Audio.LegacySoundStyle(42, 193);
			npc.DeathSound = new Terraria.Audio.LegacySoundStyle(42, 182);
			banner = npc.type;
			bannerItem = ModContent.ItemType<Items.Banners.CystalBanner>();
		}

		public override void AI()
		{
			dustTimer++;
			healTimer++;

			npc.TargetClosest(false);
			Player player = Main.player[npc.target];
			npc.spriteDirection = npc.direction;

			Lighting.AddLight(npc.position, 0.0149f / 4 * npc.life, 0.0142f / 4 * npc.life, 0.0207f / 4 * npc.life);

			if (npc.active && player.active && !shieldSpawned)
			{
				shieldSpawned = true;
				int shield1 = NPC.NewNPC((int)npc.Center.X, (int)npc.Center.Y + (npc.height / 2), ModContent.NPCType<Cystal_Shield>());
				Main.npc[shield1].Center += new Vector2(0, 150).RotatedBy(MathHelper.ToRadians(90));
				Main.npc[shield1].ai[1] = npc.whoAmI;

				int shield2 = NPC.NewNPC((int)npc.Center.X, (int)npc.Center.Y + (npc.height / 2), ModContent.NPCType<Cystal_Shield>());
				Main.npc[shield2].Center += new Vector2(0, 150).RotatedBy(MathHelper.ToRadians(180));
				Main.npc[shield2].ai[1] = npc.whoAmI;

				int shield3 = NPC.NewNPC((int)npc.Center.X, (int)npc.Center.Y + (npc.height / 2), ModContent.NPCType<Cystal_Shield>());
				Main.npc[shield3].Center += new Vector2(0, 150).RotatedBy(MathHelper.ToRadians(270));
				Main.npc[shield3].ai[1] = npc.whoAmI;

				int shield4 = NPC.NewNPC((int)npc.Center.X, (int)npc.Center.Y + (npc.height / 2), ModContent.NPCType<Cystal_Shield>());
				Main.npc[shield4].Center += new Vector2(0, 150).RotatedBy(MathHelper.ToRadians(360));
				Main.npc[shield4].ai[1] = npc.whoAmI;
			}

			if (npc.DistanceSQ(player.Center) < 150 * 150 && Main.myPlayer == player.whoAmI)
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
					if (npc.DistanceSQ(other.Center) < 150 * 150 && healTimer % 60 == 0)
					{
						if (other.life < other.lifeMax - 10)
						{
							other.life += 10;
							other.HealEffect(10, true);
						}
					}

					if (npc.DistanceSQ(other.Center) < 1200 * 1200 && other.life < other.lifeMax - 10 && other.type == Main.npc[(int)npc.ai[1]].type && other.active && !other.friendly)
					{
						npc.SimpleFlyMovement(npc.DirectionTo(Main.npc[(int)npc.ai[1]].Center + new Vector2(0.0f, -100f)) * 5f, 0.9f);
						return;
					}
				}
			}

			float playerDistance = player.DistanceSQ(npc.Center);
			if (playerDistance <= 150 * 150f && player.active)
			{
				float x = player.Center.X - npc.Center.X;
				float y = player.Center.Y - npc.Center.Y;
				float length = 8f / (float)Math.Sqrt(x * x + y * y);
				npc.velocity.X = x * length * -1 * 0.3f;
				npc.velocity.Y = y * length * -1 * 0.3f;

				npc.spriteDirection = -1;
				npc.direction = -1;
			}

			if (playerDistance <= 150 * 150f && playerDistance > 100 * 100f)
				npc.velocity = Vector2.Zero;

			if (playerDistance > 150f && player.active)
			{
				const float MoveSpeed = 0.25f;

				float x = player.Center.X - npc.Center.X;
				float y = player.Center.Y - npc.Center.Y;
				float distance = x * x + y * y;

				if (player == Main.player[npc.target])
				{
					float oldVelX = npc.velocity.X;
					float oldVelY = npc.velocity.Y;

					if (distance >= 20 * 20f)
					{
						float magnitude = 4f / distance;
						oldVelX = x * magnitude;
						oldVelY = y * magnitude;
					}

					if (npc.velocity.X < oldVelX)
					{
						npc.velocity.X += MoveSpeed;
						if (npc.velocity.X < 0 && oldVelX > 0)
							npc.velocity.X += MoveSpeed * 2f;
					}
					else if (npc.velocity.X > oldVelX)
					{
						npc.velocity.X -= MoveSpeed;
						if (npc.velocity.X > 0 && oldVelX < 0)
							npc.velocity.X -= MoveSpeed * 2f;
					}
					if (npc.velocity.Y < oldVelY)
					{
						npc.velocity.Y += MoveSpeed;
						if (npc.velocity.Y < 0 && oldVelY > 0)
							npc.velocity.Y += MoveSpeed * 2f;
					}
					else if (npc.velocity.Y > oldVelY)
					{
						npc.velocity.Y -= MoveSpeed;
						if (npc.velocity.Y > 0 && oldVelY < 0)
							npc.velocity.Y -= MoveSpeed * 2f;
					}
				}
			}
		}

		public override void HitEffect(int hitDirection, double damage)
		{
			if (npc.life <= 0)
			{
				for (int i = 1; i < 5; ++i)
					Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/CystalGore" + i), 1f);

				Player player = Main.player[npc.target];
				Filters.Scene.Deactivate("CystalTower", player.position);
				Filters.Scene.Deactivate("CystalBloodMoon", player.position);
			}

			for (int k = 0; k < 7; k++)
			{
				Dust.NewDust(npc.position, npc.width, npc.height, DustID.ScourgeOfTheCorruptor, 2.5f * hitDirection, -2.5f, 0, default, 1.2f);
				Dust.NewDust(npc.position, npc.width, npc.height, DustID.ScourgeOfTheCorruptor, 2.5f * hitDirection, -2.5f, 0, default, 0.5f);
				Dust.NewDust(npc.position, npc.width, npc.height, DustID.PurpleCrystalShard, 2.5f * hitDirection, -2.5f, 0, default, 0.7f);
			}
		}

		public override float SpawnChance(NPCSpawnInfo spawnInfo) => NPC.AnyNPCs(ModContent.NPCType<Cystal>()) && (NPC.downedBoss1 || NPC.downedBoss2 || NPC.downedBoss3 || MyWorld.downedScarabeus || MyWorld.downedReachBoss || MyWorld.downedRaider || MyWorld.downedAncientFlier) ? 0 : SpawnCondition.Corruption.Chance * 0.065f;

		public override void NPCLoot()
		{
			Filters.Scene.Deactivate("CystalTower", Main.player[npc.target].position);
			Filters.Scene.Deactivate("CystalBloodMoon", Main.player[npc.target].position);

			if (Main.rand.Next(2) == 0)
				Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, 68, Main.rand.Next(1, 3));

			if (QuestManager.GetQuest<StylistQuestCorrupt>().IsActive)
				Item.NewItem(npc.Center, ModContent.ItemType<Items.Sets.MaterialsMisc.QuestItems.CorruptDyeMaterial>());
		}

		public override bool PreDraw(SpriteBatch spriteBatch, Color drawColor)
		{
			if (colorNumber >= 510)
				check = true;
			else if (colorNumber <= 300)
				check = false;

			if (check)
				colorNumber -= 10;
			else if (!check)
				colorNumber += 10;

			var effects = npc.direction == -1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
			spriteBatch.Draw(Main.npcTexture[npc.type], npc.Center - Main.screenPosition + new Vector2(0, npc.gfxOffY), npc.frame, drawColor, npc.rotation, npc.frame.Size() / 2, npc.scale, effects, 0);
			return false;
		}

		public override void PostDraw(SpriteBatch spriteBatch, Color drawColor)
		{
			var effects = npc.direction == -1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
			spriteBatch.Draw(mod.GetTexture("NPCs/Cystal/Cystal_Glow"), npc.Center - Main.screenPosition + new Vector2(0, npc.gfxOffY), npc.frame, new Color(colorNumber / 2, colorNumber / 2, colorNumber / 2), npc.rotation, npc.frame.Size() / 2, npc.scale, effects, 0);
		}

		public override void FindFrame(int frameHeight)
		{
			if (npc.active && Main.player[npc.target].active)
			{
				npc.frameCounter++;
				if (npc.frameCounter < 6)
					npc.frame.Y = 0 * frameHeight;
				else if (npc.frameCounter < 12)
					npc.frame.Y = 1 * frameHeight;
				else if (npc.frameCounter < 18)
					npc.frame.Y = 2 * frameHeight;
				else if (npc.frameCounter < 24)
					npc.frame.Y = 3 * frameHeight;
				else if (npc.frameCounter < 30)
					npc.frame.Y = 4 * frameHeight;
				else if (npc.frameCounter < 36)
					npc.frame.Y = 5 * frameHeight;
				else if (npc.frameCounter < 42)
					npc.frame.Y = 6 * frameHeight;
				else if (npc.frameCounter < 48)
					npc.frame.Y = 7 * frameHeight;
				else
					npc.frameCounter = 0;
			}
		}
	}
}