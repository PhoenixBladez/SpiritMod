using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;
using System;
using Terraria.ID;

namespace SpiritMod.Items.Sets.ReefhunterSet.Projectiles
{
	public class ReefSpearProjectile : ModProjectile
	{
		public const int MaxDistance = 40;

		public Vector2 RealDirection => (direction * MaxDistance).RotatedBy(maxRotation * (projectile.timeLeft - (maxTimeLeft / 2f)) / maxTimeLeft);

		public Vector2 direction = Vector2.Zero;
		public int maxTimeLeft = 0;
		public float maxRotation = 0;

		public override void SetStaticDefaults() => DisplayName.SetDefault("Reefe Tridente");

		public override void SetDefaults()
		{
			projectile.width = 30;
			projectile.height = 30;
			projectile.friendly = true;
			projectile.penetrate = -1;
			projectile.tileCollide = false;
			projectile.ignoreWater = true;
			projectile.melee = true;
			projectile.aiStyle = -1;
			projectile.timeLeft = 35;

			drawHeldProjInFrontOfHeldItemAndArms = false;
		}

		public override bool CanDamage() => true;

		public override void AI()
		{
			Player p = Main.player[projectile.owner];
			p.heldProj = projectile.whoAmI;
			p.itemTime = 2;
			p.itemAnimation = 2;

			GItem.ArmsTowardsMouse(p, direction == Vector2.Zero ? Main.MouseWorld : (p.Center - RealDirection + new Vector2(0, 4)));

			if (p.whoAmI != Main.myPlayer) return; //mp check (hopefully)

			if (p.channel) //Use turn functionality
				p.direction = Main.MouseWorld.X >= p.MountedCenter.X ? 1 : -1;

			if (direction == Vector2.Zero) //Initialize
			{
				direction = Vector2.Normalize(p.Center - Main.MouseWorld);
				maxTimeLeft = projectile.timeLeft;
				maxRotation = Main.rand.NextFloat(0, MathHelper.Pi * 0.33f);
			}

			float factor = (1 - (projectile.timeLeft / (float)maxTimeLeft)) * 2.5f; //Lerp factor for pushing out and coming back in
			if (projectile.timeLeft < maxTimeLeft / 2f)
				factor = projectile.timeLeft / (maxTimeLeft / 2f);

			projectile.Center = p.Center + new Vector2(0, p.gfxOffY) - Vector2.Lerp(Vector2.Zero, RealDirection, factor) + (RealDirection * 0.5f);
		}

		public override void ModifyDamageHitbox(ref Rectangle hitbox)
		{
			Vector2 pos = projectile.Center - (RealDirection * 1.5f) - new Vector2(16);

			hitbox.X = (int)pos.X;
			hitbox.Y = (int)pos.Y;
		}

		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			Texture2D t = Main.projectileTexture[projectile.type];
			spriteBatch.Draw(t, projectile.Center - Main.screenPosition, null, lightColor, RealDirection.ToRotation() - MathHelper.Pi, new Vector2(16, 14), 1f, SpriteEffects.None, 1f);
			return false;
		}
	}
}
