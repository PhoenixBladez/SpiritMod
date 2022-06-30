using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using SpiritMod.Mechanics.Trails;

namespace SpiritMod.Projectiles.Arrow
{
	public class ShadowmoorProjectile : ModProjectile, ITrailProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Darklight Bolt");
			ProjectileID.Sets.TrailCacheLength[Projectile.type] = 3;
			ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
		}

		public override void SetDefaults()
		{
			Projectile.friendly = true;
			Projectile.hostile = false;
			Projectile.penetrate = 1;
            Projectile.alpha = 100;
			Projectile.timeLeft = 200;
			Projectile.height = 20;
			Projectile.width = 10;
			Projectile.DamageType = DamageClass.Ranged;
			AIType = ProjectileID.DeathLaser;
		}

		public void DoTrailCreation(TrailManager tManager)
		{
			switch (Main.rand.Next(3)) {
				case 0:
					tManager.CreateTrail(Projectile, new GradientTrail(new Color(142, 8, 255), new Color(91, 21, 150)), new RoundCap(), new SleepingStarTrailPosition(), 90f, 180f, new ImageShader(Mod.Assets.Request<Texture2D>("Textures/Trails/Trail_4").Value, 0.01f, 1f, 1f));
					break;
				case 1:
					tManager.CreateTrail(Projectile, new GradientTrail(new Color(222, 84, 128), new Color(190, 72, 194)), new RoundCap(), new SleepingStarTrailPosition(), 90f, 180f, new ImageShader(Mod.Assets.Request<Texture2D>("Textures/Trails/Trail_4").Value, 0.01f, 1f, 1f));
					break;
				case 2:
					tManager.CreateTrail(Projectile, new GradientTrail(new Color(126, 55, 250), new Color(230, 117, 255)), new RoundCap(), new SleepingStarTrailPosition(), 90f, 180f, new ImageShader(Mod.Assets.Request<Texture2D>("Textures/Trails/Trail_4").Value, 0.01f, 1f, 1f));
					break;
			}
			tManager.CreateTrail(Projectile, new StandardColorTrail(Color.White * 0.3f), new RoundCap(), new SleepingStarTrailPosition(), 12f, 80f, new DefaultShader());
		}

		float num;
        bool escaped = false;
        Color colorField;
        bool checkColor = false;
        public override void AI()
		{
			if (!checkColor)
            {
                switch (Main.rand.Next(3))
                {
                    case 0:
                        colorField = new Color(191, 145, 255);
                        break;
                    case 1:
                        colorField = new Color(215, 135, 255);
                        break;
                    case 2:
                        colorField = new Color(223, 94, 255);
                        break;
                }
                checkColor = true;
            }
			if (Projectile.timeLeft >= 290) {
				Projectile.tileCollide = false;
			}
			else {
				Projectile.tileCollide = true;
			}
            Lighting.AddLight(Projectile.position, 0.205f*1.85f, 0.135f*1.85f, 0.255f*1.85f);
            num += .4f;
			Projectile.rotation = Projectile.velocity.ToRotation() + 1.57f;
            float distance = Vector2.Distance(Projectile.Center, Main.MouseWorld);
			if (distance < 20f)
            {
                DustHelper.DrawDiamond(new Vector2(Projectile.Center.X, Projectile.Center.Y), 173, 4, .8f, .75f);
                SoundEngine.PlaySound(SoundID.NPCDeath, (int)Projectile.position.X, (int)Projectile.position.Y, 6);
                if (!escaped && Main.rand.NextBool(2))
                {
                    Projectile.ai[1] = 10;
                }
                escaped = true;
            }
			if (Projectile.ai[1] == 10)
            {
                Vector2 currentSpeed = new Vector2(Projectile.velocity.X, Projectile.velocity.Y);
                Projectile.velocity = currentSpeed.RotatedBy(Main.rand.Next(-3, 3) * (Math.PI / 40));
            }
		}

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            if (Main.rand.Next(5) == 0)
                target.AddBuff(BuffID.ShadowFlame, 180);
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Vector2 drawOrigin = new Vector2(TextureAssets.Projectile[Projectile.type].Value.Width * 0.5f, Projectile.height * 0.5f);
			for (int k = 0; k < Projectile.oldPos.Length; k++) {
				Vector2 drawPos = Projectile.oldPos[k] - Main.screenPosition + drawOrigin + new Vector2(0f, Projectile.gfxOffY);
				Color color = Projectile.GetAlpha(lightColor) * ((float)(Projectile.oldPos.Length - k) / (float)Projectile.oldPos.Length);
				Main.spriteBatch.Draw(TextureAssets.Projectile[Projectile.type].Value, drawPos, null, color, Projectile.rotation, drawOrigin, Projectile.scale, SpriteEffects.None, 0f);
			}
			return true;
		}
		public override Color? GetAlpha(Color lightColor)
		{
            return colorField;
		}
		public override void Kill (int timLeft)
        {
            SoundEngine.PlaySound(SoundID.NPCHit, (int)Projectile.position.X, (int)Projectile.position.Y, 3);
        }
	}
}
