using Terraria;
using Terraria.Graphics.Effects;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace SpiritMod.NPCs.StarjinxEvent.Enemies
{
	public class PulsarShockwave : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Shockwave");
		}

		public override void SetDefaults()
		{
			Projectile.hostile = true;
			Projectile.DamageType = DamageClass.Magic;
			Projectile.width = 10;
			Projectile.height = 10;
			Projectile.friendly = false;
			Projectile.alpha = 255;
			Projectile.penetrate = -1;
			Projectile.timeLeft = 64;
			Projectile.tileCollide = false;
			Projectile.extraUpdates = 1;
		}

		//int counter = -720;
		bool boom = false;
		public override bool PreAI()
		{
			if (!boom) {
				if (Main.netMode != NetmodeID.Server && !Filters.Scene["PulsarShockwave"].IsActive()) {
					Filters.Scene.Activate("PulsarShockwave", Projectile.Center).GetShader().UseColor(Color.Orange.ToVector3()).UseTargetPosition(Projectile.Center).UseOpacity(0.9f);
					boom = true;
				}
			}
			if (Main.netMode != NetmodeID.Server && Filters.Scene["PulsarShockwave"].IsActive() && boom) {
				float progress = (float)(8 - Math.Sqrt(Projectile.timeLeft)) * 60; // Will range from -3 to 3, 0 being the point where the bomb explodes.
				Filters.Scene["PulsarShockwave"].GetShader().UseProgress(progress);
			}
			for(int i = 0; i < Main.maxPlayers; i++)
			{
				Player player = Main.player[i];
				if (player.active && boom)
				{
					PulsarPlayer modPlayer = player.GetModPlayer<PulsarPlayer>();
					float collisionPoint = 0f;
					Vector2 dir = player.Center - Projectile.Center;
					dir.Normalize();
					Vector2 dir2 = dir;
					dir *= (float)(8 - Math.Sqrt(Projectile.timeLeft)) * 60;
					if (Collision.CheckAABBvLineCollision(player.position, player.position + new Vector2(player.width, player.height), Projectile.Center, Projectile.Center + dir, (Projectile.width + Projectile.height) * 0.5f * Projectile.scale, ref collisionPoint) && modPlayer.shockwaveCooldown <= 0) {		
						player.velocity += dir2 * (float)Math.Sqrt(Projectile.timeLeft) * 2;
						modPlayer.shockwaveCooldown = 30;
					}
				}
			}
			return false;
		}
		public override void Kill(int timeLeft)
		{
			if (Main.netMode != NetmodeID.Server && Filters.Scene["PulsarShockwave"].IsActive() && boom) {
				Filters.Scene["PulsarShockwave"].Deactivate();
			}
		}
	}
	public class PulsarPlayer : ModPlayer
    {
		public int shockwaveCooldown = 0;
        public override void ResetEffects()
        {
            if (shockwaveCooldown > 0)
				shockwaveCooldown--;
        }
	}
}
