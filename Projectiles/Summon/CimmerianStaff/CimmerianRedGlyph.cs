using static Terraria.ModLoader.ModContent;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System;

namespace SpiritMod.Projectiles.Summon.CimmerianStaff
{
	public class CimmerianRedGlyph : ModProjectile, IDrawAdditive
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Gilded Bolt");
            Main.projFrames[base.projectile.type] = 4;
            ProjectileID.Sets.TrailingMode[projectile.type] = 0;
            ProjectileID.Sets.MinionSacrificable[base.projectile.type] = true;
            ProjectileID.Sets.Homing[base.projectile.type] = true;
            ProjectileID.Sets.MinionTargettingFeature[projectile.type] = true;
        }

		public override void SetDefaults()
		{
			projectile.CloneDefaults(ProjectileID.WoodenArrowFriendly);
			projectile.width = 24;
			projectile.height = 38;
			projectile.minion = true;
			projectile.friendly = true;
            Main.projPet[projectile.type] = true;
            projectile.penetrate = 2;
            projectile.timeLeft = 40;
            projectile.alpha = 100;
		}
        float alphaCounter;
        float sineAdd;
        bool chooseFrame;
		public override void AI()
		{
            projectile.rotation = 0f;
            alphaCounter += .095f;
			if (Main.rand.NextBool(15))
            {
                int glyphnum = Main.rand.Next(4);
                DustHelper.DrawDustImage(new Vector2(projectile.Center.X + Main.rand.Next(-30, 30), projectile.Center.Y + Main.rand.Next(-30, 30)), 130, 0.05f, "SpiritMod/Effects/DustImages/CimmerianGlyph" + glyphnum, 1f);
            }
            DoDustEffect(projectile.Center, 34f);
            sineAdd = (float)Math.Sin(alphaCounter) + 2;
			if (!chooseFrame)
            {
                chooseFrame = true;
                projectile.frame = Main.rand.Next(0, 4);
            }
            projectile.velocity = Vector2.Zero;
        }
        public override void Kill(int timeLeft)
        {
            Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, 0f, 0f, ModContent.ProjectileType<Fire>(), projectile.damage/3 * 2, projectile.knockBack, projectile.owner, 0f, 0f);
            Main.PlaySound(SoundID.Item, (int)projectile.position.X, (int)projectile.position.Y, 14);

            for (int k = 0; k < 40; k++)
            {
                Dust d = Dust.NewDustPerfect(projectile.Center, 130, Vector2.One.RotatedByRandom(6.28f) * Main.rand.NextFloat(5), 0, default, Main.rand.NextFloat(.4f, .8f));
                d.noGravity = true;
            }
            for (int k = 0; k < 6; k++)
            {
                int glyphnum = Main.rand.Next(4);
                DustHelper.DrawDustImage(new Vector2(projectile.Center.X + Main.rand.Next(-30, 30), projectile.Center.Y + Main.rand.Next(-30, 30)), 130, 0.05f, "SpiritMod/Effects/DustImages/CimmerianGlyph" + glyphnum, 1f);
            }
        }
        private void DoDustEffect(Vector2 position, float distance, float minSpeed = 2f, float maxSpeed = 3f, object follow = null)
        {
            float angle = Main.rand.NextFloat(-MathHelper.Pi, MathHelper.Pi);
            Vector2 vec = new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle));
            Vector2 vel = vec * Main.rand.NextFloat(minSpeed, maxSpeed);

            int dust = Dust.NewDust(position - vec * distance, 0, 0, DustID.Firework_Red);
            Main.dust[dust].noGravity = true;
            Main.dust[dust].scale *= .5f;
            Main.dust[dust].velocity = vel;
            Main.dust[dust].customData = follow;
        }
        public override Color? GetAlpha(Color lightColor)
        {
            return new Color(255, 94, 94) * sineAdd;
        }
        public void AdditiveCall(SpriteBatch spriteBatch)
		{
			for (int k = 0; k < projectile.oldPos.Length; k++) {
				Color color = new Color(255, 255, 200) * 0.75f * ((float)(projectile.oldPos.Length - k) / (float)projectile.oldPos.Length);

				float scale = projectile.scale * (float)(projectile.oldPos.Length - k) / (float)projectile.oldPos.Length * 0.2f;
				Texture2D tex = GetTexture("SpiritMod/Textures/Glow");

				spriteBatch.Draw(tex, projectile.oldPos[k] + projectile.Size / 2 - Main.screenPosition, null, color * .1f, 0, tex.Size() / 2, scale * 5, default, default);
				spriteBatch.Draw(tex, projectile.oldPos[k] + projectile.Size / 2 - Main.screenPosition, null, color * 0.3f, 0, tex.Size() / 2, scale * 4, default, default);
			}
		}
	}
}
