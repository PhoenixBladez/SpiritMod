using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpiritMod.Projectiles.Hostile;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.NPCs.PirateLobber
{
	public class PirateLobber : ModNPC
	{
		bool attack = false;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Pirate Lobber");
			Main.npcFrameCount[npc.type] = 8;
		}

		public override void SetDefaults()
		{
			npc.width = 34;
			npc.height = 48;
			npc.damage = 29;
			npc.defense = 16;
			npc.lifeMax = 140;
			npc.HitSound = SoundID.NPCHit2;
			npc.DeathSound = SoundID.NPCDeath2;
			npc.value = 220f;
			npc.knockBackResist = .35f;
		}

		int frame = 0;
		public override void AI()
		{
			npc.spriteDirection = npc.direction;
			Player target = Main.player[npc.target];
			int distance = (int)Vector2.Distance(npc.Center, target.Center);
			if (distance < 400)
			{
				if (!attack)
					ResetFrame();
				attack = true;
			}
			if (distance > 500)
			{
				if (attack)
					ResetFrame();
				attack = false;
			}

			if (attack) {
				npc.velocity.X = .008f * npc.direction;
				if (frame == 5 && npc.frameCounter == 0)
					Attack();
			}
			else {
				npc.aiStyle = 3;
			}
		}

		private void ResetFrame()
		{
			npc.frameCounter = 0;
			frame = 0;
		}
		private void Attack()
		{
			if (Main.netMode != NetmodeID.MultiplayerClient)
			{
				Projectile.NewProjectile(npc.Center, new Vector2(npc.direction * 7, 0), ModContent.ProjectileType<PirateLobberBarrel>(), NPCUtils.ToActualDamage(60, 1.3f), 5);
			}
		}
		public override bool PreDraw(SpriteBatch spriteBatch, Color drawColor)
		{
			var effects = npc.direction == -1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
			spriteBatch.Draw(Main.npcTexture[npc.type], npc.Center - Main.screenPosition + new Vector2(0, npc.gfxOffY), npc.frame,
							 drawColor, npc.rotation, npc.frame.Size() / 2, npc.scale, effects, 0);
			return false;
		}
		public override void FindFrame(int frameHeight)
		{
			npc.frame.Width = 60;
			npc.frame.X = attack ? npc.frame.Width : 0;
			int numFrames = attack ? 8 : 6;
			npc.frameCounter++;
			int frameDuation = attack ? 6 : 4;
			if (npc.frameCounter >= frameDuation)
			{
				npc.frameCounter = 0;
				frame++;
			}
			frame %= numFrames;
			npc.frame.Y = frameHeight * frame;
		}
	}
	public class PirateLobberBarrel : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Pirate Barrel");
			ProjectileID.Sets.TrailCacheLength[projectile.type] = 6;
			ProjectileID.Sets.TrailingMode[projectile.type] = 0;
		}

		public override void SetDefaults()
		{
			projectile.width = 30;
			projectile.height = 30;
			projectile.friendly = false;
			projectile.hostile = true;
			projectile.penetrate = 1;
			projectile.timeLeft = 300;
		}
		int direction = 0; //0 is left, 1 is right
		float rotation = 2;
		int jumpCounter = 6;
		public override void AI()
		{
			jumpCounter++;
			rotation *= 1.005F;
			projectile.velocity.Y += 0.4F;
			if (projectile.velocity.X > 0)
				direction = 1;
			if (projectile.velocity.X < 0)
				direction = 0;
			if (direction == 0)
				projectile.rotation -= rotation / 25;
			else
				projectile.rotation += rotation / 25;
			projectile.spriteDirection = projectile.direction;
		}
		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			if (oldVelocity.X != projectile.velocity.X)
			{
				if (jumpCounter > 5)
				{
					jumpCounter = 0;
					projectile.position.Y -= 20;
					projectile.velocity.X = oldVelocity.X;
				}
				else if (projectile.timeLeft > 2)
				{
					projectile.timeLeft = 2;
				}
			}
			return false;
		}
		public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough)
		{
			fallThrough = false;
			return true;
		}
	}
}
