using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System;
using System.Collections.Generic;

namespace SpiritMod.NPCs.Automata
{
	public class AutomataCreeper : ModNPC
	{

		protected bool attacking = false;
		protected Vector2 moveDirection;
		protected Vector2 newVelocity = Vector2.Zero;
		protected int initialDirection = 0;
		protected int aiCounter = 0;
		protected const int TIMERAMOUNT = 300;

		private Vector2 oldVelocity = Vector2.Zero;
		private bool shot;

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Automata Creeper");
			Main.npcFrameCount[npc.type] = 5;
		}

		public override void SetDefaults()
		{
			npc.width = 36;
			npc.height = 36;
			npc.damage = 70;
			npc.defense = 30;
			npc.lifeMax = 600;
			npc.HitSound = SoundID.NPCHit6;
			npc.DeathSound = SoundID.NPCDeath8;
			npc.value = 10000f;
			npc.knockBackResist = 0;
			npc.noGravity = true;
			initialDirection = (Main.rand.Next(2) * 2) - 1;
			moveDirection = new Vector2(initialDirection, 0);
			npc.noTileCollide = true;
		}

		public override void AI()
		{
			aiCounter++;

			if (aiCounter % TIMERAMOUNT == 200)
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

		protected void Attack()
		{
			npc.velocity = Vector2.Zero;
			if (npc.frameCounter < 1)
				shot = false;

			if (npc.frameCounter > 2 && !shot)
			{
				Projectile proj = Projectile.NewProjectileDirect(npc.Center, npc.velocity, ModContent.ProjectileType<AutomataCreeperProj>(), Main.expertMode ? 40 : 60, 4, npc.target, npc.ai[0], npc.ai[1]);
				if (proj.modProjectile is AutomataCreeperProj modproj)
				{
					modproj.moveDirection = moveDirection;
				}
				shot = true;
			}
		}

		protected virtual Vector2 Collide()
		{
			return Collision.noSlopeCollision(npc.position, npc.velocity, npc.width, npc.height, true, true);
		}

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
			float speed = 3;
			if (npc.ai[1] == 0f)
			{
				if (npc.collideY)
				{
					npc.ai[0] = 2f;
				}
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
				{
					npc.ai[0] = 2f;
				}
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
			npc.velocity = speed * moveDirection;
			npc.velocity = Collide();
		}

		public override bool PreDraw(SpriteBatch spriteBatch, Color drawColor)
		{
			spriteBatch.Draw(Main.npcTexture[npc.type], npc.Center - Main.screenPosition, npc.frame, drawColor, npc.rotation % 6.28f, npc.frame.Size() / 2, npc.scale, initialDirection != 1 ? SpriteEffects.FlipVertically : SpriteEffects.None, 0);
			return false;
		}

		public override float SpawnChance(NPCSpawnInfo spawnInfo)
		{
			int x = spawnInfo.spawnTileX;
			int y = spawnInfo.spawnTileY;
			int tile = (int)Main.tile[x, y].type;
			return (tile == 367) && spawnInfo.spawnTileY > Main.rockLayer && Main.hardMode ? 0.5f : 0f;
		}

		public override void FindFrame(int frameHeight)
		{
			npc.frame.Width = Main.npcTexture[npc.type].Width / 2;
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
		public override void SetStaticDefaults() => DisplayName.SetDefault("Cog");

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
			projectile.width = projectile.height = 38;
			projectile.timeLeft = 150;
			projectile.ignoreWater = true;
		}
		public override void AI()
		{
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
				projectile.rotation += (float)(moveDirection.X * moveDirection.Y) * 0.13f;
				if (collideY)
				{
					projectile.ai[0] = 2f;
				}
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
				projectile.rotation -= (float)(moveDirection.X * moveDirection.Y) * 0.13f;
				if (collideX)
				{
					projectile.ai[0] = 2f;
				}
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

		protected virtual Vector2 Collide()
		{
			return Collision.noSlopeCollision(projectile.position, projectile.velocity, projectile.width, projectile.height, true, true);
		}

	}
}
