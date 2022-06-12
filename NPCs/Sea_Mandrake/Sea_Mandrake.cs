using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.IO;
using Terraria;
using Terraria.ID;
using Terraria.DataStructures;
using Terraria.ModLoader;
using SpiritMod.Items.Consumable.Fish;
using SpiritMod.Items.Weapon.Magic.LuminanceSeacone;
using SpiritMod.Mechanics.QuestSystem;

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
			Main.npcFrameCount[npc.type] = 4;
			NPCID.Sets.TrailCacheLength[npc.type] = 20;
			NPCID.Sets.TrailingMode[npc.type] = 0;
		}

		public override void SetDefaults()
		{
			npc.aiStyle = 16;
			npc.lifeMax = 50;
			npc.defense = 7;
			npc.value = 200f;
			npc.knockBackResist = 1.2f;
			npc.width = 30;
			npc.aiStyle = 16;
			npc.height = 50;
			npc.damage = 0;
			npc.lavaImmune = false;
			npc.noTileCollide = false;
			npc.noGravity = true;
			npc.HitSound = SoundID.NPCHit25;
			npc.DeathSound = SoundID.NPCDeath28;

			aiType = NPCID.Goldfish;
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
			Player player = Main.player[npc.target];

			if (Main.rand.Next(500) == 0)
				Main.PlaySound(SoundLoader.customSoundType, (int)npc.position.X, (int)npc.position.Y, mod.GetSoundSlot(SoundType.Custom, "Sounds/Mandrake_Idle"));

			npc.rotation = npc.velocity.X * .1f;

			if (npc.velocity.X < 0f)
				npc.spriteDirection = -1;
			else if (npc.velocity.X > 0f)
				npc.spriteDirection = 1;

			if (npc.wet)
				Movement();

			if (!npc.wet && !player.wet)
				npc.velocity.Y = 8f;

			if (!hasGottenColor)
			{
				hasGottenColor = true;
				r = Main.rand.Next(255);
				g = Main.rand.Next(255);
				b = Main.rand.Next(255);
			}

			sineTimer++;

			Lighting.AddLight(new Vector2(npc.Center.X, npc.Center.Y), r * 0.002f, g * 0.002f, b * 0.002f);

			if (npc.wet)
				DodgeProjectiles();
		}

		private void DodgeProjectiles()
		{
			for (int i = 0; i < Main.projectile.Length; i++)
			{
				Projectile projectile = Main.projectile[i];

				if (!projectile.active)
					continue;

				float dist = Vector2.DistanceSquared(projectile.Center, npc.Center);

				if (dist <= 100f * 100f && dist > 20f * 20f && projectile.friendly)
				{
					Vector2 vel = projectile.Center - npc.Center + new Vector2(Main.rand.Next(-10, 10), Main.rand.Next(-10, 10));
					float speed = 8f / vel.Length();

					npc.velocity = vel * speed * -0.83f;

					if (projectile.position.X > npc.position.X)
						npc.spriteDirection = npc.direction = -1;
					else
						npc.spriteDirection = npc.direction = 1;
				}
			}
		}

		private void Movement()
		{
			Player player = Main.player[npc.target];
			float distance = npc.Distance(player.Center);
			bool validYDistance = Math.Abs(player.Center.Y - npc.Center.Y) < npc.height;

			if (validYDistance && distance < 130 && player.wet && npc.wet)
			{
				Vector2 vel = npc.DirectionFrom(player.Center) * 6.5f;

				npc.velocity = vel;
				npc.rotation = npc.velocity.X * .15f;

				if (player.position.X > npc.position.X)
				{
					npc.spriteDirection = -1;
					npc.direction = -1;
					npc.netUpdate = true;
				}
				else if (player.position.X < npc.position.X)
				{
					npc.spriteDirection = 1;
					npc.direction = 1;
					npc.netUpdate = true;
				}
			}

			if (validYDistance && Vector2.Distance(player.Center, npc.Center) <= 120 && Collision.CanHitLine(npc.Center, 0, 0, Main.player[npc.target].Center, 0, 0))
			{
				player.AddBuff(BuffID.Darkness, 150);
				player.Hurt(PlayerDeathReason.LegacyDefault(), (int)(npc.damage / 1.5f), npc.direction, false, false, false, -1);

				for (int i = 0; i < 2; i++)
				{
					int index2 = Dust.NewDust(npc.position, npc.width, npc.height, DustID.AncientLight, 0.0f, 0.0f, 100, new Color(r, g, b), 3f);
					Main.dust[index2].alpha += Main.rand.Next(100);
					Main.dust[index2].noGravity = true;
					Main.dust[index2].velocity.X += i == 0 ? Main.rand.Next(-80, -40) * 0.025f * npc.velocity.X : Main.rand.Next(-240, -180) * 0.025f * npc.velocity.X;
					Main.dust[index2].velocity.Y -= 0.4f + Main.rand.Next(-3, 14) * 0.15f;
					Main.dust[index2].fadeIn = (float)(0.25 + Main.rand.Next(10) * 0.15f);
				}
			}
		}

		public override void NPCLoot()
		{
			if (Main.rand.NextBool(20))
				Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ModContent.ItemType<LuminanceSeacone>(), 1);

			if (Main.rand.NextBool(2))
				Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ModContent.ItemType<RawFish>(), 1);

			if (QuestManager.GetQuest<Mechanics.QuestSystem.Quests.StylistQuestSeafoam>().IsActive)
				Item.NewItem(npc.Center, ModContent.ItemType<Items.Sets.MaterialsMisc.QuestItems.SeaMandrakeSac>());
		}

		public override void HitEffect(int hitDirection, double damage)
		{
			if (npc.life <= 0)
			{
				for (int index1 = 0; index1 < 26; ++index1)
				{
					int index2 = Dust.NewDust(npc.position, npc.width, npc.height, DustID.AncientLight, npc.velocity.X * 0.5f, npc.velocity.Y * 0.5f, 90, new Color(r, g, b), 2.5f);
					Main.dust[index2].noGravity = true;
					Main.dust[index2].fadeIn = 1f;
					Main.dust[index2].velocity *= 4f;
					Main.dust[index2].noLight = true;
				}
			}

			for (int k = 0; k < 18; k++)
				Dust.NewDust(npc.position, npc.width, npc.height, DustID.AncientLight, 2.5f * hitDirection, -2.5f, 0, new Color(r, g, b), Main.rand.NextFloat(0.5f, 1.2f));
		}

		public override float SpawnChance(NPCSpawnInfo spawnInfo) => SpawnCondition.OceanMonster.Chance * 0.05f;
		public override bool PreDraw(SpriteBatch spriteBatch, Color drawColor) => false;

		public override void PostDraw(SpriteBatch spriteBatch, Color drawColor)
		{
			Vector2 drawPos = npc.Center - Main.screenPosition + new Vector2(0, npc.gfxOffY);
			SpriteEffects effects = npc.direction == -1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
			Texture2D glow = mod.GetTexture("NPCs/Sea_Mandrake/Sea_Mandrake_Glow");

			spriteBatch.Draw(Main.npcTexture[npc.type], drawPos, npc.frame, drawColor, npc.rotation, npc.frame.Size() / 2, npc.scale, effects, 0);
			spriteBatch.Draw(glow, drawPos, npc.frame, new Color(r - npc.alpha, g - npc.alpha, b - npc.alpha, byte.MaxValue - npc.alpha), npc.rotation, npc.frame.Size() / 2, npc.scale, effects, 0.0f);

			const int MaxGlows = 6;

			for (int index = 0; index < MaxGlows; ++index)
			{
				float sine = (float)Math.Sin(sineTimer * 0.06f) * 2.5f;
				Vector2 sineOffset = new Vector2(sine * sine, 0).RotatedBy(index * MathHelper.TwoPi / MaxGlows);
				Main.spriteBatch.Draw(glow, drawPos + sineOffset, npc.frame, new Color(r, g, b, 0) * 0.2f, npc.rotation, npc.frame.Size() / 2, npc.scale, effects, 0.0f);
			}
		}

		public override void FindFrame(int frameHeight)
		{
			npc.frameCounter++;
			if (npc.frameCounter < 6)
				npc.frame.Y = 0;
			else if (npc.frameCounter < 12)
				npc.frame.Y = frameHeight;
			else if (npc.frameCounter < 18)
				npc.frame.Y = 2 * frameHeight;
			else if (npc.frameCounter < 24)
				npc.frame.Y = 3 * frameHeight;
			else
				npc.frameCounter = 0;
		}
	}
}