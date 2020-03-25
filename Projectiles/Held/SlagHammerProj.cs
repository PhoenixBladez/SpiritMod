using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles.Held
{
	public class SlagHammerProj : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Slag Breaker");
            ProjectileID.Sets.TrailCacheLength[projectile.type] = 9;
            ProjectileID.Sets.TrailingMode[projectile.type] = 1;

        }
        public override void SetDefaults()
        {
            projectile.width = 90;    
            projectile.height = 90;     
            projectile.friendly = true;  
            projectile.penetrate = -1;   
            projectile.tileCollide = false;
            projectile.ignoreWater = true; 
            projectile.melee = true;
			projectile.ownerHitCheck = true;
        }

       
      	public override void AI()
        {
            Player player = Main.player[projectile.owner];
            for (int k = 0; k < 2; k++)
            {
                int dust = Dust.NewDust(projectile.Center, projectile.width, projectile.height, 127);
                Main.dust[dust].velocity *= -1f;
                Main.dust[dust].noGravity = true;
                Vector2 vector2_1 = new Vector2((float)Main.rand.Next(-100, 101), (float)Main.rand.Next(-100, 101));
                vector2_1.Normalize();
                Vector2 vector2_2 = vector2_1 * ((float)Main.rand.Next(50, 100) * 0.04f);
                Main.dust[dust].velocity = vector2_2;
                vector2_2.Normalize();
                Vector2 vector2_3 = vector2_2 * 34f;
                Main.dust[dust].position = player.Center - vector2_3;
            }
            projectile.ownerHitCheck = true;
            projectile.soundDelay--;
            if (projectile.soundDelay <= 0)
            {
                Main.PlaySound(2, (int)projectile.Center.X, (int)projectile.Center.Y, 1);
                projectile.soundDelay = 60;
            }
            if (Main.myPlayer == projectile.owner)
            {
                if (!player.channel || player.noItems || player.CCed)
                {
                    projectile.Kill();
                }
            }
            projectile.Center = player.MountedCenter;
            projectile.position.X += player.width / 2 + (10 * -player.direction) * player.direction;
            projectile.spriteDirection = player.direction;
            projectile.rotation += 0.2f * player.direction;
            if (projectile.rotation > MathHelper.TwoPi)
            {
                projectile.rotation -= MathHelper.TwoPi;
            }
            else if (projectile.rotation < 0)
            {
                projectile.rotation += MathHelper.TwoPi;
            }
            player.heldProj = projectile.whoAmI;
            player.itemTime = 2;
            player.itemAnimation = 2;
            player.itemRotation = projectile.rotation;
        }
        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            Texture2D texture = Main.projectileTexture[projectile.type];
            spriteBatch.Draw(texture, projectile.Center - Main.screenPosition, null, Color.White, projectile.rotation, new Vector2(texture.Width / 2, texture.Height / 2), 1f, projectile.spriteDirection == 1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 0f);
            return false;
        }
		public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
		{
			Player player = Main.player[projectile.owner];
			if(target.Center.X > player.Center.X)
				hitDirection = 1;
			else
				hitDirection = -1;
		}
        int hits;
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            if (Main.rand.Next(6) == 2)
                target.AddBuff(BuffID.OnFire, 180);
            hits++;
            if (hits >= 4)
            {
                projectile.Kill();
            }
            {
                int n = 4;
                int deviation = Main.rand.Next(0, 300);
                for (int i = 0; i < n; i++)
                {
                    float rotation = MathHelper.ToRadians(270 / n * i + deviation);
                    Vector2 perturbedSpeed = new Vector2(projectile.velocity.X, projectile.velocity.Y).RotatedBy(rotation);
                    perturbedSpeed.Normalize();
                    perturbedSpeed.X *= 2.5f;
                    perturbedSpeed.Y *= 2.5f;
                    Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, perturbedSpeed.X, perturbedSpeed.Y, 504, projectile.damage / 2, 2, projectile.owner);
                }
            }
        }
        public override void Kill(int timeLeft)
        {
            Main.PlaySound(2, (int)projectile.position.X, (int)projectile.position.Y, 14);
            ProjectileExtras.Explode(projectile.whoAmI, 120, 120,
                delegate
                {
                    for (int i = 0; i < 40; i++)
                    {
                        int num = Dust.NewDust(projectile.position, projectile.width, projectile.height, 6, 0f, -2f, 0, default(Color), 1.2f);
                        Main.dust[num].noGravity = true;
                        Dust expr_62_cp_0 = Main.dust[num];
                        expr_62_cp_0.position.X = expr_62_cp_0.position.X + ((float)(Main.rand.Next(-50, 51) / 20) - 1.5f);
                        Dust expr_92_cp_0 = Main.dust[num];
                        expr_92_cp_0.position.Y = expr_92_cp_0.position.Y + ((float)(Main.rand.Next(-50, 51) / 20) - 1.5f);
                        if (Main.dust[num].position != projectile.Center)
                        {
                            Main.dust[num].velocity = projectile.DirectionTo(Main.dust[num].position) * 6f;
                        }
                    }
                });
            for (int num625 = 0; num625 < 2; num625++)
            {
                float scaleFactor10 = 0.33f;
                if (num625 == 1)
                    scaleFactor10 = 0.66f;

                if (num625 == 2)
                    scaleFactor10 = 1f;

                int num626 = Gore.NewGore(new Vector2(projectile.position.X + (float)(projectile.width / 2) - 24f, projectile.position.Y + (float)(projectile.height / 2) - 24f), default(Vector2), Main.rand.Next(61, 64), 1f);
                Main.gore[num626].velocity *= scaleFactor10;
                Gore expr_13AB6_cp_0 = Main.gore[num626];
                expr_13AB6_cp_0.velocity.X = expr_13AB6_cp_0.velocity.X + 1f;
                Gore expr_13AD6_cp_0 = Main.gore[num626];
                expr_13AD6_cp_0.velocity.Y = expr_13AD6_cp_0.velocity.Y + 1f;
                num626 = Gore.NewGore(new Vector2(projectile.position.X + (float)(projectile.width / 2) - 24f, projectile.position.Y + (float)(projectile.height / 2) - 24f), default(Vector2), Main.rand.Next(61, 64), 1f);
                Main.gore[num626].velocity *= scaleFactor10;
                Gore expr_13B79_cp_0 = Main.gore[num626];
                expr_13B79_cp_0.velocity.X = expr_13B79_cp_0.velocity.X - 1f;
                Gore expr_13B99_cp_0 = Main.gore[num626];
                expr_13B99_cp_0.velocity.Y = expr_13B99_cp_0.velocity.Y + 1f;
            }
        }
    }
}