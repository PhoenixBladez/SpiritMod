using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpiritMod.Dusts;
using SpiritMod.Stargoop;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Weapon.Magic.RealityQuill
{
	public class RealityQuillProjectile : ModProjectile, IMetaball
	{
		bool start;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Magic Gloop");
			ProjectileID.Sets.TrailCacheLength[projectile.type] = 30; 
			ProjectileID.Sets.TrailingMode[projectile.type] = 0;
		}
		public override void SetDefaults()
		{
			projectile.width = 32;
			projectile.height = 32;
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

		public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox) => projectile.Distance(targetHitbox.Center.ToVector2()) < (16 * (projectile.scale + 0.1f) + (targetHitbox.Width + targetHitbox.Height) / 2); //circular collision
		public override void AI()
		{
			if (!start)
			{
				SpiritMod.Metaballs.NebulaLayer.Metaballs.Add(this);

				projectile.scale = 0.1f * projectile.ai[0];
				projectile.timeLeft = 150;
				start = true;
			}

			if (projectile.timeLeft >= 140)
			{
				float normalizedX = 1 - ((projectile.timeLeft - 140) / 8f);
				projectile.scale = normalizedX * normalizedX * projectile.ai[0];
			}
			if (projectile.timeLeft < 140 && projectile.timeLeft >= 132)
			{
				projectile.scale = (projectile.scale + projectile.ai[0]) / 2;
			}
			if (projectile.timeLeft >= 10 && projectile.timeLeft < 24)
			{
				projectile.scale += 0.02f * projectile.ai[0];
			}
			if (projectile.timeLeft < 10)
			{
				projectile.scale *= 0.8f;
			}
		}

		public override void Kill(int timeLeft)
		{
			for (int i = 0; i < 2; i++)
			{
				Vector2 delta = Main.rand.NextVector2Unit() * Main.rand.NextFloat(4f, 5f);

				Dust.NewDustPerfect(projectile.position, ModContent.DustType<NebulaGoopDust>(), delta, Scale: Main.rand.NextFloat(2f, 4f));
			}

			SpiritMod.Metaballs.NebulaLayer.Metaballs.Remove(this);
		}

		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit) {
            int cooldown = 20;
            projectile.localNPCImmunity[target.whoAmI] = 20;
            target.immune[projectile.owner] = cooldown;
        }

		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor) => false;

		public void DrawOnMetaballLayer(SpriteBatch sB)
		{
			//sB.Draw(Starjinx.Metaballs.Mask, (projectile.position - Main.screenPosition) / 2, null, Color.White, 0f, Vector2.One * 256f, projectile.scale / 16f, SpriteEffects.None, 0f);
		}
	}
}