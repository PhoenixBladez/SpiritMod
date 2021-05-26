using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Weapon.Magic.StarfireLamp
{
    public class StarfireLampPurple : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Starfire");
            ProjectileID.Sets.TrailCacheLength[projectile.type] = 30;
            ProjectileID.Sets.TrailingMode[projectile.type] = 0; 
            ProjectileID.Sets.Homing[projectile.type] = true;
        }
        public override void SetDefaults()
        {
            projectile.width = 20;
            projectile.height = 32;
            projectile.aiStyle = 0;
            projectile.scale = 1f;
            projectile.tileCollide = true;
            projectile.hide = false;
            projectile.extraUpdates = 1;
            projectile.friendly = true;
            projectile.magic = true;
            Main.projFrames[projectile.type] = 4;
        }

        public override Color? GetAlpha(Color lightColor)
        {
            return Color.White;
        }

        bool primsCreated = false;
        public override void AI()
        {
            Player player = Main.player[projectile.owner];
            projectile.frameCounter++;
            if (projectile.frameCounter >= 8)
            {
                projectile.frameCounter = 0;
                projectile.frame = (projectile.frame + 1) % 4;
            }


            if (projectile.velocity.X < 0f)
                projectile.spriteDirection = -1;
            else
                projectile.spriteDirection = 1;

            if (player.HeldItem.type != mod.ItemType("StarfireLamp"))
                projectile.Kill();

            bool target = false;
            float x = 0.075f;
            float y = 0.075f;

            projectile.velocity += new Vector2((float)Math.Sign(Main.player[(int)projectile.ai[0]].Center.X - projectile.Center.X), (float)Math.Sign(Main.player[(int)projectile.ai[0]].Center.Y - 80 - projectile.Center.Y)) * new Vector2(x, y);
            if (!primsCreated)
            {
                AdjustMagnitude(ref projectile.velocity);
                primsCreated = true;
				SpiritMod.primitives.CreateTrail(new StarfirePrimTrail(projectile, Color.HotPink));
            }
            Vector2 move = Vector2.Zero;
            float distance = 800f;

            if (distance > 800f)
            {
                projectile.active = false;
            }
            else
            {
                projectile.active = true;
            }
            for (int k = 0; k < 200; k++)
            {
                if (Main.npc[k].active && !Main.npc[k].dontTakeDamage && !Main.npc[k].friendly && Main.npc[k].lifeMax > 5)
                {
                    Vector2 newMove = Main.npc[k].Center - projectile.Center;
                    float distanceTo = (float)Math.Sqrt(newMove.X * newMove.X + newMove.Y * newMove.Y);
                    if (distanceTo < distance)
                    {
                        move = newMove;
                        distance = distanceTo;
                        target = true;
                    }
                }
            }
            if (target)
            {
                projectile.tileCollide = true;
                projectile.velocity = Vector2.Lerp(projectile.velocity, Vector2.Normalize(move) * 20, 0.008f);
            }
            else if (projectile.velocity.Length() > 4 && !target)
            {
                projectile.velocity *= (2f / projectile.velocity.Length());
            }

            Lighting.AddLight(player.position, 0.5f, 0f, 0.25f);
            if (Main.player[(int)projectile.ai[0]].active && !Main.player[(int)projectile.ai[0]].dead)
                return;

            projectile.Kill();
        }
        private void AdjustMagnitude(ref Vector2 vector)
        {
            if (vector.Length() > 6f)
            {
                vector *= 6f / vector.Length();
            }
        }
        public override void Kill(int timeLeft)
        {
            //Main.PlaySound(4, (int)projectile.position.X, (int)projectile.position.Y, 3, 1f, 0f);
            Gore.NewGore(projectile.Center, projectile.velocity, mod.GetGoreSlot("Gores/StarjinxGore"), 1);
        }
    }
}