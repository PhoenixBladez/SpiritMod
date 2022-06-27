using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpiritMod.Dusts;
using SpiritMod.Effects.Stargoop;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using SpiritMod.Particles;

namespace SpiritMod.Items.Weapon.Magic.RealityQuill
{
	public class RealityQuillProjectile : ModProjectile, IMetaball
	{
		bool start;

		Vector2 previousMousePosition = Vector2.Zero;
		Vector2 currentMousePosition = Vector2.Zero;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Magic Gloop");
			ProjectileID.Sets.TrailCacheLength[Projectile.type] = 30; 
			ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
		}
		public override void SetDefaults()
		{
			Projectile.width = 32;
			Projectile.height = 32;
			Projectile.aiStyle = 0;
			Projectile.tileCollide = false;
			Projectile.hide = false;
			Projectile.friendly = true;
			Projectile.DamageType = DamageClass.Magic;
			Projectile.ignoreWater = true;
			Projectile.penetrate = -1;
			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = 40;
			Projectile.timeLeft = 200;
		}

		public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox) => Projectile.Distance(targetHitbox.Center.ToVector2()) < (16 * (Projectile.scale + 0.1f) + (targetHitbox.Width + targetHitbox.Height) / 2); //circular collision
		public override void AI()
		{
			if (!start)
			{
				SpiritMod.Metaballs.NebulaLayer.Metaballs.Add(this);

				Projectile.scale = 0.1f * Projectile.ai[0];
				Projectile.timeLeft = 150;
				previousMousePosition = currentMousePosition = Main.MouseWorld;
				start = true;
			}

			if (Projectile.timeLeft >= 140)
			{
				float normalizedX = 1 - ((Projectile.timeLeft - 140) / 8f);
				Projectile.scale = normalizedX * normalizedX * Projectile.ai[0];
			}
			if (Projectile.timeLeft < 140 && Projectile.timeLeft >= 132)
			{
				Projectile.scale = (Projectile.scale + Projectile.ai[0]) / 2;
			}
			if (Projectile.timeLeft >= 10 && Projectile.timeLeft < 24)
			{
				Projectile.scale += 0.02f * Projectile.ai[0];
			}
			if (Projectile.timeLeft < 10)
			{
				Projectile.scale *= 0.8f;
			}

			previousMousePosition = currentMousePosition;
			currentMousePosition = Main.MouseWorld;
		}

		public override void Kill(int timeLeft)
		{
			for (int i = 0; i < 2; i++)
			{
				Vector2 delta = Main.rand.NextVector2Unit() * Main.rand.NextFloat(4f, 5f);

				Dust.NewDustPerfect(Projectile.position, ModContent.DustType<NebulaGoopDust>(), delta, Scale: Main.rand.NextFloat(2f, 4f));
			}

			SpiritMod.Metaballs.NebulaLayer.Metaballs.Remove(this);
		}

		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit) {
            int cooldown = 20;
            Projectile.localNPCImmunity[target.whoAmI] = 20;
            target.immune[Projectile.owner] = cooldown;

			for (int i = 0; i < 8; i++)
			{
				Vector2 vel = Main.rand.NextFloat(6.28f).ToRotationVector2() * 4;
				vel.Normalize();
				vel = vel.RotatedBy(Main.rand.NextFloat(-0.5f, 0.5f));
				vel *= Main.rand.NextFloat(2, 5);
				ImpactLine line = new ImpactLine(target.Center - (vel * 5), vel, Color.Purple, new Vector2(0.25f, Main.rand.NextFloat(0.75f, 1.75f)), 70);
				line.TimeActive = 30;
				ParticleHandler.SpawnParticle(line);

			}
		}

		public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
		{
			float distance = (previousMousePosition - currentMousePosition).Length() * 5;
			damage = (int)(damage * MathHelper.Clamp((float)Math.Sqrt(distance), 1, 7));
		}

		public override bool PreDraw(ref Color lightColor) => false;

		public void DrawOnMetaballLayer(SpriteBatch sB)
		{
			//sB.Draw(Starjinx.Metaballs.Mask, (projectile.position - Main.screenPosition) / 2, null, Color.White, 0f, Vector2.One * 256f, projectile.scale / 16f, SpriteEffects.None, 0f);
		}
	}
}