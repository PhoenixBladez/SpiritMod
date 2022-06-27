using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Graphics.Shaders;
using SpiritMod.Mechanics.Trails;

namespace SpiritMod.Projectiles.Arrow
{
	public class MorningtideProjectile2 : ModProjectile, IDrawAdditive, ITrailProjectile
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Sunbeam Bolt");
			ProjectileID.Sets.TrailCacheLength[Projectile.type] = 4;
			ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
		}

		public override void SetDefaults()
		{
			Projectile.penetrate = 1;
			Projectile.friendly = true;
			Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
			Projectile.DamageType = DamageClass.Ranged;
            Projectile.timeLeft = 300;
            Projectile.width = 26;
            Projectile.height = 54;
		}

		public void DoTrailCreation(TrailManager tM)
		{
			tM.CreateTrail(Projectile, new GradientTrail(new Color(255, 225, 117), new Color(91, 21, 150)), new RoundCap(), new SleepingStarTrailPosition(), 80f, 180f, new ImageShader(Mod.GetTexture("Textures/Trails/Trail_4"), 0.01f, 1f, 1f));
			tM.CreateTrail(Projectile, new GradientTrail(new Color(255, 225, 117), new Color(91, 21, 150)), new RoundCap(), new SleepingStarTrailPosition(), 60f, 180f, new ImageShader(Mod.GetTexture("Textures/Trails/Trail_1"), 0.01f, 1f, 1f));
		}

		public override void Kill(int timLeft)
        {
            SoundEngine.PlaySound(SoundID.NPCHit, (int)Projectile.position.X, (int)Projectile.position.Y, 3);

            for (int k = 0; k < 10; k++)
            {

                Dust d = Dust.NewDustPerfect(Projectile.Center, 222, Vector2.One.RotatedByRandom(6.28f) * Main.rand.NextFloat(3), 0, default, 0.55f);
                d.noGravity = true;

                Dust d1 = Dust.NewDustPerfect(Projectile.Center, 222, Vector2.One.RotatedByRandom(6.28f) * Main.rand.NextFloat(3), 0, default, 0.55f);
                d1.shader = GameShaders.Armor.GetSecondaryShader(100, Main.LocalPlayer);
            }
        }

		public override void AI()
		{
			Lighting.AddLight((int)(Projectile.position.X / 16f), (int)(Projectile.position.Y / 16f), 0.396f, 0.170588235f, 0.564705882f);
			Projectile.rotation = Projectile.velocity.ToRotation() + 1.57f;
            Vector2 currentSpeed = new Vector2(Projectile.velocity.X, Projectile.velocity.Y);
            Projectile.velocity = currentSpeed.RotatedBy(Main.rand.Next(-1, 1) * (Math.PI / 50));
        }
		public void AdditiveCall(SpriteBatch spriteBatch)
		{
			for (int k = 0; k < Projectile.oldPos.Length; k++)
			{
				Color color = new Color(255, 255, 200) * 0.75f * ((float)(Projectile.oldPos.Length - k) / (float)Projectile.oldPos.Length);

				float scale = Projectile.scale;
				Texture2D tex = ModContent.Request<Texture2D>("SpiritMod/Projectiles/Arrow/MorningtideProjectile2");

				spriteBatch.Draw(tex, Projectile.oldPos[k] + Projectile.Size / 2 - Main.screenPosition, null, color, Projectile.rotation, tex.Size() / 2, scale, default, default);

				Color color1 = new Color(255, 186, 252) * 0.45475f * ((float)(Projectile.oldPos.Length - k) / (float)Projectile.oldPos.Length);
				for (int j = 0; j < 2; j++)
				{
					spriteBatch.Draw(tex, Projectile.oldPos[k] + Projectile.Size / 2 - Main.screenPosition + new Vector2(Main.rand.Next(-3, 3), Main.rand.Next(-3, 3)), null, color1, Projectile.rotation, tex.Size() / 2, scale, default, default);
				}
			}
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

		public override Color? GetAlpha(Color lightColor) => Color.Orange;
	}
}
