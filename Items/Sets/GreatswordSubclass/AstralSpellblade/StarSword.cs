using Terraria;
using Microsoft.Xna.Framework;
using System;
using Terraria.ModLoader;
using System.IO;
using Microsoft.Xna.Framework.Graphics;
using SpiritMod.Particles;
using Terraria.ID;
using SpiritMod.Utilities;

namespace SpiritMod.Items.Sets.GreatswordSubclass.AstralSpellblade
{
    public class StarSword : ModProjectile
    {
		public override void SetStaticDefaults() => DisplayName.SetDefault("Astral Spellblade");

		public override void SetDefaults()
		{
			projectile.Size = new Vector2(128, 128);
			projectile.friendly = true;
			projectile.melee = true;
			projectile.ownerHitCheck = true;
			projectile.tileCollide = false;
			projectile.ignoreWater = true;
			projectile.penetrate = -1;
			projectile.aiStyle = -1;
			projectile.alpha = 255;
			projectile.scale = 1.5f;
		}

		private Projectile Parent => Main.projectile[(int)projectile.ai[0]];

		private bool Swinging => Parent.ai[0] != 0;
		public override bool CanDamage() => Swinging;
		private float Combo => Parent.ai[1];
		private float Charge => Math.Min(Parent.localAI[1], 1);
		private float AiTimer => Parent.localAI[0];

		private Player ProjOwner => Main.player[projectile.owner];
		public override void AI()
		{
			if (!ProjOwner.active || ProjOwner.dead || ProjOwner.frozen || Charge == 0 || Parent.type != ModContent.ProjectileType<AstralGreatswordHeld>() || !Parent.active)
			{
				projectile.Kill();
				return;
			}

			projectile.Center = Parent.Center + Vector2.UnitX.RotatedBy(Parent.AngleFrom(ProjOwner.MountedCenter)) * 
				120 * Charge * ((float)(Math.Sin(Main.GameUpdateCount/8f)/20) + 1f) * projectile.scale
				* ((Swinging && Combo == 2) ? MathHelper.Lerp(1, 1.5f, (float)Math.Pow(AiTimer / 20, 0.2f)) : 1);

			projectile.alpha = Math.Max(projectile.alpha - 3, 0);
			projectile.spriteDirection = projectile.direction = ProjOwner.direction;
			projectile.rotation = Parent.rotation;
		}

		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			Texture2D tex = Main.projectileTexture[projectile.type];
			SpriteEffects effects = (projectile.spriteDirection < 0) ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
			Rectangle drawRect = new Rectangle(0, (int)(tex.Height * (1 - Charge)), (int)(tex.Width * Charge), (int)(tex.Height * Charge));
			Vector2 origin = drawRect.Size() / 2;
			float Timer = (float)(Math.Sin(Main.GlobalTime * 4) / 2) + 0.5f;

			
			void DrawTex(Texture2D texture, Color color, float scale = 1f, Vector2? offset = null) => spriteBatch.Draw(texture, projectile.Center + (offset ?? Vector2.Zero) - Main.screenPosition, drawRect, color, projectile.rotation, origin, scale * projectile.scale, effects, 0);

			for (int i = 0; i < 6; i++)
				DrawTex(tex, Color.White * projectile.Opacity * 0.25f * (1 - Timer), 1, Vector2.UnitX.RotatedBy(MathHelper.TwoPi * i / 6) * 7 * Timer);
			for (int i = 0; i < 3; i++)
				DrawTex(tex, Color.White * projectile.Opacity * 0.25f * ((1 - Timer) / 2 + 0.5f) * 0.5f, 1, Vector2.UnitX.RotatedBy(MathHelper.ToRadians(Main.GlobalTime * 200) + (MathHelper.TwoPi * i / 3)) * 3);

			DrawTex(tex, Color.White * projectile.Opacity);
			return false;
		}

		public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
		{
			return Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), ProjOwner.Center,
					projectile.Center) ? true : base.Colliding(projHitbox, targetHitbox);
		}

		public override void ModifyHitNPC(NPC target, ref int damage, ref float knockBack, ref bool crit, ref int hitDirection)
        {
             if (target.boss)
                damage = (int)(damage * 1.25f);
		}

		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			if (Main.dedServ)
				return;

			Vector2 position = Main.rand.NextVector2CircularEdge(50, 50);
			Color color = Color.White;
			color.A = (byte)(color.A * 2);
			ParticleHandler.SpawnParticle(new ImpactLine(target.Center + position, -position / 4, color, Main.rand.NextFloat(0.6f, 0.8f) * new Vector2(1, 3), 10));
			for(int i = 0; i < 4; i++)
			{
				ParticleHandler.SpawnParticle(new StarParticle(
					target.Center + Main.rand.NextVector2Circular(6, 6),
					position.RotatedByRandom(MathHelper.PiOver4) * Main.rand.NextFloat(0.025f, 0.075f) * (Main.rand.NextBool() ? -1 : 1),
					color * 0.75f, SpiritMod.StarjinxColor(Main.GlobalTime) * 0.75f, Main.rand.NextFloat(0.3f, 0.6f), 20));
			}
		}
	}
}