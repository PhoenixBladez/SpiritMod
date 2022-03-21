using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;

namespace SpiritMod.Items.Sets.ReefhunterSet.Projectiles
{
	public class SkullSentryEye : ModProjectile
	{
		public const int MaxDistance = 400;

		public ref float Target => ref projectile.ai[0];

		internal Vector2 anchor = Vector2.Zero;

		public override void SetStaticDefaults() => DisplayName.SetDefault("Maneater");

		public override void SetDefaults()
		{
			projectile.width = 46;
			projectile.height = 64;
			projectile.ranged = true;
			projectile.friendly = true;
			projectile.penetrate = -1;
			projectile.aiStyle = -1;
		}

		public override void AI()
		{
			if (Target == -1 || InvalidTarget())
				FindTarget();
			else
			{

			}
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
				if (npc.active && npc.CanBeChasedBy() && npc.DistanceSQ(projectile.Center) > dist)
					tempTarget = npc.whoAmI;
			}

			Target = tempTarget;
		}

		public override void PostDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			Texture2D pupil = ModContent.GetTexture(Texture + "_Pupil");
			
			spriteBatch.Draw(pupil, projectile.Center - Main.screenPosition, null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
		}
	}
}
