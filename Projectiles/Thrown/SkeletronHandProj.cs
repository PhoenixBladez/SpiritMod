
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpiritMod.Items.Weapon.Thrown;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles.Thrown
{
	public class SkeletronHandProj : ModProjectile
	{
		int timer = 0;
		// USE THIS DUST: 261
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Bone Cutter");
			ProjectileID.Sets.TrailCacheLength[Projectile.type] = 4;
			ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
		}

		public override void SetDefaults()
		{
			Projectile.width = Projectile.height = 22;

			Projectile.hostile = false;
			Projectile.friendly = true;
			Projectile.DamageType = DamageClass.Ranged;
			Projectile.ignoreWater = true;
			Projectile.tileCollide = true;

			Projectile.penetrate = 4;

			Projectile.timeLeft = 160;
		}

		public override void Kill(int timeLeft)
		{
			if (Main.rand.Next(0, 4) == 0)
				Item.NewItem((int)Projectile.position.X, (int)Projectile.position.Y, Projectile.width, Projectile.height, ModContent.ItemType<SkeletronHand>(), 1, false, 0, false, false);

			for (int i = 0; i < 5; i++) {
				int d = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.Obsidian);
				Main.dust[d].scale *= .5f;
			}
			SoundEngine.PlaySound(SoundID.Dig, Projectile.Center);
		}
		public override bool PreDraw(ref Color lightColor)
		{
			Vector2 drawOrigin = new Vector2(TextureAssets.Projectile[Projectile.type].Value.Width * 0.5f, Projectile.height * 0.5f);
			for (int k = 0; k < Projectile.oldPos.Length; k++) {
				Vector2 drawPos = Projectile.oldPos[k] - Main.screenPosition + drawOrigin + new Vector2(0f, Projectile.gfxOffY);
				Color color = Projectile.GetAlpha(lightColor) * ((float)(Projectile.oldPos.Length - k) / (float)Projectile.oldPos.Length);
				spriteBatch.Draw(TextureAssets.Projectile[Projectile.type].Value, drawPos, null, color, Projectile.rotation, drawOrigin, Projectile.scale, SpriteEffects.None, 0f);
			}
			return false;
		}

		public override bool PreAI()
		{
			Projectile.rotation += .3f;
			timer++;
			if (timer == 20 || timer == 40 || timer == 80)
				Projectile.velocity *= 0.15f;
			else if (timer == 30 || timer == 90)
				Projectile.velocity *= 10;
			else if (timer >= 100)
				timer = 0;

			return false;
		}

	}
}
