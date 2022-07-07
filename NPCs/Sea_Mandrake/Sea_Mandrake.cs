using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.IO;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.DataStructures;
using Terraria.ModLoader;
using SpiritMod.Items.Consumable.Fish;
using SpiritMod.Items.Weapon.Magic.LuminanceSeacone;
using SpiritMod.Mechanics.QuestSystem;
using SpiritMod.Mechanics.QuestSystem.Quests;
using Terraria.ModLoader.Utilities;

namespace SpiritMod.NPCs.Sea_Mandrake
{
	public class Sea_Mandrake : ModNPC
	{
		private bool hasGottenColor = false;
		private int r = 0;
		private int g = 0;
		private int b = 0;
		private int sineTimer = 0;

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Sea Mandrake");
			Main.npcFrameCount[NPC.type] = 5;
			NPCID.Sets.TrailCacheLength[NPC.type] = 20;
			NPCID.Sets.TrailingMode[NPC.type] = 0;
		}

		public override void SetDefaults()
		{
			NPC.aiStyle = 16;
			NPC.lifeMax = 50;
			NPC.defense = 7;
			NPC.value = 200f;
			NPC.knockBackResist = 1.2f;
			NPC.width = 52;
			NPC.height = 84;
			NPC.aiStyle = 16;
			NPC.damage = 0;
			NPC.lavaImmune = false;
			NPC.noTileCollide = false;
			NPC.noGravity = true;
			NPC.HitSound = SoundID.NPCHit25;
			NPC.DeathSound = SoundID.NPCDeath28;

			AIType = NPCID.Goldfish;
		}

		public override void SendExtraAI(BinaryWriter writer)
		{
			writer.Write(r);
			writer.Write(g);
			writer.Write(b);
			writer.Write(hasGottenColor);
		}

		public override void ReceiveExtraAI(BinaryReader reader)
		{
			r = reader.ReadInt32();
			g = reader.ReadInt32();
			b = reader.ReadInt32();
			hasGottenColor = reader.ReadBoolean();
		}
		public override void AI()
		{
			Player player = Main.player[NPC.target];

			if (Main.rand.Next(500) == 0)
				SoundEngine.PlaySound(new SoundStyle("SpiritMod/Sounds/Mandrake_Idle"), NPC.Center);

			NPC.rotation = NPC.velocity.X * .1f;

			if (NPC.velocity.X < 0f)
				NPC.spriteDirection = -1;
			else if (NPC.velocity.X > 0f)
				NPC.spriteDirection = 1;

			if (NPC.wet)
				Movement();

			if (!NPC.wet && !player.wet)
				NPC.velocity.Y = 8f;

			if (!hasGottenColor)
			{
				hasGottenColor = true;
				r = Main.rand.Next(255);
				g = Main.rand.Next(255);
				b = Main.rand.Next(255);
			}

			sineTimer++;

			Lighting.AddLight(new Vector2(NPC.Center.X, NPC.Center.Y), r * 0.002f, g * 0.002f, b * 0.002f);

			if (NPC.wet)
				DodgeProjectiles();
		}

		private void DodgeProjectiles()
		{
			for (int i = 0; i < Main.projectile.Length; i++)
			{
				Projectile projectile = Main.projectile[i];

				if (!projectile.active)
					continue;

				float dist = Vector2.DistanceSquared(projectile.Center, NPC.Center);

				if (dist <= 100f * 100f && dist > 20f * 20f && projectile.friendly)
				{
					Vector2 vel = projectile.Center - NPC.Center + new Vector2(Main.rand.Next(-10, 10), Main.rand.Next(-10, 10));
					float speed = 8f / vel.Length();

					NPC.velocity = vel * speed * -0.83f;

					if (projectile.position.X > NPC.position.X)
						NPC.spriteDirection = NPC.direction = -1;
					else
						NPC.spriteDirection = NPC.direction = 1;
				}
			}
		}

		private void Movement()
		{
			Player player = Main.player[NPC.target];
			float distance = NPC.Distance(player.Center);
			bool validYDistance = Math.Abs(player.Center.Y - NPC.Center.Y) < NPC.height;

			if (validYDistance && distance < 130 && player.wet && NPC.wet)
			{
				Vector2 vel = NPC.DirectionFrom(player.Center) * 6.5f;

				NPC.velocity = vel;
				NPC.rotation = NPC.velocity.X * .15f;

				if (player.position.X > NPC.position.X)
				{
					NPC.spriteDirection = -1;
					NPC.direction = -1;
					NPC.netUpdate = true;
				}
				else if (player.position.X < NPC.position.X)
				{
					NPC.spriteDirection = 1;
					NPC.direction = 1;
					NPC.netUpdate = true;
				}
			}

			if (validYDistance && Vector2.Distance(player.Center, NPC.Center) <= 120 && Collision.CanHitLine(NPC.Center, 0, 0, Main.player[NPC.target].Center, 0, 0))
			{
				player.AddBuff(BuffID.Darkness, 150);
				player.Hurt(PlayerDeathReason.LegacyDefault(), (int)(NPC.damage / 1.5f), NPC.direction, false, false, false, -1);

				for (int i = 0; i < 2; i++)
				{
					int index2 = Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.AncientLight, 0.0f, 0.0f, 100, new Color(r, g, b), 3f);
					Main.dust[index2].alpha += Main.rand.Next(100);
					Main.dust[index2].noGravity = true;
					Main.dust[index2].velocity.X += i == 0 ? Main.rand.Next(-80, -40) * 0.025f * NPC.velocity.X : Main.rand.Next(-240, -180) * 0.025f * NPC.velocity.X;
					Main.dust[index2].velocity.Y -= 0.4f + Main.rand.Next(-3, 14) * 0.15f;
					Main.dust[index2].fadeIn = (float)(0.25 + Main.rand.Next(10) * 0.15f);
				}
			}
		}

		public override void OnKill()
		{
			if (QuestManager.GetQuest<StylistQuestSeafoam>().IsActive) //Quest not loot
				Item.NewItem(NPC.GetSource_Death(), NPC.Center, ModContent.ItemType<Items.Sets.MaterialsMisc.QuestItems.SeaMandrakeSac>());
		}

		public override void ModifyNPCLoot(NPCLoot npcLoot)
		{
			npcLoot.AddCommon<LuminanceSeacone>(20);
			npcLoot.AddCommon<RawFish>(2);
		}

		public override void HitEffect(int hitDirection, double damage)
		{
			if (NPC.life <= 0)
			{
				for (int index1 = 0; index1 < 26; ++index1)
				{
					int index2 = Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.AncientLight, NPC.velocity.X * 0.5f, NPC.velocity.Y * 0.5f, 90, new Color(r, g, b), 2.5f);
					Main.dust[index2].noGravity = true;
					Main.dust[index2].fadeIn = 1f;
					Main.dust[index2].velocity *= 4f;
					Main.dust[index2].noLight = true;
				}
			}

			for (int k = 0; k < 18; k++)
				Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.AncientLight, 2.5f * hitDirection, -2.5f, 0, new Color(r, g, b), Main.rand.NextFloat(0.5f, 1.2f));
		}

		public override float SpawnChance(NPCSpawnInfo spawnInfo)
		{
			if (QuestManager.GetQuest<StylistQuestSeafoam>().IsActive)
				return SpawnCondition.OceanMonster.Chance * 0.2f;
			return SpawnCondition.OceanMonster.Chance * 0.05f;
		}

		public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor) => false;

		public override void PostDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
		{
			Vector2 drawPos = NPC.Center - Main.screenPosition + new Vector2(0, NPC.gfxOffY);
			SpriteEffects effects = NPC.direction == -1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
			Texture2D glow = Mod.Assets.Request<Texture2D>("NPCs/Sea_Mandrake/Sea_Mandrake_Glow").Value;

			spriteBatch.Draw(TextureAssets.Npc[NPC.type].Value, drawPos, NPC.frame, drawColor, NPC.rotation, NPC.frame.Size() / 2, NPC.scale, effects, 0);
			spriteBatch.Draw(glow, drawPos, NPC.frame, new Color(r - NPC.alpha, g - NPC.alpha, b - NPC.alpha, byte.MaxValue - NPC.alpha), NPC.rotation, NPC.frame.Size() / 2, NPC.scale, effects, 0.0f);

			const int MaxGlows = 6;

			for (int index = 0; index < MaxGlows; ++index)
			{
				float sine = (float)Math.Sin(sineTimer * 0.06f) * 2.5f;
				Vector2 sineOffset = new Vector2(sine * sine, 0).RotatedBy(index * MathHelper.TwoPi / MaxGlows);
				Main.spriteBatch.Draw(glow, drawPos + sineOffset, NPC.frame, new Color(r, g, b, 0) * 0.2f, NPC.rotation, NPC.frame.Size() / 2, NPC.scale, effects, 0.0f);
			}
		}

		public override void FindFrame(int frameHeight)
		{
			NPC.frameCounter++;
			if (NPC.frameCounter < 6)
				NPC.frame.Y = 0;
			else if (NPC.frameCounter < 12)
				NPC.frame.Y = frameHeight;
			else if (NPC.frameCounter < 18)
				NPC.frame.Y = 2 * frameHeight;
			else if (NPC.frameCounter < 24)
				NPC.frame.Y = 3 * frameHeight;
			else if (NPC.frameCounter < 30)
				NPC.frame.Y = 4 * frameHeight;
			else
				NPC.frameCounter = 0;
		}
	}
}