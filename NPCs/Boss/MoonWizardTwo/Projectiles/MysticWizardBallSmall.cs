using System;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.NPCs.Boss.MoonWizardTwo.Projectiles
{
    public class MysticWizardBallSmall : ModProjectile, IDrawAdditive
    {
		public bool Small = false;
		readonly float gravity = 0.3f;
		public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Wizard's Moonjelly Ball");
        }
        public override void SetDefaults()
        {
            projectile.width = 68;
            projectile.height = 68;
            projectile.penetrate = -1; 
			projectile.friendly = false; 
            projectile.hostile = true; 
            projectile.aiStyle = -1; 
			projectile.scale = 0.6f;
			projectile.timeLeft = 900;
        }
		public override void OnHitPlayer(Player target, int damage, bool crit) 
		{
			projectile.Kill();
		}
		public override void AI()
		{
			Lighting.AddLight(new Vector2(projectile.Center.X, projectile.Center.Y), 0.075f*2, 0.231f*2, 0.255f*2);
			projectile.velocity.Y += gravity;
			if (projectile.timeLeft < 15)
			{
				projectile.scale += .06f;
			}
			var list = Main.projectile.Where(x => x.Hitbox.Intersects(projectile.Hitbox));
			foreach (var proj in list)
				if (proj.active && proj.friendly && !proj.hostile && projectile.timeLeft > 2)
					projectile.timeLeft = 2;
			for (int i = 0; i < Main.player.Length; i++)
			{
				Player player = Main.player[i];
				if ((player.Center - projectile.Center).Length() < 300 && projectile.timeLeft > 15)
					projectile.timeLeft = 15;
			}
		}
        public void AdditiveCall(SpriteBatch spriteBatch)
        {
            {
                for (int k = 0; k < 1; k++)
                {
                    Color color = Color.White * ((float)(projectile.oldPos.Length - k) / (float)projectile.oldPos.Length);

                    float scale = projectile.scale;
                    Texture2D tex = ModContent.GetTexture("SpiritMod/NPCs/Boss/MoonWizardTwo/Projectiles/MysticWizardBall_Glow");

                    spriteBatch.Draw(tex, projectile.oldPos[k] + tex.Size() / 2 - Main.screenPosition, null, color, projectile.rotation, tex.Size() / 2, scale, default, default);
                    //spriteBatch.Draw(tex, projectile.oldPos[k] + projectile.Size / 2 - Main.screenPosition, null, color, projectile.rotation, tex.Size() / 2, scale, default, default);
                }
            }
        }
		public override void Kill(int timeLeft)
        {
			float speed = Small ? 1.25f : 3;
            for (int k = 0; k < 18; k++)
            {

                Dust d = Dust.NewDustPerfect(projectile.Center, 226, Vector2.One.RotatedByRandom(6.28f) * Main.rand.NextFloat(5), 0, default, 0.75f);
                d.noGravity = true;
            }
           for (float i = 0; i < 6.28; i+= (1.57f / 2f))
			{
				Projectile.NewProjectile(projectile.Center, i.ToRotationVector2() * 5, ModContent.ProjectileType<MysticWizardBallSmall_Projectile>(), projectile.damage, projectile.knockBack, projectile.owner);
			}

        }
		public override void PostDraw(SpriteBatch spriteBatch, Color drawColor)
		{
			Color color1 = Lighting.GetColor((int)(projectile.position.X + projectile.width * 0.5) / 16, (int)((projectile.position.Y + projectile.height * 0.5) / 16.0));
			Vector2 drawOrigin = new Vector2(Main.glowMaskTexture[239].Width * 0.5f, Main.glowMaskTexture[239].Height * 0.5f);
			int r1 = color1.R;
			Vector2 position1 = projectile.Center - Main.screenPosition;
			position1 += new Vector2(10, 12);
			Texture2D texture2D2 = Main.glowMaskTexture[239];
			float num11 = (float)(Main.GlobalTime % 1.0 / 1.0);
			float num12 = num11;
			if (num12 > 0.5)
				num12 = 1f - num11;
			if (num12 < 0.0)
				num12 = 0.0f;
			float num13 = (float)((num11 + 0.5) % 1.0);
			float num14 = num13;
			if (num14 > 0.5)
				num14 = 1f - num13;
			if (num14 < 0.0)
				num14 = 0.0f;
			Rectangle r2 = texture2D2.Frame(1, 1, 0, 0);
			Color color3 = new Color(84, 207, 255) * 1.6f;
			Main.spriteBatch.Draw(texture2D2, position1, new Microsoft.Xna.Framework.Rectangle?(r2), color3, projectile.rotation, drawOrigin, projectile.scale * .73f, SpriteEffects.None ^ SpriteEffects.FlipHorizontally, 0.0f);
			float num15 = 1f + num11 * 0.75f;
			Main.spriteBatch.Draw(texture2D2, position1, new Microsoft.Xna.Framework.Rectangle?(r2), color3 * num12, projectile.rotation, drawOrigin, projectile.scale * .73f * num15, SpriteEffects.None ^ SpriteEffects.FlipHorizontally, 0.0f);
			float num16 = 1f + num13 * 0.75f;
			Main.spriteBatch.Draw(texture2D2, position1, new Microsoft.Xna.Framework.Rectangle?(r2), color3 * num14, projectile.rotation, drawOrigin, projectile.scale * .73f * num16, SpriteEffects.None ^ SpriteEffects.FlipHorizontally, 0.0f);
		}
	}
}
