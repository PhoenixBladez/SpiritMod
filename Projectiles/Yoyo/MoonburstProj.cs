using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using SpiritMod.Dusts;

namespace SpiritMod.Projectiles.Yoyo
{
	public class MoonburstProj : ModProjectile, IDrawAdditive
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Moonburst");
			ProjectileID.Sets.TrailCacheLength[Projectile.type] = 1;
			ProjectileID.Sets.TrailingMode[Projectile.type] = 0;

		}
		
		public override void SetDefaults()
		{
			Projectile.CloneDefaults(ProjectileID.Valor);
			AIType = ProjectileID.Code1;
            Projectile.width = Projectile.height = 14;

        }
        float alphaCounter;
        public override void AI()
        {
            Lighting.AddLight(new Vector2(Projectile.Center.X, Projectile.Center.Y), 0.075f * .75f, 0.231f * .75f, 0.255f * .75f);
            alphaCounter += .04f;
            if (Projectile.frameCounter >= 10)
            {
                Projectile.Kill();
            }
        }
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            Projectile.frameCounter++;
        }
        public void AdditiveCall(SpriteBatch spriteBatch, Vector2 screenPos)
        {
            if (Projectile.frameCounter >= 1)
            {
                for (int k = 0; k < Projectile.oldPos.Length; k++)
                {
                    Color color = new Color(255, 255, 255) * 0.95f * ((float)(Projectile.oldPos.Length - k) / (float)Projectile.oldPos.Length);

                    float scale = (Projectile.frameCounter * .13f) + .09f;
                    Texture2D tex = ModContent.Request<Texture2D>("SpiritMod/Projectiles/Yoyo/MoonburstBubble", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;

                    spriteBatch.Draw(tex, Projectile.oldPos[k] + Projectile.Size / 2 - screenPos, null, color, 0f, tex.Size() / 2, scale, default, default);
                    spriteBatch.Draw(tex, Projectile.oldPos[k] + Projectile.Size / 2 - screenPos, null, color * .4f, 0f, tex.Size() / 2, scale, default, default);
                }
            }
        }
        public override bool PreDraw(ref Color lightColor)
        {
            for (int k = 0; k < Projectile.oldPos.Length; k++)
            {
                float sineAdd = (float)Math.Sin(alphaCounter) + 3;
                SpriteEffects spriteEffects = SpriteEffects.None;
                if (Projectile.spriteDirection == 1)
                    spriteEffects = SpriteEffects.FlipHorizontally;
                Texture2D ripple = Mod.Assets.Request<Texture2D>("Effects/Masks/Extra_49").Value;
                Main.spriteBatch.Draw(ripple, Projectile.oldPos[k] + Projectile.Size / 2 - Main.screenPosition, new Microsoft.Xna.Framework.Rectangle?(), new Color((int)(7.5f * sineAdd), (int)(16.5f * sineAdd), (int)(18f * sineAdd), 0) * .65f, Projectile.rotation, ripple.Size() / 2f, Projectile.frameCounter * .15f, spriteEffects, 0);
            }
            return true;
        }
		public override void Kill(int timeLeft)
        {
            ProjectileExtras.Explode(Projectile.whoAmI, 150, 150, delegate
            {
                if (Projectile.frameCounter >= 8)
                {
                    SoundEngine.PlaySound(SoundID.Item54);
                    SoundEngine.PlaySound(SoundID.Item118);
                    {
                        for (int i = 0; i < 15; i++)
                        {
                            int num = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.Electric, 0f, -2f, 0, default, 2f);
                            Main.dust[num].noGravity = true;
                            Main.dust[num].position.X += Main.rand.Next(-50, 51) * .05f - 1.5f;
                            Main.dust[num].position.Y += Main.rand.Next(-50, 51) * .05f - 1.5f;
                            Main.dust[num].scale *= .25f;
                            if (Main.dust[num].position != Projectile.Center)
                                Main.dust[num].velocity = Projectile.DirectionTo(Main.dust[num].position) * 6f;
                        }
                    }
                    DustHelper.DrawDustImage(Projectile.Center, 226, 0.29f, "SpiritMod/Effects/DustImages/MoonSigil", 1f);
                }
            });
        }
    }
}