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
using Terraria.Utilities;
using Terraria.Enums;
using SpiritMod.Particles;

namespace SpiritMod.Items.Weapon.Magic.RealityQuill
{
	public class RealityQuillProjectileTwo : ModProjectile
	{

		Vector2 previousMousePosition = Vector2.Zero;
		Vector2 currentMousePosition = Vector2.Zero;

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

				previousMousePosition = currentMousePosition = Main.MouseWorld;
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

			previousMousePosition = currentMousePosition;
			currentMousePosition = Main.MouseWorld;
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
						Vector2 startPos = point + (Vector2.Normalize(prev - point) * collisionPoint);
						for (int i = 0; i < 3; i++)
						{
							Vector2 vel = Vector2.Normalize(point - prev).RotatedBy(Main.rand.NextFloat(-0.3f,0.3f)) * 3;
							//vel = vel.RotatedBy(Main.rand.NextBool() ? -1.57f : 1.57f);
							vel.Normalize();
							vel = vel.RotatedBy(Main.rand.NextFloat(-0.5f, 0.5f));
							vel *= Main.rand.NextFloat(2, 5);
							VoidImpactLine line = new VoidImpactLine(startPos - (vel * 2), vel, Color.Purple, new Vector2(0.25f, Main.rand.NextFloat(0.75f, 1.75f)), 70);
							line.TimeActive = 30;
							ParticleHandler.SpawnParticle(line);

						}
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

		public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
		{
			float distance = (previousMousePosition - currentMousePosition).Length();
			damage = (int)(damage * MathHelper.Clamp((float)Math.Sqrt(distance), 1, 3));
		}

		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor) => false;

		public override bool? CanCutTiles() => true;

		// Plot a line from the start of the Solar Eruption to the end of it, to change the tile-cutting collision logic. (Don't change this.)
		public override void CutTiles()
		{
			DelegateMethods.tilecut_0 = TileCuttingContext.AttackProjectile;
			Vector2 prev = Vector2.Zero;
			foreach (Vector2 point in points)
			{
				if (prev != Vector2.Zero)
				{
					Utils.PlotTileLine(point, prev, 20, DelegateMethods.CutTiles);
				}
				prev = point;
			}
		}


	}
}