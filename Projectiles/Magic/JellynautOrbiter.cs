using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using SpiritMod.NPCs.Boss.MoonWizard.Projectiles;
namespace SpiritMod.Projectiles.Magic
{
	public class JellynautOrbiter : ModProjectile
	{
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Arcane Jellyfish");
            Main.projFrames[Projectile.type] = 5;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 1;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
        }

		public override void SetDefaults()
		{
			Projectile.aiStyle = -1;
			Projectile.width = 8;
			Projectile.height = 8;
			Projectile.friendly = false;
			Projectile.tileCollide = false;
			Projectile.hostile = false;
			Projectile.penetrate = 2;
            Projectile.hide = false;
			Projectile.timeLeft = 9999;
		}
        float alphaCounter;
		public override void AI()
        {
            alphaCounter += .04f;
            Projectile.rotation = (float)Math.Atan2(Projectile.velocity.Y, Projectile.velocity.X) + 1.57f;
            Lighting.AddLight(new Vector2(Projectile.Center.X, Projectile.Center.Y), 0.075f * 2, 0.231f * 2, 0.255f * 2);
            Projectile.frameCounter++;
            Projectile.spriteDirection = -Projectile.direction;
            if (Projectile.frameCounter >= 10)
            {
                Projectile.frame = (Projectile.frame + 1) % Main.projFrames[Projectile.type];
                Projectile.frameCounter = 0;
            }

            float num1 = 10f;
            float num2 = 5f;
            float num3 = 40f;
            num1 = 10f;
            num2 = 7.5f;
            if (Projectile.timeLeft > 30 && Projectile.alpha > 0)
                Projectile.alpha -= 25;
            if (Projectile.timeLeft > 30 && Projectile.alpha < 128 && Collision.SolidCollision(Projectile.position, Projectile.width, Projectile.height))
                Projectile.alpha = 128;
            if (Projectile.alpha < 0)
                Projectile.alpha = 0;

            if (++Projectile.frameCounter > 4)
            {
                Projectile.frameCounter = 0;
                if (++Projectile.frame >= 4)
                    Projectile.frame = 0;
            }

            ++Projectile.ai[1];
            double num5 = (double)Projectile.ai[1] / 180.0;

            int index1 = (int)Projectile.ai[0];
            if (index1 >= 0 && Main.player[index1].active && !Main.player[index1].dead)
            {
                if (Projectile.Distance(Main.player[index1].Center) <= num3)
                    return;
                Vector2 unitY = Projectile.DirectionTo(Main.player[index1].Center);
                if (unitY.HasNaNs())
                    unitY = Vector2.UnitY;
                Projectile.velocity = (Projectile.velocity * (num1 - 1f) + unitY * num2) / num1;
            }
            else
            {
                if (Projectile.timeLeft > 30)
                    Projectile.timeLeft = 30;
                if (Projectile.ai[0] == -1f)
                    return;
                Projectile.ai[0] = -1f;
                Projectile.netUpdate = true;
            }
        }
		public override Color? GetAlpha(Color lightColor) => Color.White;
		public override bool PreDraw(ref Color lightColor)
        {
            float sineAdd = (float)Math.Sin(alphaCounter) + 3;
            SpriteEffects spriteEffects = SpriteEffects.None;
            if (Projectile.spriteDirection == 1)
                spriteEffects = SpriteEffects.FlipHorizontally;
            int xpos = (int)((Projectile.Center.X + 10) - Main.screenPosition.X) - (int)(TextureAssets.Projectile[Projectile.type].Value.Width / 2);
            int ypos = (int)((Projectile.Center.Y + 10) - Main.screenPosition.Y) - (int)(TextureAssets.Projectile[Projectile.type].Value.Width / 2);
            Texture2D ripple = Mod.Assets.Request<Texture2D>("Effects/Masks/Extra_49").Value;
            Main.spriteBatch.Draw(ripple, new Vector2(xpos, ypos), new Microsoft.Xna.Framework.Rectangle?(), new Color((int)(7.5f * sineAdd), (int)(16.5f * sineAdd), (int)(18f * sineAdd), 0), Projectile.rotation, ripple.Size() / 2f, .5f * Projectile.scale, spriteEffects, 0);
            return true;
        }
        public override void Kill(int timeLeft)
        {
            SoundEngine.PlaySound(SoundID.Item, (int)Projectile.position.X, (int)Projectile.position.Y, 69);
            Vector2 direction = Main.MouseWorld - Projectile.Center;
            direction.Normalize();
            direction *= 12f;
            {
                float A = (float)Main.rand.Next(-200, 200) * 0.05f;
                float B = (float)Main.rand.Next(-200, 200) * 0.05f;
                int p = Projectile.NewProjectile(Projectile.GetSource_Death(), Projectile.Center, direction,
                ModContent.ProjectileType<JellyfishOrbiter_Projectile>(), 16, 3, Main.myPlayer);
                Main.projectile[p].friendly = true;
                Main.projectile[p].hostile = false;
				Main.projectile[newProj].DamageType = DamageClass.Magic;
			}
        }
	}
}
