using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using System.Linq;
using Terraria.ID;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace SpiritMod.Items.Armor.StarjinxSet
{
    public class ManajinxStar : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Starlight Energy");
            ProjectileID.Sets.TrailCacheLength[projectile.type] = 20;
            ProjectileID.Sets.TrailingMode[projectile.type] = 2;
        }
        public override void SetDefaults()
        {
            projectile.width = 16;
            projectile.height = 16;
            projectile.aiStyle = 0;
            projectile.tileCollide = true;
            projectile.timeLeft = 720;
            projectile.extraUpdates = 2;
            projectile.magic = true;
            projectile.friendly = true;
            projectile.penetrate = 2;
            projectile.usesLocalNPCImmunity = true;
            projectile.localNPCHitCooldown = 10;
            projectile.ignoreWater = true;
            projectile.scale = 0.7f;
        }
        public override void AI()
        {
            projectile.rotation += 0.1f;
            float maxdist = 500;
            var validtargets = Main.npc.Where(x => x != null && x.CanBeChasedBy(this) && x.Distance(projectile.Center) < maxdist);
            if (validtargets.Any() && projectile.ai[0] == 0)
            {
                NPC finaltarget = null;
                foreach (NPC target in validtargets)
                {
                    maxdist = target.Distance(projectile.Center);
                    finaltarget = target;
                }
                if(finaltarget != null)
                    projectile.velocity = Vector2.Lerp(projectile.velocity, projectile.DirectionTo(finaltarget.Center) * 8, 0.05f);
            }

            if (Main.rand.Next(50) == 0)
                Gore.NewGore(projectile.Center, projectile.velocity / 4, mod.GetGoreSlot("Gores/StarjinxGore"), 0.75f);

            if (projectile.timeLeft < 25 || projectile.penetrate <= 1)
                Fadeout();

            if (projectile.ai[0] > 0)
            {
                projectile.velocity *= 0.9f;
                projectile.alpha += 10;
                if (projectile.alpha > 255)
                    projectile.Kill();
            }
        }
        private void Fadeout()
        {
            projectile.ai[0]++;
            projectile.tileCollide = false;
            projectile.penetrate = -1;
            projectile.damage = 0;
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Fadeout();
            projectile.velocity = oldVelocity;
            return false;
        }
        public override bool CanDamage() => projectile.ai[0] == 0;

        public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            damage += Math.Max(target.defense / 2, 20); //ignore up to 40 defense
        }

        private static readonly BasicEffect effect = new BasicEffect(Main.instance.GraphicsDevice);
        private float Timer => Main.GlobalTime * 4 + projectile.ai[1];
        public override bool PreDraw(SpriteBatch spriteBatch, Color drawColor)
        {
            effect.VertexColorEnabled = true;
            for (int i = 1; i < ProjectileID.Sets.TrailCacheLength[projectile.type]; i++)
            {
                Vector2 newCenter = projectile.oldPos[i] + projectile.Size / 2;
                float lerpamount = 0.5f + (i / (float)(ProjectileID.Sets.TrailCacheLength[projectile.type] * 2));
                float scale = (ProjectileID.Sets.TrailCacheLength[projectile.type] - i) / (float)ProjectileID.Sets.TrailCacheLength[projectile.type];
                spriteBatch.Draw(mod.GetTexture("Extras/StardustPillarStar"),
                    newCenter - Main.screenPosition,
                    null,
                    Color.Lerp(SpiritMod.StarjinxColor(Main.GlobalTime * 4 + i), Color.Transparent, lerpamount) * projectile.Opacity, 0, new Vector2(36, 36),
                    scale * ((float)Math.Sin(projectile.timeLeft / 14) / 8 + 1) * 0.4f,
                    SpriteEffects.None,
                    0);
            }
            spriteBatch.Draw(mod.GetTexture("Extras/StardustPillarStar"),
                projectile.Center - Main.screenPosition,
                null,
                Color.Lerp(SpiritMod.StarjinxColor(Main.GlobalTime * 4), Color.Transparent, 0.75f) * projectile.Opacity, 
                0, 
                new Vector2(36, 36),
                projectile.scale * ((float)Math.Sin(projectile.timeLeft / 14) / 8 + 1),
                SpriteEffects.None,
                0);

            StarDraw.DrawStarBasic(effect, projectile.Center, projectile.rotation, projectile.scale * 30, Color.Lerp(SpiritMod.StarjinxColor(Timer), Color.Black, 0.15f) * projectile.Opacity);
            StarDraw.DrawStarBasic(effect, projectile.Center, projectile.rotation, projectile.scale * 20, Color.Lerp(SpiritMod.StarjinxColor(Timer), Color.White, 0.75f) * projectile.Opacity);
            return false;
        }
    }
}