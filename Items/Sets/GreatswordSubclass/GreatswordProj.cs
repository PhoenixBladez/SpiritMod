using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using SpiritMod.UI.ChargeMeter;
using SpiritMod.Prim;
using SpiritMod.Utilities;

namespace SpiritMod.Items.Sets.GreatswordSubclass
{
    public abstract class GreatswordProj : ModProjectile
	{
        public sealed override void SetStaticDefaults()
		{
            SafeSetStaticDefaults();
			Main.projFrames[Projectile.type] = 3;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 2;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
        }
        public sealed override void SetDefaults()
		{
			Projectile.hostile = false;
			Projectile.DamageType = DamageClass.Melee;
			Projectile.width = Projectile.height = 80;
			Projectile.aiStyle = -1;
			Projectile.friendly = true;
			Projectile.penetrate = -1;
			Projectile.tileCollide = false;
            Projectile.alpha = 255;
            SafeSetDefaults();
		}
        public virtual void SafeSetStaticDefaults() {}
        public virtual void SafeSetDefaults() {}
        public virtual void SafeAI(){}
        public virtual void SafePostAI(){}
        public virtual void CreatePrims(Vector2 start, Vector2 mid, Vector2 end) {}

        protected int flickerTime = 0; //dont change these
        protected bool released = false;
        protected bool maxcharge = false;
        protected float angularMomentum = 1;
        protected float offset = 0.2f;
        protected bool primsCreated = false;
        protected Vector2 direction = Vector2.Zero;
        protected float minOffset = 0.2f;
        protected int growCounter = 0;
        protected float charge
		{
			get {return Projectile.ai[0];}
			set {Projectile.ai[0] = value;}
		}

        public float radians
        {
            get
            {
                Player player = Main.player[Projectile.owner];
                return player.itemRotation;
            }
            set
            {
                Player player = Main.player[Projectile.owner];
                player.itemRotation = value;
                if (player.direction != 1)
                {
                    player.itemRotation -= 3.14f;
                }
                player.itemRotation = MathHelper.WrapAngle(player.itemRotation);
            }
        }

        protected float chargeMax; //change these
        protected int minDamage;
        protected int maxDamage;
        protected int minKnockback;
        protected int maxKnockback;
        protected float chargeRate;
        protected float swingSpeed;
        protected float pullBack;
        protected float maxOffset;
        public override void AI()
        {
            SafeAI();
            Projectile.scale = growCounter < 10 ? (Projectile.ai[0] / 10f) : 1;
            Player player = Main.player[Projectile.owner];
            player.itemTime = 2;
            player.itemAnimation = 2;
            Projectile.scale = charge < 10 ? (charge / 10f) : 1;
            player.ChangeDir(Main.MouseWorld.X > player.position.X ? 1 : -1);

            Projectile.Center = player.MountedCenter + (direction.RotatedBy(offset * player.direction) * (charge));
            Projectile.damage = minDamage + (int)((maxDamage - minDamage) * (charge / (float)chargeMax) * player.GetDamage(DamageClass.Melee));
            if (player.channel && !released)
            {
                direction = Main.MouseWorld - (player.Center - new Vector2(4, 4));
			    direction.Normalize();
                if (charge < chargeMax)
                {
                    charge+= chargeRate;
                    if (charge < chargeMax / 1.5f)
                         offset -= pullBack;
                }
                if (offset < minOffset)
                    minOffset = offset;
                 ChargeMeterPlayer modPlayer = player.GetModPlayer<ChargeMeterPlayer>();
                if(charge >= chargeMax && !maxcharge)
                {
                    maxcharge = true;
                    SoundEngine.PlaySound(SoundID.MaxMana, (int)player.MountedCenter.X, (int)player.MountedCenter.Y, 1, 1.5f, -0.2f);
                }
            }
            else
            {
                if (!released)
                {
                    SoundEngine.PlaySound(SoundID.Item, (int)player.MountedCenter.X, (int)player.MountedCenter.Y, 105, 0.5f + charge / (chargeMax / 2), 0.5f - charge / chargeMax);
                    released = true;
                }
                offset += 0.2f;
                if(offset > (minOffset + maxOffset) / 2 && !primsCreated)
                {
                    primsCreated = true;
                    CreatePrims(player.Center + (direction.RotatedBy(maxOffset * player.direction) * (50 + charge)), 
                    player.Center + (direction.RotatedBy(offset * player.direction) * (50 + charge)), 
                    player.Center + (direction.RotatedBy(minOffset  * player.direction) * (50 + charge)));
                }
                if (offset > maxOffset)
                {
                    Projectile.active = false;
                }
            }
            radians = direction.ToRotation() + (offset * player.direction);
            if (player.direction == -1)
                radians += 3.14f;
            SafePostAI();
        }
        public SpriteEffects Effects => ((Main.player[Projectile.owner].direction * (int)Main.player[Projectile.owner].gravDir) < 0) ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
        public float Truerotation => ((float)radians + 0.76f) - ((Effects == SpriteEffects.FlipHorizontally) ? MathHelper.PiOver2 : 0);
        public Vector2 Origin => (Effects == SpriteEffects.FlipHorizontally) ? new Vector2(TextureAssets.Projectile[Projectile.type].Value.Width, TextureAssets.Projectile[Projectile.type].Value.Height / 3) : new Vector2(0, TextureAssets.Projectile[Projectile.type].Value.Height / 3);
        public sealed override bool PreDraw(ref Color lightColor)
        {
            Texture2D tex = TextureAssets.Projectile[Projectile.type].Value;
			Color color = lightColor;
			Main.spriteBatch.Draw(tex, Main.player[Projectile.owner].Center - Main.screenPosition, new Rectangle(0, 0, tex.Width, tex.Height / 3), color, Truerotation, Origin, Projectile.scale, Effects, 0);
            Main.spriteBatch.Draw(tex, Main.player[Projectile.owner].Center - Main.screenPosition, new Rectangle(0, 2 * (tex.Height / 3), tex.Width, tex.Height / 3), Color.White, Truerotation, Origin, Projectile.scale, Effects, 0);
			if (maxcharge && !released && flickerTime < 16) {
				flickerTime++;
				color = Color.White;
				float flickerTime2 = (float)(flickerTime / 20f);
				float alpha = 1.5f - (((flickerTime2 * flickerTime2) / 2) + (2f * flickerTime2));
				if (alpha < 0) {
					alpha = 0;
				}
				Main.spriteBatch.Draw(tex, Main.player[Projectile.owner].Center - Main.screenPosition, new Rectangle(0, tex.Height / 3, tex.Width, tex.Height / 3), color * alpha, Truerotation, Origin, Projectile.scale, Effects, 1);
			}
            return false;
        }
        public override bool? CanDamage()/* tModPorter Suggestion: Return null instead of false */ => released; //only damage enemies before it hits four enemies(piercing but without killing projectile), and after it starts being swung

        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            Player player = Main.player[Projectile.owner];
            if (Collision.CheckAABBvLineCollision(targetHitbox.Center(), targetHitbox.Size(), player.Center, Projectile.Center + (direction.RotatedBy(offset * player.direction) * 40)))
                return true;

            return projHitbox.Intersects(targetHitbox);
        }
        public override void ModifyDamageHitbox(ref Rectangle hitbox)
        {
            Player player = Main.player[Projectile.owner];
            hitbox.X = (int)(player.MountedCenter.X + (direction.RotatedBy(offset * player.direction) * Math.Min(charge + 40, chargeMax)).X) - Projectile.height/2;
            hitbox.Y = (int)(player.MountedCenter.Y + (direction.RotatedBy(offset * player.direction) * Math.Min(charge + 40, chargeMax)).Y) - Projectile.width/2;
        }
    }
}