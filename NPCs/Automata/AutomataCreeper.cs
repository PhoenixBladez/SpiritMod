using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System;
using System.IO;
using System.Collections.Generic;
using SpiritMod.Mechanics.BoonSystem;
using SpiritMod.Buffs;
using Terraria.Audio;
using SpiritMod.Buffs.DoT;

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
			Main.npcFrameCount[npc.type] = 5;
		}

		public override void SetDefaults()
		{
			npc.width = 36;
			npc.height = 36;
			npc.damage = 70;
			npc.defense = 30;
			npc.lifeMax = 350;
			npc.HitSound = SoundID.NPCHit4;
			npc.DeathSound = SoundID.NPCDeath14;
			npc.value = 180f;
			npc.knockBackResist = 0;
			npc.buffImmune[BuffID.Poisoned] = true;
			npc.buffImmune[BuffID.Venom] = true;
			npc.buffImmune[ModContent.BuffType<FesteringWounds>()] = true;
			npc.buffImmune[ModContent.BuffType<BloodCorrupt>()] = true;
			npc.buffImmune[ModContent.BuffType<BloodInfusion>()] = true;
			npc.noGravity = true;
			initialDirection = (Main.rand.Next(2) * 2) - 1;
			moveDirection = new Vector2(initialDirection, 0);
			npc.noTileCollide = true;
			banner = npc.type;
			bannerItem = ModContent.ItemType<Items.Banners.ArachmatonBanner>();
		}

		public override void OnHitPlayer(Player target, int damage, bool crit)
		{
			if (Main.rand.Next(5) == 0)
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
			Lighting.AddLight((int)(npc.Center.X / 16f), (int)((npc.Center.Y + (npc.height / 2f)) / 16f), 0.1f * 2, 0.1f * 2, .1f * 2);

			if (aiCounter % TIMERAMOUNT == ATKAMOUNT)
			{
				attacking = true;
				npc.frameCounter = 0;
				oldVelocity = npc.velocity;
			}

			if (aiCounter % TIMERAMOUNT == 0)
			{
				attacking = false;
				npc.velocity = oldVelocity;
			}

			if (!attacking)
				Crawl();
			else
				Attack();
		}

		protected virtual void Attack()
		{
			npc.velocity = Vector2.Zero;
			if (npc.frameCounter < 1)
				shot = false;

			if (npc.frameCounter > 2 && !shot)
			{
				int glyphnum = Main.rand.Next(10);
				DustHelper.DrawDustImage(npc.Center, 6, 0.05f, "SpiritMod/Effects/Glyphs/Glyph" + glyphnum, 1.5f);
				int proj = Projectile.NewProjectile(npc.Center, npc.velocity, ModContent.ProjectileType<AutomataCreeperProj>(), Main.expertMode ? 40 : 60, 4, npc.target, npc.ai[0], npc.ai[1]);
				if (Main.projectile[proj].modProjectile is AutomataCreeperProj modproj)
					modproj.moveDirection = moveDirection;

				shot = true;

				if (Main.netMode == NetmodeID.MultiplayerClient)
				{
					NetMessage.SendData(MessageID.SyncNPC, -1, -1, null, npc.whoAmI);
					NetMessage.SendData(MessageID.SyncProjectile, -1, -1, null, proj);
				}
			}
		}

		protected Vector2 Collide() => Collision.noSlopeCollision(npc.position, npc.velocity, npc.width, npc.height, true, true);

		protected virtual void RotateCrawl()
		{
			float rotDifference = ((((npc.velocity.ToRotation() - npc.rotation) % 6.28f) + 9.42f) % 6.28f) - 3.14f;
			if (Math.Abs(rotDifference) < 0.15f)
			{
				npc.rotation = npc.velocity.ToRotation();
				return;
			}
			npc.rotation += Math.Sign(rotDifference) * 0.1f;
		}

		protected void Crawl()
		{
			newVelocity = Collide();

			if (Math.Abs(newVelocity.X) < 0.5f)
				npc.collideX = true;
			else
				npc.collideX = false;
			if (Math.Abs(newVelocity.Y) < 0.5f)
				npc.collideY = true;
			else
				npc.collideY = false;

			RotateCrawl();

			if (npc.ai[0] == 0f)
			{
				npc.TargetClosest(true);
				moveDirection.Y = 1;
				npc.ai[0] = 1f;
			}

			if (npc.ai[1] == 0f)
			{
				if (npc.collideY)
					npc.ai[0] = 2f;

				if (!npc.collideY && npc.ai[0] == 2f)
				{
					moveDirection.X = -moveDirection.X;
					npc.ai[1] = 1f;
					npc.ai[0] = 1f;
				}
				if (npc.collideX)
				{
					moveDirection.Y = -moveDirection.Y;
					npc.ai[1] = 1f;
				}
			}
			else
			{
				if (npc.collideX)
					npc.ai[0] = 2f;

				if (!npc.collideX && npc.ai[0] == 2f)
				{
					moveDirection.Y = -moveDirection.Y;
					npc.ai[1] = 0f;
					npc.ai[0] = 1f;
				}
				if (npc.collideY)
				{
					moveDirection.X = -moveDirection.X;
					npc.ai[1] = 0f;
				}
			}
			npc.velocity = SPEED * moveDirection;
			npc.velocity = Collide();
		}

		public override void HitEffect(int hitDirection, double damage)
		{
			for (int k = 0; k < 10; k++)
			{
				Dust.NewDust(npc.position, npc.width, npc.height, DustID.Sunflower, 2.5f * hitDirection, -2.5f, 0, Color.White, 0.47f);
				Dust.NewDust(npc.position, npc.width, npc.height, DustID.Wraith, 2.5f * hitDirection, -2.5f, 0, default, Main.rand.NextFloat(.45f, .55f));
			}

			if (npc.life <= 0)
			{
				Main.PlaySound(new LegacySoundStyle(4, 6).WithPitchVariance(0.2f), npc.Center);
				for (int i = 0; i < 4; ++i)
				{
					Gore.NewGore(npc.position, new Vector2(npc.velocity.X * .5f, npc.velocity.Y * .5f), 99);
				}

				for (int i = 1; i < 6; ++i)
					Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/AutomataCreeper/AutomataCreeper" + i), 1f);
			}
		}

		public override void NPCLoot()
		{
			if (Main.rand.NextBool(100))
				Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemID.ArmorPolish);
			if (Main.rand.NextBool(85))
				Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ModContent.ItemType<Items.Accessory.GoldenApple>());
		}

		public override bool PreDraw(SpriteBatch spriteBatch, Color drawColor)
		{
			spriteBatch.Draw(Main.npcTexture[npc.type], npc.Center - Main.screenPosition, npc.frame, drawColor, npc.rotation % 6.28f, npc.frame.Size() / 2, npc.scale, initialDirection != 1 ? SpriteEffects.FlipVertically : SpriteEffects.None, 0);
			return false;
		}

		public override float SpawnChance(NPCSpawnInfo spawnInfo) => (spawnInfo.spawnTileType == TileID.Marble) && spawnInfo.spawnTileY > Main.rockLayer && Main.hardMode ? 1f : 0f;

		public override void FindFrame(int frameHeight)
		{
			// MULTIPLAYER FIX, textures arent loaded on server
			// you might be able to use if (Main.dedServ) return; not sure tho
			npc.frame.Width = 112 / 2;
			//npc.frame.Width = Main.npcTexture[npc.type].Width / 2;
			npc.frameCounter %= Main.npcFrameCount[npc.type];
			int frame = (int)npc.frameCounter;
			npc.frame.Y = frame * frameHeight;
			if (attacking)
			{
				npc.frameCounter += 0.20f;
				npc.frame.X = 0;
			}
			else
			{
				npc.frameCounter += 0.20f;
				npc.frame.X = npc.frame.Width;
			}
		}
	}

	public class AutomataCreeperProj : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			ProjectileID.Sets.TrailCacheLength[projectile.type] = 2;
			ProjectileID.Sets.TrailingMode[projectile.type] = 0;
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
			projectile.penetrate = -1;
			projectile.tileCollide = false;
			projectile.hostile = true;
			projectile.friendly = false;
			projectile.width = projectile.height = 36;
			projectile.timeLeft = 150;
			projectile.ignoreWater = true;
		}

		public override void AI()
		{
			Dust.NewDustPerfect(new Vector2(projectile.position.X + Main.rand.Next(projectile.width), projectile.Bottom.Y - Main.rand.Next(7)), 6, new Vector2(-projectile.velocity.X, -projectile.velocity.Y *.5f)).noGravity = true;
			Lighting.AddLight((int)(projectile.position.X / 16f), (int)(projectile.position.Y / 16f), 0.301f, .047f, .016f);

			if (speed < 12)
				speed *= 1.03f;
			if (growCounter < 1)
				projectile.scale = growCounter += 0.1f;

			newVelocity = Collide();
			if (Math.Abs(newVelocity.X) < 0.5f)
				collideX = true;
			else
				collideX = false;
			if (Math.Abs(newVelocity.Y) < 0.5f)
				collideY = true;
			else
				collideY = false;

			if (projectile.ai[1] == 0f)
			{
				projectile.rotation += (moveDirection.X * moveDirection.Y) * 0.43f;

				if (collideY)
					projectile.ai[0] = 2f;

				if (!collideY && projectile.ai[0] == 2f)
				{
					moveDirection.X = -moveDirection.X;
					projectile.ai[1] = 1f;
					projectile.ai[0] = 1f;
				}
				if (collideX)
				{
					moveDirection.Y = -moveDirection.Y;
					projectile.ai[1] = 1f;
				}
			}
			else
			{
				projectile.rotation -= (moveDirection.X * moveDirection.Y) * 0.13f;

				if (collideX)
					projectile.ai[0] = 2f;

				if (!collideX && projectile.ai[0] == 2f)
				{
					moveDirection.Y = -moveDirection.Y;
					projectile.ai[1] = 0f;
					projectile.ai[0] = 1f;
				}
				if (collideY)
				{
					moveDirection.X = -moveDirection.X;
					projectile.ai[1] = 0f;
				}
			}
			projectile.velocity = speed * moveDirection;
			projectile.velocity = Collide();
		}

		protected virtual Vector2 Collide() => Collision.noSlopeCollision(projectile.position, projectile.velocity, projectile.width, projectile.height, true, true);

		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			Vector2 drawOrigin = new Vector2(Main.projectileTexture[projectile.type].Width * 0.5f, projectile.height * 0.5f);
			for (int k = 0; k < projectile.oldPos.Length; k++)
			{
				Vector2 drawPos = projectile.oldPos[k] - Main.screenPosition + drawOrigin + new Vector2(0f, projectile.gfxOffY);
				Color color = projectile.GetAlpha(lightColor) * ((projectile.oldPos.Length - k) / (float)projectile.oldPos.Length);
				spriteBatch.Draw(Main.projectileTexture[projectile.type], drawPos, null, color, projectile.rotation, drawOrigin, projectile.scale, SpriteEffects.None, 0f);
			}
			return false;
		}
	}
}
