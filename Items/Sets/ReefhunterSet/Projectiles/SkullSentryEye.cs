using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Sets.ReefhunterSet.Projectiles
{
	public class SkullSentryEye : ModProjectile
	{
		public const int MaxDistance = 400;

		public ref float Target => ref projectile.ai[0];
		public ref float Timer => ref projectile.ai[1];

		internal Vector2 anchor = Vector2.Zero;

		public override void SetStaticDefaults() => DisplayName.SetDefault("Maneater");

		public override void SetDefaults()
		{
			projectile.width = 32;
			projectile.height = 32;
			projectile.ranged = true;
			projectile.friendly = true;
			projectile.penetrate = -1;
			projectile.aiStyle = -1;
		}

		public override bool CanDamage() => false;

		public override void AI()
		{
			if (Target == -1 || InvalidTarget())
			{
				FindTarget();

			}
			else
			{
				NPC target = Main.npc[(int)Target];

				if (Timer++ == 30)
				{
					Vector2 vel = projectile.DirectionTo(target.Center + target.velocity) * 20;
					Projectile.NewProjectile(projectile.position, vel, ProjectileID.WoodenArrowFriendly, projectile.damage, 10.2f, projectile.owner);
					Timer = 0;
				}
			}

			projectile.velocity = (anchor - projectile.Center) * 0.02f;
		}

		private bool InvalidTarget()
		{
			NPC npc = Main.npc[(int)Target];
			return !npc.active || npc.DistanceSQ(projectile.Center) > MaxDistance * MaxDistance;
		}

		private void FindTarget()
		{
			int tempTarget = -1;

			for (int i = 0; i < Main.maxNPCs; ++i)
			{
				NPC npc = Main.npc[i];

				float dist = tempTarget == -1 ? MaxDistance * MaxDistance : Main.npc[tempTarget].DistanceSQ(projectile.Center);
				if (npc.active && npc.CanBeChasedBy() && npc.DistanceSQ(projectile.Center) < dist)
					tempTarget = npc.whoAmI;
			}

			Target = tempTarget;
		}

		public override void PostDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			Texture2D pupil = ModContent.GetTexture(Texture + "_Pupil");
			Vector2 origin = Main.projectileTexture[projectile.type].Size() / 2f;
			Vector2 pos = projectile.Center - Main.screenPosition + origin - new Vector2(2);

			if (Target != -1)
			{
				NPC npc = Main.npc[(int)Target];
				pos += projectile.DirectionTo(npc.Center) * 6;
			}

			spriteBatch.Draw(pupil, pos, null, Color.White, 0f, origin, 1f, SpriteEffects.None, 0f);
		}
	}
}
