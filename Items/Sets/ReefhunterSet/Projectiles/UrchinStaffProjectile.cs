using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;

namespace SpiritMod.Items.Sets.ReefhunterSet.Projectiles
{
	public class UrchinStaffProjectile : ModProjectile
	{
		public override void SetStaticDefaults() => DisplayName.SetDefault("Urchin Staff");

		public override void SetDefaults()
		{
			projectile.width = 50;
			projectile.height = 54;
			projectile.friendly = true;
			projectile.penetrate = -1;
			projectile.tileCollide = false;
			projectile.ignoreWater = true;
			projectile.magic = true;
			projectile.aiStyle = -1;

			drawHeldProjInFrontOfHeldItemAndArms = false;
		}

		public override bool CanDamage() => false;

		public override void AI()
		{
			Player p = Main.player[projectile.owner];
			p.heldProj = projectile.whoAmI;

			if (p.whoAmI != Main.myPlayer) return; //mp check (hopefully)

			projectile.Center = p.Center;
			projectile.timeLeft = p.itemAnimation;

			if (p.itemAnimation < p.itemAnimationMax / 1.25f && projectile.ai[0] == 0)
			{
				projectile.ai[0] = 1f;

				Vector2 pos = p.Center + new Vector2(0, -50);
				Vector2 vel = Vector2.Normalize(Main.MouseWorld - pos) * 12;
				Projectile.NewProjectile(pos, vel, ModContent.ProjectileType<UrchinBall>(), projectile.damage, 2f, projectile.owner);
			}
		}

		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			Texture2D t = Main.projectileTexture[projectile.type];
			Player p = Main.player[projectile.owner];

			float rot = ((1 - (p.itemAnimation / (float)p.itemAnimationMax)) * MathHelper.Pi) - MathHelper.PiOver2;
			if (p.direction == -1)
				rot = (p.itemAnimation / (float)p.itemAnimationMax) * MathHelper.Pi - MathHelper.Pi;

			spriteBatch.Draw(t, projectile.Center - Main.screenPosition, new Rectangle(0, 56 * (int)projectile.ai[0], 50, 54), lightColor, rot, t.Size() * new Vector2(0, 0.5f), 1f, SpriteEffects.None, 1f);
			return false;
		}
	}
}
