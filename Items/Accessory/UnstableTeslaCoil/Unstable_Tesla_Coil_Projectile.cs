using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace SpiritMod.Items.Accessory.UnstableTeslaCoil
{
	public class Unstable_Tesla_Coil_Projectile : ModProjectile
	{
		public float x = 0f;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Lightning Zap");
		}
		public override void SetDefaults()
		{
			projectile.width = 4;
			projectile.height = 4;
			projectile.hide = true;
			projectile.friendly = true;
			projectile.aiStyle = 0;
			projectile.MaxUpdates = 15;
			projectile.timeLeft = 66;
			projectile.tileCollide = false;
		}
		public override void AI()
		{
			projectile.localAI[0] += 1f;
			
			if (projectile.localAI[0] > -1f)
			{
				x = projectile.Center.Y + 50;
			}
			if (projectile.localAI[0] > -1f)
            {
				for (int i = 0; i < 10; i++)
				{
					float PosX = projectile.Center.X - projectile.velocity.X / 10f * (float)i;
					float PosY = projectile.Center.Y - projectile.velocity.Y / 10f * (float)i;
					
					int dustIndex = Dust.NewDust(new Vector2(PosX, PosY), 0, 0, DustID.Electric, 0f, 0f, 180, default, 0.5f);
					
					Main.dust[dustIndex].position.X = PosX;
					Main.dust[dustIndex].position.Y = PosY;
					
					Main.dust[dustIndex].velocity *= 0f;
					Main.dust[dustIndex].noGravity = true;
				}
            }
			
			Vector2 vector2_1 = new Vector2(projectile.ai[0], projectile.ai[1]);
			float speed = 16f;
			float dX = vector2_1.X - projectile.Center.X;
			float dY = vector2_1.Y - projectile.Center.Y;
			
			float dist = (float)Math.Sqrt((double)(dX * dX + dY * dY));
			
			speed /= dist;
			
			Vector2 randomSpeed = new Vector2(dX, dY).RotatedByRandom(MathHelper.ToRadians(90));
			
			if (projectile.localAI[0] > 1f)
			{
				projectile.velocity = new Vector2(randomSpeed.X * speed, randomSpeed.Y * speed);
			}
		}
		public override void Kill(int timeLeft)
		{
			int num = 22;
			for (int index1 = 0; index1 < num; ++index1)
			{
				int index2 = Dust.NewDust(projectile.Center, 0, 0, DustID.FireworkFountain_Yellow, 0.0f, 0.0f, 0, new Color(), 0.75f);
				Main.dust[index2].velocity *= 0.3f;
				--Main.dust[index2].velocity.Y;
				Main.dust[index2].position = Vector2.Lerp(Main.dust[index2].position, projectile.Center, 0.75f);
			}
			Main.PlaySound(SoundID.Trackable, (int)projectile.position.X, (int)projectile.position.Y, 141, 1f, 0f);
		}
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			  Player player = Main.player[projectile.owner];
            int num = -1;
            for (int i = 0; i < 200; i++)
            {
                if (Main.npc[i].CanBeChasedBy(player, false) && Main.npc[i] == target)
                {
                    num = i;
                }
            }
            {
                player.MinionAttackTargetNPC = num;
            }
		}
	}
}