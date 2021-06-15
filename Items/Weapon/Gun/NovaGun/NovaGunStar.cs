using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using System;
using Terraria.ID;
using Terraria.Graphics.Effects;
using System.Linq;
using System.Collections.Generic;
using System.Runtime.Remoting.Messaging;
using System.IO;
using SpiritMod.Prim;

namespace SpiritMod.Items.Weapon.Gun.NovaGun
{
    public class NovaGunStar : ModProjectile, IBasicPrimDraw
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Nova Star");
            ProjectileID.Sets.TrailCacheLength[projectile.type] = 8;
            ProjectileID.Sets.TrailingMode[projectile.type] = 2; 
            ProjectileID.Sets.Homing[projectile.type] = true;
        }
        public override void SetDefaults()
        {
            projectile.width = 58;
            projectile.height = 58;
            projectile.aiStyle = 0;
            projectile.tileCollide = false;
            projectile.timeLeft = 360;
            projectile.ranged = true;
            projectile.friendly = true;
            projectile.penetrate = 1;
            projectile.ignoreWater = true;
            projectile.hide = true;
        }

        public void DrawPrimShape(BasicEffect effect)
        {
            if (projectile.velocity.Length() >= 1)
            {
                for (int i = 1; i < ProjectileID.Sets.TrailCacheLength[projectile.type]; i += 2)
                {
                    Vector2 newCenter = projectile.oldPos[i] + projectile.Size / 2;
                    float lerpamount = 0.5f + (i / (float)(ProjectileID.Sets.TrailCacheLength[projectile.type] * 2));
                    StarDraw.DrawStarBasic(effect, newCenter, projectile.oldRot[i], Scale * 100, Color.Lerp(SpiritMod.StarjinxColor(Main.GlobalTime * 4 + i), Color.Transparent, lerpamount));
                }
            }

            StarDraw.DrawStarBasic(effect, projectile.Center, projectile.rotation, Scale * 100, Color.Lerp(SpiritMod.StarjinxColor(Main.GlobalTime * 4), Color.Black, 0.1f));
            StarDraw.DrawStarBasic(effect, projectile.Center, projectile.rotation, Scale * 70, Color.White);
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color drawColor)
        {
            spriteBatch.Draw(mod.GetTexture("Extras/StardustPillarStar"), 
                projectile.Center - Main.screenPosition, 
                null,
                Color.Lerp(SpiritMod.StarjinxColor(Main.GlobalTime * 4), Color.Transparent, 0.5f), 0, new Vector2(36, 36),
                Scale * 2.5f * ((float)Math.Sin(projectile.timeLeft/7)/8 + 1), 
                SpriteEffects.None, 
                0);

            return false;
        }

        IEnumerable<NPC> Validtargets(float maxdist) => Main.npc.Where(x => x.CanBeChasedBy(this) && x.Distance(projectile.Center) < maxdist);

        ref float Charge => ref projectile.ai[1];

        ref float Timer => ref projectile.ai[0];

        float Scale => 0.66f * ((Charge * 0.33f) + 0.5f); //regular scale isnt being adjusted properly, keeps being set back to what it was on the first tick after being changed, no clue whats causing it, im going to go insane

        public override void AI()
        {
            Timer++;
            float rotationamount = MathHelper.Lerp(0.1f, 0.13f, (projectile.ai[0] - 30f) / 60f);
            projectile.rotation += rotationamount;

            if (Main.rand.Next(7) == 0)
                Gore.NewGore(projectile.Center + Main.rand.NextVector2Circular(12, 12), Main.rand.NextVector2Circular(1, 1) + projectile.velocity/2, mod.GetGoreSlot("Gores/StarjinxGore"));

            if (Timer >= 30 && Timer < 90)
            {
                projectile.velocity *= 0.94f;
                float maxdist = 2000;
                if (!Validtargets(maxdist).Any())
                    Timer = 30;
                else if (Charge >= 1.5f) //skip to launching itself if it is fully charged
                    Timer = 90;

            }
            if(Timer >= 90 && Timer < 110)
            {
                projectile.velocity = Vector2.Lerp(projectile.velocity, Vector2.Normalize(projectile.velocity) * 10 * (Charge / 2 + 1), 0.2f);
                float maxdist = 2000;
                NPC target = null;
                for (int i = 0; i < Main.npc.Length; i++)
                {
                    if (Validtargets(maxdist).Contains(Main.npc[i]))
                    {
                        maxdist = projectile.Distance(Main.npc[i].Center);
                        target = Main.npc[i];
                    }
                }
                if (target != null)
                {
                    projectile.velocity = projectile.velocity.RotatedBy(projectile.direction * projectile.AngleTo(target.Center) / 40);
                }
            }
            if(Timer >= 110)
            {
                float maxdist = 2000;
                NPC target = null;
                for(int i = 0; i < Main.npc.Length; i++)
                {
                    if(Validtargets(maxdist).Contains(Main.npc[i]))
                    {
                        maxdist = projectile.Distance(Main.npc[i].Center);
                        target = Main.npc[i];
                    }
                }
                if(target != null)
                {
                    projectile.velocity = Vector2.Lerp(projectile.velocity, projectile.DirectionTo(target.Center) * 30, 0.1f);
                }
            }
        }

        public override void Kill(int timeLeft)
        {
            SpiritMod.tremorTime = 20;
            int maxgore = 25 + Main.rand.Next(-5, 6);
            for(int i = 0; i < maxgore; i++)
            {
                Vector2 vel = (i % 3 == 0) ? projectile.velocity/2 : projectile.velocity.RotatedBy(MathHelper.Pi / 2);
                vel = vel.RotatedByRandom((i % 3 == 0) ?  Math.PI / 2 : Math.PI / 6) * Main.rand.NextFloat(0.25f, 1f) * ((i % 2 == 0) ? 1 : -1);
                Gore.NewGore(projectile.Center, vel, mod.GetGoreSlot("Gores/StarjinxGore"));
            }

            Projectile proj = Projectile.NewProjectileDirect(projectile.Center, Vector2.Zero, mod.ProjectileType("NovaShockwave"), (int)(projectile.damage * (Charge + 1)), projectile.knockBack, projectile.owner, 0, Scale);
            proj.rotation = projectile.rotation;
            Main.PlaySound(mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/Thunder"), projectile.Center);
        }

        public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection) => damage = (int)(damage * ((Charge * 1.35f) + 1));

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit) => target.immune[projectile.owner] = 15;

        public override bool? CanHitNPC(NPC target) => !target.friendly && target.active && Timer >= 90;
    }

    internal class NovaShockwave : ModProjectile, IBasicPrimDraw
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Nova Star");
        }

        public override void SetDefaults()
        {
            projectile.friendly = true;
            projectile.width = projectile.height = 50;
            projectile.tileCollide = false;
            projectile.timeLeft = 20;
            projectile.ranged = true;
            projectile.friendly = true;
            projectile.penetrate = -1;
            projectile.ignoreWater = true;
            projectile.hide = true;
        }

        ref float Scale => ref projectile.ai[1];
        public override void AI()
        {
            Scale *= 1.08f;
            projectile.ai[0] += 0.075f;
        }

        public void DrawPrimShape(BasicEffect effect)
        {
            StarDraw.DrawStarBasic(effect, projectile.Center, projectile.rotation, Scale * 150, Color.Lerp(SpiritMod.StarjinxColor(Main.GlobalTime * 4), Color.Transparent, projectile.ai[0]));
        }

        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox) => projectile.Distance(targetHitbox.Center.ToVector2()) < (100 * Scale);

        public override bool? CanHitNPC(NPC target) => projectile.timeLeft > 10; //no double hits
    }
}