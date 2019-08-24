using System;

using Microsoft.Xna.Framework;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles.Magic
{
	class Lightning
	{

		//public override void AI()
		//{
		//	if (projectile.frameCounter >= projectile.extraUpdates * 2)
		//	{
		//		projectile.frameCounter = 0;
		//		float velocity = projectile.velocity.Length();
		//		Random random2 = new Random((int)projectile.ai[1]);
		//		Vector2 spinningpoint3 = -Vector2.UnitY;
		//		int i = 0;
		//		do
		//		{
		//			int num880 = random2.Next();
		//			projectile.ai[1] = (float)num880;
		//			num880 %= 100;
		//			float f2 = (float)num880 / 100f * 6.28318548f;
		//			Vector2 rotation = f2.ToRotationVector2();
		//			if (rotation.Y > 0f)
		//			{
		//				rotation.Y *= -1f;
		//			}
		//			if (rotation.Y > -0.02f)
		//			{
		//				continue;
		//			}
		//			if (rotation.X * (float)(projectile.extraUpdates + 1) * 2f * velocity + projectile.localAI[0] > 40f)
		//			{
		//				continue;
		//			}
		//			if (rotation.X * (float)(projectile.extraUpdates + 1) * 2f * velocity + projectile.localAI[0] < -40f)
		//			{
		//				continue;
		//			}
		//			spinningpoint3 = rotation;
		//			goto IL_24175;
		//		}
		//		while (i++ < 100);
		//		projectile.velocity = Vector2.Zero;
		//		if (projectile.localAI[1] < 1f)
		//		{
		//			projectile.localAI[1] += 2f;
		//		}
		//		IL_24175:
		//		if (projectile.velocity != Vector2.Zero)
		//		{
		//			projectile.localAI[0] += spinningpoint3.X * (float)(projectile.extraUpdates + 1) * 2f * velocity;
		//			projectile.velocity = spinningpoint3.RotatedBy((double)(projectile.ai[0] + 1.57079637f), default(Vector2)) * velocity;
		//			projectile.rotation = projectile.velocity.ToRotation() + 1.57079637f;
		//			if (Main.rand.Next(4) == 0 && Main.netMode != 1 && projectile.localAI[1] == 0f)
		//			{
		//				float num881 = (float)Main.rand.Next(-3, 4) * ((float)Math.PI / 9f);
		//				Vector2 vector90 = projectile.ai[0].ToRotationVector2().RotatedBy((double)num881) * projectile.velocity.Length();
		//				if (!Collision.CanHitLine(projectile.Center, 0, 0, projectile.Center + vector90 * 50f, 0, 0))
		//				{
		//					Projectile.NewProjectile(projectile.Center.X - vector90.X, projectile.Center.Y - vector90.Y, vector90.X, vector90.Y, projectile.type, projectile.damage, projectile.knockBack, projectile.owner, vector90.ToRotation() + 1000f, projectile.ai[1]);
		//					return;
		//				}
		//			}
		//		}
		//	}
		//}
	}
}
