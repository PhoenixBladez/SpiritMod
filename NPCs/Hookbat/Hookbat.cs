using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using System;
using SpiritMod.Mechanics.QuestSystem;
using SpiritMod.Mechanics.QuestSystem.Quests;
using SpiritMod.Items.Consumable.Quest;
using Terraria.GameContent.Bestiary;

namespace SpiritMod.NPCs.Hookbat
{
	public class Hookbat : ModNPC
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Hookbat");
			Main.npcFrameCount[NPC.type] = 5;
			NPCID.Sets.TrailCacheLength[NPC.type] = 2;
			NPCID.Sets.TrailingMode[NPC.type] = 0;
		}

		public override void SetDefaults()
		{
			NPC.width = 38;
			NPC.height = 38;
			NPC.damage = 10;
			NPC.rarity = 2;
			NPC.defense = 4;
			NPC.lifeMax = 42;
			NPC.knockBackResist = .53f;
			NPC.noGravity = true;
			NPC.value = 60f;
			NPC.noTileCollide = false;
			NPC.HitSound = SoundID.NPCHit1;
			NPC.DeathSound = SoundID.NPCDeath4;
		}

		public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
		{
			bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Surface,
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Times.NightTime,
				new FlavorTextBestiaryInfoElement("These simple thieving pests decided ‘divide and conquer’ was a solid strategy after stealing the adventurer’s cloth."),
			});
		}

		int frame;

		public override void AI()
		{
			NPC.spriteDirection = NPC.direction;
			Player target = Main.player[NPC.target];

			NPC.ai[0]++;
			if (!target.dead && NPC.ai[1] < 2f)
			{
				if (NPC.collideX)
				{
					NPC.velocity.X = NPC.oldVelocity.X * -0.5f;
					if (NPC.direction == -1 && NPC.velocity.X > 0f && NPC.velocity.X < 2f)
						NPC.velocity.X = 2f;
					if (NPC.direction == 1 && NPC.velocity.X < 0f && NPC.velocity.X > -2f)
						NPC.velocity.X = -2f;
				}
				if (NPC.collideY)
				{
					NPC.velocity.Y = NPC.oldVelocity.Y * -0.5f;
					if (NPC.velocity.Y > 0f && NPC.velocity.Y < 1f)
						NPC.velocity.Y = 1f;
					if (NPC.velocity.Y < 0f && NPC.velocity.Y > -1f)
						NPC.velocity.Y = -1f;
				}

				NPC.TargetClosest(true);

				if (NPC.direction == -1 && NPC.velocity.X > -5f)
				{
					NPC.velocity.X = NPC.velocity.X - 0.21f;
					if (NPC.velocity.X > 5f)
						NPC.velocity.X = NPC.velocity.X - 0.21f;
					else if (NPC.velocity.X > 0f)
						NPC.velocity.X = NPC.velocity.X - 0.05f;

					if (NPC.velocity.X < -5f)
						NPC.velocity.X = -5f;
				}
				else if (NPC.direction == 1 && NPC.velocity.X < 5f)
				{
					NPC.velocity.X = NPC.velocity.X + 0.21f;
					if (NPC.velocity.X < -5f)
						NPC.velocity.X = NPC.velocity.X + 0.21f;
					else if (NPC.velocity.X < 0f)
						NPC.velocity.X = NPC.velocity.X + 0.05f;

					if (NPC.velocity.X > 5f)
						NPC.velocity.X = 5f;
				}

				float num3225 = Math.Abs(NPC.Center.X - target.Center.X);
				float num3224 = target.position.Y - (NPC.height / 2f);

				if (num3225 > 50f)
					num3224 -= 150f;

				if (NPC.position.Y < num3224)
				{
					NPC.velocity.Y = NPC.velocity.Y + 0.05f;
					if (NPC.velocity.Y < 0f)
						NPC.velocity.Y = NPC.velocity.Y + 0.01f;
				}
				else
				{
					NPC.velocity.Y = NPC.velocity.Y - 0.05f;
					if (NPC.velocity.Y > 0f)
						NPC.velocity.Y = NPC.velocity.Y - 0.01f;
				}

				if (NPC.velocity.Y < -4f)
					NPC.velocity.Y = -4f;
				if (NPC.velocity.Y > 4f)
					NPC.velocity.Y = 3f;
			}

			Vector2 direction = Main.player[NPC.target].Center - NPC.Center;

			if (NPC.ai[0] == 190)
			{
				NPC.ai[1] = 1f;
				NPC.netUpdate = true;
			}

			if (NPC.ai[1] == 1f)
			{
				frame = 4;
				int distance = (int)Math.Sqrt((NPC.Center.X - target.Center.X) * (NPC.Center.X - target.Center.X) + (NPC.Center.Y - target.Center.Y) * (NPC.Center.Y - target.Center.Y));
				if (distance < 400)
				{
					if (NPC.ai[2] == 0)
					{
						direction.Normalize();
						SoundEngine.PlaySound(SoundID.DD2_WyvernDiveDown, NPC.Center);
						direction.X *= Main.rand.Next(14, 17);
						direction.Y *= Main.rand.Next(19, 27);
						NPC.velocity.X = direction.X;
						NPC.velocity.Y = direction.Y;
						NPC.ai[2]++;
					}
					else
						NPC.velocity.Y -= .0625f;
				}
				if (NPC.ai[0] > 235)
				{
					NPC.ai[0] = 0f;
					NPC.ai[1] = 0f;
					NPC.ai[2] = 0f;
					NPC.netUpdate = true;
				}
			}
		}

		public override float SpawnChance(NPCSpawnInfo spawnInfo)
		{
			bool valid = spawnInfo.SpawnTileY < Main.rockLayer && !Main.dayTime && spawnInfo.Player.ZoneOverworldHeight && !NPC.AnyNPCs(ModContent.NPCType<Hookbat>());
			if (!valid)
				return 0f;
			if (QuestManager.GetQuest<FirstAdventure>().IsActive)
				return 0.25f;
			return 0.01f;
		}

		public override void HitEffect(int hitDirection, double damage)
		{
			for (int k = 0; k < 10; k++)
				Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Blood, hitDirection * 2.5f, -1f, 0, default, Main.rand.NextFloat(.45f, 1.15f));

			if (NPC.life <= 0)
			{
				for (int i = 1; i < 4; ++i)
					Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, Mod.Find<ModGore>("Hookbat" + i).Type, 1f);
				Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, Mod.Find<ModGore>("Hookbat1").Type, 1f);
			}
		}

		public override void OnKill()
		{
			if (QuestManager.GetQuest<FirstAdventure>().IsActive)
				Item.NewItem(NPC.GetSource_Death(), NPC.getRect(), ModContent.ItemType<DurasilkSheaf>());
		}

		public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
		{
			if (NPC.ai[1] == 1f)
			{
				Vector2 drawOrigin = new Vector2(TextureAssets.Npc[NPC.type].Value.Width * 0.5f, (NPC.height * 0.5f));
				for (int k = 0; k < NPC.oldPos.Length; k++)
				{
					var effects = NPC.direction == -1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
					Vector2 drawPos = NPC.oldPos[k] - screenPos + drawOrigin + new Vector2(0f, NPC.gfxOffY);
					Color color = NPC.GetAlpha(drawColor) * (float)(((float)(NPC.oldPos.Length - k) / (float)NPC.oldPos.Length) / 2);
					spriteBatch.Draw(TextureAssets.Npc[NPC.type].Value, drawPos, NPC.frame, color, NPC.rotation, drawOrigin, NPC.scale, effects, 0f);
				}
			}
			return true;
		}

		public override void FindFrame(int frameHeight)
		{
			NPC.frameCounter++;

			if (NPC.frameCounter >= 6)
			{
				frame++;
				NPC.frameCounter = 0;

				if (!NPC.IsABestiaryIconDummy)
					NPC.netUpdate = true;
			}
			if (frame > 3)
				frame = 0;

			NPC.frame.Y = frameHeight * frame;
		}
	}
}