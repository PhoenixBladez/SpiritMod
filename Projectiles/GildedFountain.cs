using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpiritMod.Projectiles
{
    public class GildedFountain : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Gold Cascade");
        }

        public override void SetDefaults()
        {
            projectile.friendly = true;
            projectile.melee = true;
            projectile.width = 16;
            projectile.height = 70;
            projectile.penetrate = 5;
            projectile.alpha = 255;
            projectile.timeLeft = 240;
        }
        public override bool PreAI()
        {
            float num1627 = 2f;
            float num1626 = (float)projectile.timeLeft / 60f;
            if (num1626 < 1f)
            {
                num1627 *= num1626;
            }

            for (int num1625 = 0; num1625 < 4; num1625 = 10+ 1)
            {
                Vector2 vector267 = new Vector2(0f, 0f - num1627);
                vector267 *= 0.85f + (float)Main.rand.NextDouble() * 0.2f;
                Vector2 spinningpoint18 = vector267;
                double radians17 = (Main.rand.NextDouble() - 0.5) * 1.5707963705062866;
                Vector2 vector333 = default(Vector2);
                vector267 = spinningpoint18.RotatedBy(radians17, vector333);
                Vector2 position160 = projectile.position;
                int width124 = projectile.width;
                int height124 = projectile.height;
                int num1624 = Dust.NewDust(position160, width124, height124, 222, 0f, 0f, 100, new Color(), 1f);
                Dust dust77 = Main.dust[num1624];
                dust77.scale = 1f + (float)Main.rand.NextDouble() * 0.3f;
                Dust dust81 = dust77;
                dust81.velocity *= 0.5f;
                if (dust77.velocity.Y > 0f)
                {
                    Dust expr_1D344_cp_0 = dust77;
                    expr_1D344_cp_0.velocity.Y = expr_1D344_cp_0.velocity.Y * -1f;
                }
                dust81 = dust77;
                dust81.position -= new Vector2((float)(2 + Main.rand.Next(-2, 3)), 0f);
                dust81 = dust77;
                dust81.velocity += vector267;
                dust77.scale = 0.6f;
                dust77.fadeIn = dust77.scale + 0.2f;
                Dust expr_1D3CA_cp_0 = dust77;
                expr_1D3CA_cp_0.velocity.Y = expr_1D3CA_cp_0.velocity.Y * 2f;
            }
            return true;
        }
        public override void AI()
        {
            projectile.ai[1] += 1f;
            if (projectile.ai[1] >= 7200f)
            {
                projectile.alpha += 5;
                if (projectile.alpha > 255)
                {
                    projectile.alpha = 255;
                    projectile.Kill();
                }
            }

            projectile.localAI[0] += 1f;
            if (projectile.localAI[0] >= 10f)
            {
                projectile.localAI[0] = 0f;
                int num416 = 0;
                int num417 = 0;
                float num418 = 0f;
                int num419 = projectile.type;
                for (int num420 = 0; num420 < 1000; num420++)
                {
                    if (Main.projectile[num420].active && Main.projectile[num420].owner == projectile.owner && Main.projectile[num420].type == num419 && Main.projectile[num420].ai[1] < 3600f)
                    {
                        num416++;
                        if (Main.projectile[num420].ai[1] > num418)
                        {
                            num417 = num420;
                            num418 = Main.projectile[num420].ai[1];
                        }
                    }
                }
                if (num416 > 1)
                {
                    Main.projectile[num417].netUpdate = true;
                    Main.projectile[num417].ai[1] = 36000f;
                    return;
                }
            }
        }
        public override void Kill(int timeLeft)
        {
            Dust.NewDust(projectile.position + projectile.velocity,
                projectile.width, projectile.height,
                222, projectile.oldVelocity.X * 0.2f, projectile.oldVelocity.Y * 0.2f);
        }
    }
}