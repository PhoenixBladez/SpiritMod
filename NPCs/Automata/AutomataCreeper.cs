using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using System;
using System.IO;
using System.Collections.Generic;
using SpiritMod.Mechanics.BoonSystem;
using SpiritMod.Buffs;
using Terraria.Audio;
using SpiritMod.Buffs.DoT;
using Terraria.GameContent.ItemDropRules;
using Terraria.GameContent.Bestiary;

namespace SpiritMod.NPCs.Automata
{
	public class AutomataCreeper : ModNPC, IBoonable
	{
		protected bool attacking = false;
		protected Vector2 moveDirection;
		protected Vector2 newVelocity = Vector2.Zero;
		protected int initialDirection = 0;
		protected int aiCounter = 0;
		protected Vector2 oldVelocity = Vector2.Zero;
		protected bool shot;

		protected virtual int TIMERAMOUNT => 300;
		protected virtual int ATKAMOUNT => 240;

		protected virtual int SPEED => 3;

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Arachmaton");
			Main.npcFrameCount[NPC.type] = 5;
		}

		public override void SetDefaults()
		{
			NPC.width = 36;
			NPC.height = 36;
			NPC.damage = 70;
			NPC.defense = 30;
			NPC.lifeMax = 350;
			NPC.HitSound = SoundID.NPCHit4;
			NPC.DeathSound = SoundID.NPCDeath14;
			NPC.value = 180f;
			NPC.knockBackResist = 0;
			NPC.buffImmune[BuffID.Poisoned] = true;
			NPC.buffImmune[BuffID.Venom] = true;
			NPC.buffImmune[ModContent.BuffType<FesteringWounds>()] = true;
			NPC.buffImmune[ModContent.BuffType<BloodCorrupt>()] = true;
			NPC.buffImmune[ModContent.BuffType<BloodInfusion>()] = true;
			NPC.noGravity = true;
			initialDirection = (Main.rand.Next(2) * 2) - 1;
			moveDirection = new Vector2(initialDirection, 0);
			NPC.noTileCollide = true;
			Banner = NPC.type;
			BannerItem = ModContent.ItemType<Items.Banners.ArachmatonBanner>();
		}

		public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
		{
			bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Marble,
				new FlavorTextBestiaryInfoElement("Forged from brass, a sturdy metal. Makes a lasting frame. A core from marble, containing untapped magic. This gives it mind. And at last, the spark of life. Roam free, new friend."),
			});
		}

		public override void OnHitPlayer(Player target, int damage, bool crit)
		{
			if (Main.rand.NextBool(5))
				target.AddBuff(BuffID.BrokenArmor, 1800);
		}

		public override void SendExtraAI(BinaryWriter writer)
		{
			writer.Write(attacking);
			writer.Write(initialDirection);
			writer.Write(aiCounter);
			writer.Write(shot);
		}

		public override void ReceiveExtraAI(BinaryReader reader)
		{
			attacking = reader.ReadBoolean();
			initialDirection = reader.ReadInt32();
			aiCounter = reader.ReadInt32();
			shot = reader.ReadBoolean();
		}

		public override void AI()
		{
			aiCounter++;
			Lighting.AddLight((int)(NPC.Center.X / 16f), (int)((NPC.Center.Y + (NPC.height / 2f)) / 16f), 0.1f * 2, 0.1f * 2, .1f * 2);

			if (aiCounter % TIMERAMOUNT == ATKAMOUNT)
			{
				attacking = true;
				NPC.frameCounter = 0;
				oldVelocity = NPC.velocity;
			}

			if (aiCounter % TIMERAMOUNT == 0)
			{
				attacking = false;
				NPC.velocity = oldVelocity;
			}

			if (!attacking)
				Crawl();
			else
				Attack();
		}

		protected virtual void Attack()
		{
			NPC.velocity = Vector2.Zero;
			if (NPC.frameCounter < 1)
				shot = false;

			if (NPC.frameCounter > 2 && !shot)
			{
				int glyphnum = Main.rand.Next(10);
				DustHelper.DrawDustImage(NPC.Center, 6, 0.05f, "SpiritMod/Effects/Glyphs/Glyph" + glyphnum, 1.5f);
				int proj = Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, NPC.velocity, ModContent.ProjectileType<AutomataCreeperProj>(), Main.expertMode ? 40 : 60, 4, NPC.target, NPC.ai[0], NPC.ai[1]);
				if (Main.projectile[proj].ModProjectile is AutomataCreeperProj modproj)
					modproj.moveDirection = moveDirection;

				shot = true;

				if (Main.netMode == NetmodeID.MultiplayerClient)
				{
					NetMessage.SendData(MessageID.SyncNPC, -1, -1, null, NPC.whoAmI);
					NetMessage.SendData(MessageID.SyncProjectile, -1, -1, null, proj);
				}
			}
		}

		protected Vector2 Collide() => Collision.noSlopeCollision(NPC.position, NPC.velocity, NPC.width, NPC.height, true, true);

		protected virtual void RotateCrawl()
		{
			float rotDifference = ((((NPC.velocity.ToRotation() - NPC.rotation) % 6.28f) + 9.42f) % 6.28f) - 3.14f;
			if (Math.Abs(rotDifference) < 0.15f)
			{
				NPC.rotation = NPC.velocity.ToRotation();
				return;
			}
			NPC.rotation += Math.Sign(rotDifference) * 0.1f;
		}

		protected void Crawl()
		{
			newVelocity = Collide();

			if (Math.Abs(newVelocity.X) < 0.5f)
				NPC.collideX = true;
			else
				NPC.collideX = false;
			if (Math.Abs(newVelocity.Y) < 0.5f)
				NPC.collideY = true;
			else
				NPC.collideY = false;

			RotateCrawl();

			if (NPC.ai[0] == 0f)
			{
				NPC.TargetClosest(true);
				moveDirection.Y = 1;
				NPC.ai[0] = 1f;
			}

			if (NPC.ai[1] == 0f)
			{
				if (NPC.collideY)
					NPC.ai[0] = 2f;

				if (!NPC.collideY && NPC.ai[0] == 2f)
				{
					moveDirection.X = -moveDirection.X;
					NPC.ai[1] = 1f;
					NPC.ai[0] = 1f;
				}
				if (NPC.collideX)
				{
					moveDirection.Y = -moveDirection.Y;
					NPC.ai[1] = 1f;
				}
			}
			else
			{
				if (NPC.collideX)
					NPC.ai[0] = 2f;

				if (!NPC.collideX && NPC.ai[0] == 2f)
				{
					moveDirection.Y = -moveDirection.Y;
					NPC.ai[1] = 0f;
					NPC.ai[0] = 1f;
				}
				if (NPC.collideY)
				{
					moveDirection.X = -moveDirection.X;
					NPC.ai[1] = 0f;
				}
			}
			NPC.velocity = SPEED * moveDirection;
			NPC.velocity = Collide();
		}

		public override void HitEffect(int hitDirection, double damage)
		{
			for (int k = 0; k < 10; k++)
			{
				Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Sunflower, 2.5f * hitDirection, -2.5f, 0, Color.White, 0.47f);
				Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Wraith, 2.5f * hitDirection, -2.5f, 0, default, Main.rand.NextFloat(.45f, .55f));
			}

			if (NPC.life <= 0)
			{
				SoundEngine.PlaySound(SoundID.NPCDeath6 with { PitchVariance = 0.2f }, NPC.Center);
				for (int i = 0; i < 4; ++i)
				{
					Gore.NewGore(NPC.GetSource_Death(), NPC.position, new Vector2(NPC.velocity.X * .5f, NPC.velocity.Y * .5f), 99);
				}

				for (int i = 1; i < 6; ++i)
					Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, Mod.Find<ModGore>("AutomataCreeper" + i).Type, 1f);
			}
		}

		public override void ModifyNPCLoot(NPCLoot npcLoot)
		{
			npcLoot.Add(ItemDropRule.Common(ItemID.ArmorPolish, 100));
			npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<Items.Accessory.GoldenApple>(), 85));
		}

		public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
		{
			spriteBatch.Draw(TextureAssets.Npc[NPC.type].Value, NPC.Center - screenPos, NPC.frame, drawColor, NPC.rotation % 6.28f, NPC.frame.Size() / 2, NPC.scale, initialDirection != 1 ? SpriteEffects.FlipVertically : SpriteEffects.None, 0);
			return false;
		}

		public override float SpawnChance(NPCSpawnInfo spawnInfo) => (spawnInfo.SpawnTileType == TileID.Marble) && spawnInfo.SpawnTileY > Main.rockLayer && Main.hardMode ? 1f : 0f;

		public override void FindFrame(int frameHeight)
		{
			// MULTIPLAYER FIX, textures arent loaded on server
			// you might be able to use if (Main.dedServ) return; not sure tho
			NPC.frame.Width = 112 / 2;
			//npc.frame.Width = Main.npcTexture[npc.type].Width / 2;
			NPC.frameCounter %= Main.npcFrameCount[NPC.type];
			int frame = (int)NPC.frameCounter;
			NPC.frame.Y = frame * frameHeight;
			if (attacking)
			{
				NPC.frameCounter += 0.20f;
				NPC.frame.X = 0;
			}
			else
			{
				NPC.frameCounter += 0.20f;
				NPC.frame.X = NPC.frame.Width;
			}
		}
	}

	public class AutomataCreeperProj : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			ProjectileID.Sets.TrailCacheLength[Projectile.type] = 2;
			ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
			DisplayName.SetDefault("Cog");
		}

		public Vector2 moveDirection;
		public Vector2 newVelocity = Vector2.Zero;
		public float speed = 1f;

		private float growCounter = 0;
		bool collideX = false;
		bool collideY = false;

		public override void SetDefaults()
		{
			Projectile.penetrate = -1;
			Projectile.tileCollide = false;
			Projectile.hostile = true;
			Projectile.friendly = false;
			Projectile.width = Projectile.height = 36;
			Projectile.timeLeft = 150;
			Projectile.ignoreWater = true;
		}

		public override void AI()
		{
			Dust.NewDustPerfect(new Vector2(Projectile.position.X + Main.rand.Next(Projectile.width), Projectile.Bottom.Y - Main.rand.Next(7)), 6, new Vector2(-Projectile.velocity.X, -Projectile.velocity.Y *.5f)).noGravity = true;
			Lighting.AddLight((int)(Projectile.position.X / 16f), (int)(Projectile.position.Y / 16f), 0.301f, .047f, .016f);

			if (speed < 12)
				speed *= 1.03f;
			if (growCounter < 1)
				Projectile.scale = growCounter += 0.1f;

			newVelocity = Collide();
			if (Math.Abs(newVelocity.X) < 0.5f)
				collideX = true;
			else
				collideX = false;
			if (Math.Abs(newVelocity.Y) < 0.5f)
				collideY = true;
			else
				collideY = false;

			if (Projectile.ai[1] == 0f)
			{
				Projectile.rotation += (moveDirection.X * moveDirection.Y) * 0.43f;

				if (collideY)
					Projectile.ai[0] = 2f;

				if (!collideY && Projectile.ai[0] == 2f)
				{
					moveDirection.X = -moveDirection.X;
					Projectile.ai[1] = 1f;
					Projectile.ai[0] = 1f;
				}
				if (collideX)
				{
					moveDirection.Y = -moveDirection.Y;
					Projectile.ai[1] = 1f;
				}
			}
			else
			{
				Projectile.rotation -= (moveDirection.X * moveDirection.Y) * 0.13f;

				if (collideX)
					Projectile.ai[0] = 2f;

				if (!collideX && Projectile.ai[0] == 2f)
				{
					moveDirection.Y = -moveDirection.Y;
					Projectile.ai[1] = 0f;
					Projectile.ai[0] = 1f;
				}
				if (collideY)
				{
					moveDirection.X = -moveDirection.X;
					Projectile.ai[1] = 0f;
				}
			}
			Projectile.velocity = speed * moveDirection;
			Projectile.velocity = Collide();
		}

		protected virtual Vector2 Collide() => Collision.noSlopeCollision(Projectile.position, Projectile.velocity, Projectile.width, Projectile.height, true, true);

		public override bool PreDraw(ref Color lightColor)
		{
			Vector2 drawOrigin = new Vector2(TextureAssets.Projectile[Projectile.type].Value.Width * 0.5f, Projectile.height * 0.5f);
			for (int k = 0; k < Projectile.oldPos.Length; k++)
			{
				Vector2 drawPos = Projectile.oldPos[k] - Main.screenPosition + drawOrigin + new Vector2(0f, Projectile.gfxOffY);
				Color color = Projectile.GetAlpha(lightColor) * ((Projectile.oldPos.Length - k) / (float)Projectile.oldPos.Length);
				Main.spriteBatch.Draw(TextureAssets.Projectile[Projectile.type].Value, drawPos, null, color, Projectile.rotation, drawOrigin, Projectile.scale, SpriteEffects.None, 0f);
			}
			return false;
		}
	}
}
