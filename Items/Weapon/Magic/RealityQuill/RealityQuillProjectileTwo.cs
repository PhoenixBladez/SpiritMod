using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpiritMod.Dusts;
using SpiritMod.Effects.Stargoop;
using System.Collections.Generic;
using System.Linq;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Weapon.Magic.RealityQuill
{
	public class RealityQuillProjectileTwo : ModProjectile
	{
		bool primsCreated = false;
		bool released = false;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Magic Gloop");
			ProjectileID.Sets.TrailCacheLength[projectile.type] = 30; 
			ProjectileID.Sets.TrailingMode[projectile.type] = 0;
		}
		public override void SetDefaults()
		{
			projectile.width = 2;
			projectile.height = 2;
			projectile.aiStyle = 0;
			projectile.tileCollide = false;
			projectile.hide = false;
			projectile.friendly = true;
			projectile.magic = true;
			projectile.ignoreWater = true;
			projectile.penetrate = -1;
			projectile.usesLocalNPCImmunity = true;
			projectile.localNPCHitCooldown = 40;
			projectile.timeLeft = 200;
		}

		RealityQuillPrimTrail trail;
		public List<Vector2> points = new List<Vector2>();
		public override void AI()
		{
			Player player = Main.player[projectile.owner];
			foreach(Vector2 point in points)
				Lighting.AddLight(point, Color.Purple.R * 0.007f, Color.Purple.G * 0.007f, Color.Purple.B * 0.007f);
			if (!primsCreated)
			{
				trail = new RealityQuillPrimTrail(projectile);

				SpiritMod.primitives.CreateTrail(trail);
				primsCreated = true;
			}
			if (!released && player.channel)
			{
				if (player.statMana > 0) {
                    player.statMana -= 2;
					player.manaRegenDelay = 60;
				}
				if (player.statMana <= 0) {
					projectile.Kill();
				}
				player.itemTime = 5;
				player.itemAnimation = 5;
				projectile.position = Main.MouseWorld;
				points.Add(projectile.position);
				if (points.Count() > 100)
					points.RemoveAt(0);
				projectile.timeLeft = 25;
			}
			else
				released = true;
			trail._addPoints = !released;
		}
		public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox) {
			Vector2 prev = Vector2.Zero;
            foreach (Vector2 point in points)
			{
				if (prev != Vector2.Zero)
				{
					float collisionPoint = 0f;
					if (Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), point, prev, 20, ref collisionPoint)) 
					{
						return true;
					}
				}
				prev = point;
			}
			return false;
        }

		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit) {
            int cooldown = 10;
            projectile.localNPCImmunity[target.whoAmI] = 10;
            target.immune[projectile.owner] = cooldown;
        }

		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor) => false;

	}
}