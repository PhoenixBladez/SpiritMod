using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ModLoader;

namespace SpiritMod.YoYoOverload.Items
{
	public class PrimeSaw : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Prime Saw");
		}

		public override void SetDefaults()
		{
			base.projectile.damage = base.mod.GetItem("SkellyV").item.damage;
			base.projectile.friendly = true;
			base.projectile.width = 16;
			base.projectile.height = 16;
			base.projectile.penetrate = -1;
			base.projectile.ignoreWater = true;
			base.projectile.tileCollide = false;
			base.projectile.melee = true;
		}

		public override void AI()
		{
			if (!Main.projectile[(int)base.projectile.localAI[0]].active)
			{
				base.projectile.Kill();
			}
			int num = (int)base.projectile.velocity.X * 16;
			int num2 = (int)base.projectile.velocity.Y;
			base.projectile.frameCounter++;
			if (base.projectile.frameCounter > 120)
			{
				base.projectile.frameCounter = 0;
				if (base.projectile.frame == 5)
				{
					base.projectile.frame = 0;
				}
				else
				{
					base.projectile.frame++;
				}
			}
			int num3 = 80 - num;
			int num4 = 12 - num2;
			int num5 = 16;
			base.projectile.localAI[1] += 0.0104719754f * (float)num4;
			base.projectile.localAI[1] %= 6.28318548f;
			Vector2 center = Main.projectile[(int)base.projectile.localAI[0]].Center;
			center.X -= (float)num5;
			base.projectile.rotation = (float)Math.Atan2((double)center.Y, (double)center.X) - 2f;
			base.projectile.Center = center + (float)num3 * new Vector2((float)Math.Cos((double)base.projectile.localAI[1]), (float)Math.Sin((double)base.projectile.localAI[1]));
		}

		private static Vector2 GetVelocity(Projectile projectile)
		{
			float num = 400f;
			Vector2 velocity = projectile.velocity;
			Vector2 vector = new Vector2((float)Main.rand.Next(-100, 101), (float)Main.rand.Next(-100, 101));
			vector.Normalize();
			Vector2 vector2 = vector * ((float)Main.rand.Next(10, 41) * 0.1f);
			if (Main.rand.Next(3) == 0)
			{
				vector2 *= 2f;
			}
			Vector2 vector3 = velocity * 0.25f + vector2;
			for (int i = 0; i < 200; i++)
			{
				if (Main.npc[i].CanBeChasedBy(projectile, false))
				{
					float num2 = Main.npc[i].position.X + (float)(Main.npc[i].width / 2);
					float num3 = Main.npc[i].position.Y + (float)(Main.npc[i].height / 2);
					float num4 = Math.Abs(projectile.position.X + (float)(projectile.width / 2) - num2) + Math.Abs(projectile.position.Y + (float)(projectile.height / 2) - num3);
					if ((double)num4 < (double)num && Collision.CanHit(projectile.position, projectile.width, projectile.height, Main.npc[i].position, Main.npc[i].width, Main.npc[i].height))
					{
						num = num4;
						vector3.X = num2;
						vector3.Y = num3;
						Vector2 vector4 = vector3 - projectile.Center;
						vector4.Normalize();
						vector3 = vector4 * 8f;
					}
				}
			}
			return vector3 * 0.8f;
		}
	}
}
