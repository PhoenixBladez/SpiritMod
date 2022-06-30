using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent;
using Terraria.ModLoader;

namespace SpiritMod.Items.Sets.ReefhunterSet.Projectiles
{
	public class ReefSpearProjectile : ModProjectile
	{
		public const int MaxDistance = 40;

		public Vector2 RealDirection => (direction * MaxDistance).RotatedBy(maxRotation * (Projectile.timeLeft - (maxTimeLeft / 2f)) / maxTimeLeft);

		public Vector2 direction = Vector2.Zero;
		public int maxTimeLeft = 0;
		public float maxRotation = 0;

		public override void SetStaticDefaults() => DisplayName.SetDefault("Reefe Tridente");

		public override void SetDefaults()
		{
			Projectile.width = 30;
			Projectile.height = 30;
			Projectile.friendly = true;
			Projectile.penetrate = -1;
			Projectile.tileCollide = false;
			Projectile.ignoreWater = true;
			Projectile.DamageType = DamageClass.Melee;
			Projectile.aiStyle = -1;
			Projectile.timeLeft = 18;

			DrawHeldProjInFrontOfHeldItemAndArms = false;
		}

		public override bool? CanDamage()/* tModPorter Suggestion: Return null instead of false */ => true;

		public override void AI()
		{
			Player p = Main.player[Projectile.owner];
			p.heldProj = Projectile.whoAmI;
			p.itemTime = 2;
			p.itemAnimation = 2;

			GItem.ArmsTowardsMouse(p, direction == Vector2.Zero ? Main.MouseWorld : (p.Center - RealDirection + new Vector2(0, 4)));

			if (p.whoAmI != Main.myPlayer) return; //mp check (hopefully)

			if (p.channel) //Use turn functionality
				p.direction = Main.MouseWorld.X >= p.MountedCenter.X ? 1 : -1;

			if (direction == Vector2.Zero) //Initialize
			{
				direction = Vector2.Normalize(p.Center - Main.MouseWorld);
				maxTimeLeft = Projectile.timeLeft;
				maxRotation = Main.rand.NextFloat(0, MathHelper.Pi * 0.33f);
			}

			float factor = (1 - (Projectile.timeLeft / (float)maxTimeLeft)) * 2f; //Lerp factor for pushing out and coming back in
			if (Projectile.timeLeft < maxTimeLeft / 2f)
				factor = Projectile.timeLeft / (maxTimeLeft / 2f);

			Projectile.Center = p.Center + new Vector2(0, p.gfxOffY) - Vector2.Lerp(Vector2.Zero, RealDirection, factor) + (RealDirection * 0.5f);
		}

		public override void ModifyDamageHitbox(ref Rectangle hitbox)
		{
			Vector2 pos = Projectile.Center - (RealDirection * 1.5f) - new Vector2(16);

			hitbox.X = (int)pos.X;
			hitbox.Y = (int)pos.Y;
		}

		public override bool PreDraw(ref Color lightColor)
		{
			Texture2D t = TextureAssets.Projectile[Projectile.type].Value;
			Main.spriteBatch.Draw(t, Projectile.Center - Main.screenPosition, null, lightColor, RealDirection.ToRotation() - MathHelper.Pi, new Vector2(16, 14), 1f, SpriteEffects.None, 1f);
			return false;
		}
	}
}
