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
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 20;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
		}
        public override void SetDefaults()
        {
            Projectile.width = 16;
            Projectile.height = 16;
            Projectile.aiStyle = 0;
            Projectile.tileCollide = true;
            Projectile.timeLeft = 720;
            Projectile.extraUpdates = 2;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.friendly = true;
            Projectile.penetrate = 2;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 10;
            Projectile.ignoreWater = true;
            Projectile.scale = 0.7f;
        }
        public override void AI()
        {
            Projectile.rotation += 0.1f;
            float maxdist = 500;
            var validtargets = Main.npc.Where(x => x != null && x.CanBeChasedBy(this) && x.Distance(Projectile.Center) < maxdist);
            if (validtargets.Any() && Projectile.ai[0] == 0)
            {
                NPC finaltarget = null;
                foreach (NPC target in validtargets)
                {
                    maxdist = target.Distance(Projectile.Center);
                    finaltarget = target;
                }
                if(finaltarget != null)
                    Projectile.velocity = Vector2.Lerp(Projectile.velocity, Projectile.DirectionTo(finaltarget.Center) * 8, 0.05f);
            }

            if (Main.rand.Next(50) == 0)
                Gore.NewGore(Projectile.GetSource_FromAI(), Projectile.Center, Projectile.velocity / 4, Mod.Find<ModGore>("Gores/StarjinxGore").Type, 0.75f);

            if (Projectile.timeLeft < 25 || Projectile.penetrate <= 1)
                Fadeout();

            if (Projectile.ai[0] > 0)
            {
                Projectile.velocity *= 0.9f;
                Projectile.alpha += 10;
                if (Projectile.alpha > 255)
                    Projectile.Kill();
            }
        }
        private void Fadeout()
        {
            Projectile.ai[0]++;
            Projectile.tileCollide = false;
            Projectile.penetrate = -1;
            Projectile.damage = 0;
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Fadeout();
            Projectile.velocity = oldVelocity;
            return false;
        }
        public override bool? CanDamage()/* tModPorter Suggestion: Return null instead of false */ => Projectile.ai[0] == 0;

        public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            damage += Math.Max(target.defense / 2, 20); //ignore up to 40 defense
        }

        private float Timer => Main.GlobalTimeWrappedHourly * 4 + Projectile.ai[1];
        public override bool PreDraw(ref Color lightColor)
        {
            for (int i = 1; i < ProjectileID.Sets.TrailCacheLength[Projectile.type]; i++)
            {
                Vector2 newCenter = Projectile.oldPos[i] + Projectile.Size / 2;
                float lerpamount = 0.5f + (i / (float)(ProjectileID.Sets.TrailCacheLength[Projectile.type] * 2));
                float scale = (ProjectileID.Sets.TrailCacheLength[Projectile.type] - i) / (float)ProjectileID.Sets.TrailCacheLength[Projectile.type];
                Main.spriteBatch.Draw(Mod.Assets.Request<Texture2D>("Textures/StardustPillarStar").Value,
                    newCenter - Main.screenPosition,
                    null,
                    Color.Lerp(SpiritMod.StarjinxColor(Main.GlobalTimeWrappedHourly * 4 + i), Color.Transparent, lerpamount) * Projectile.Opacity, 0, new Vector2(36, 36),
                    scale * ((float)Math.Sin(Projectile.timeLeft / 14) / 8 + 1) * 0.4f,
                    SpriteEffects.None,
                    0);
            }
            Main.spriteBatch.Draw(Mod.Assets.Request<Texture2D>("Textures/StardustPillarStar").Value,
                Projectile.Center - Main.screenPosition,
                null,
                Color.Lerp(SpiritMod.StarjinxColor(Main.GlobalTimeWrappedHourly * 4), Color.Transparent, 0.75f) * Projectile.Opacity, 
                0, 
                new Vector2(36, 36),
                Projectile.scale * ((float)Math.Sin(Projectile.timeLeft / 14) / 8 + 1),
                SpriteEffects.None,
                0);

            return false;
        }
    }
}