using Terraria.ModLoader;
using Terraria;
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
namespace SpiritMod.Projectiles.Clubs
{
	public class NautilusBubbleProj : ModProjectile, IDrawAdditive
	{

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Bubble");
		}

		public override void SetDefaults()
		{
			projectile.aiStyle = -1;
			projectile.width = 42;
			projectile.height = 42;
			projectile.friendly = true;
			projectile.hostile = false;
            projectile.melee = true;
            projectile.penetrate = 2;
			projectile.timeLeft = 100;
			projectile.alpha = 110;
		}
        float alphaCounter;
		public override void AI()
		{
            alphaCounter += .04f;
			if (projectile.timeLeft > 55)
            {
                projectile.tileCollide = false;
            }
			else
            {
                projectile.tileCollide = true;
            }
			projectile.velocity.Y -= 0.01f;
        }
        public void DrawAdditive(SpriteBatch spriteBatch)
        {
            {
                for (int k = 0; k < projectile.oldPos.Length; k++)
                {
                    Color color = new Color(255, 255, 255) * 0.95f * ((float)(projectile.oldPos.Length - k) / (float)projectile.oldPos.Length);

                    Texture2D tex = ModContent.GetTexture("SpiritMod/Projectiles/Yoyo/MoonburstBubble_Glow");
                    
                    spriteBatch.Draw(tex, projectile.oldPos[k] + projectile.Size / 2 - Main.screenPosition, null, color, 0f, tex.Size() / 2, projectile.scale, default, default);
                }
            }
        }
        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            float sineAdd = (float)Math.Sin(alphaCounter) + 3;
            SpriteEffects spriteEffects = SpriteEffects.None;
            if (projectile.spriteDirection == 1)
                spriteEffects = SpriteEffects.FlipHorizontally;
            int xpos = (int)((projectile.Center.X + 21) - Main.screenPosition.X) - (int)(Main.projectileTexture[projectile.type].Width / 2);
            int ypos = (int)((projectile.Center.Y + 21) - Main.screenPosition.Y) - (int)(Main.projectileTexture[projectile.type].Width / 2);
            Texture2D ripple = mod.GetTexture("Effects/Masks/Extra_49");
            Main.spriteBatch.Draw(ripple, new Vector2(xpos, ypos), new Microsoft.Xna.Framework.Rectangle?(), new Color((int)(7.5f * sineAdd), (int)(16.5f * sineAdd), (int)(18f * sineAdd), 0), projectile.rotation, ripple.Size() / 2f, projectile.scale, spriteEffects, 0);
            return true;
        }
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            if (!target.boss && !target.friendly && target.knockBackResist != 0f && !target.dontTakeDamage)
            {
                target.velocity.Y -= 5.6f;
            }
        }
        public override void Kill(int timeLeft)
		{
			Main.PlaySound(2, (int)projectile.position.X, (int)projectile.position.Y, 54);
			for (int i = 0; i < 20; i++) {
				int num = Dust.NewDust(projectile.position, projectile.width, projectile.height, ModContent.DustType<Dusts.CryoDust>(), 0f, -2f, 0, default(Color), 2.2f);
				Main.dust[num].noGravity = true;
				Main.dust[num].position.X += Main.rand.Next(-50, 51) * .05f - 1.5f;
				Main.dust[num].position.Y += Main.rand.Next(-50, 51) * .05f - 1.5f;
				Main.dust[num].scale *= .2825f;
				if (Main.dust[num].position != projectile.Center)
					Main.dust[num].velocity = projectile.DirectionTo(Main.dust[num].position) * 1f;
			}
        }
        public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough)
        {
            fallThrough = false;
            return true;
        }

    }
}
