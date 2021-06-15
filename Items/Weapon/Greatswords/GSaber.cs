using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using SpiritMod.ChargeMeter;
using SpiritMod.Prim;
using SpiritMod.Utilities;

namespace SpiritMod.Items.Weapon.Greatswords
{
    public class GSaber : ModItem
    {

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Granitech Saber");
            Tooltip.SetDefault("Hold to charge up multiple energy slashes\n'Cutting-edge, guaranteed!'");
        }

        public override void SetDefaults()
        {
            item.channel = true;
            item.damage = 54;
            item.width = 60;
            item.height = 60;
            item.useTime = 60;
            item.useAnimation = 60;
            item.useStyle = ItemUseStyleID.HoldingOut;
            Item.staff[item.type] = true;
            item.melee = true;
            item.noMelee = true;
            item.knockBack = 7;
            item.useTurn = false;
            item.value = Terraria.Item.sellPrice(0, 2, 20, 0);
            item.rare = 4;
            item.autoReuse = true;
            item.shoot = mod.ProjectileType("GSaberProj");
            item.shootSpeed = 6f;
            item.noUseGraphic = true;
        }

		public override Vector2? HoldoutOffset() => new Vector2(-10, 0);
	}

    public class GSaberProj : ModProjectile
    {
        protected int flickerTime = 20; //dont change these
        protected bool released = false;
        protected bool maxcharge = false;
        protected bool primsCreated = false;
        protected float offset = 0;
        protected Vector2 direction = Vector2.Zero;
        protected int growCounter = 0;
        protected float charge;
        protected float swings = 0;
        protected int timesSwung = 0;
        protected float swingAcc = 0;

		float chargeRate = 0.75f;
		float chargePerSwing = 15;
		float dechargeRate = 1.5f;
		float swingRadians = 2.333f;
		float swingMax = 9;
		int range = 100;
		int extraRange = 25;

		public float Radians
        {
            get
            {
                Player player = Main.player[projectile.owner];
                return player.itemRotation;
            }
            set
            {
                Player player = Main.player[projectile.owner];
                player.itemRotation = value;
                if (player.direction != 1)
                    player.itemRotation -= 3.14f;
                player.itemRotation = MathHelper.WrapAngle(player.itemRotation);
            }
        }
        public sealed override void SetStaticDefaults()
		{
			Main.projFrames[projectile.type] = 3;
            ProjectileID.Sets.TrailCacheLength[projectile.type] = 2;
            ProjectileID.Sets.TrailingMode[projectile.type] = 0;
        }
        public sealed override void SetDefaults()
		{
			projectile.hostile = false;
			projectile.melee = true;
			projectile.width = projectile.height = 80;
			projectile.aiStyle = -1;
			projectile.friendly = true;
			projectile.penetrate = -1;
			projectile.tileCollide = false;
            projectile.alpha = 255;
		}

        public override void AI()
        {
            projectile.scale = growCounter < 10 ? (growCounter / 10f) : 1;
            growCounter++;

            Player player = Main.player[projectile.owner];
            player.heldProj = projectile.whoAmI;
            player.itemTime = 2;
            player.itemAnimation = 2;
            player.ChangeDir(Main.MouseWorld.X > player.position.X ? 1 : -1);

            direction = Vector2.Normalize(Main.MouseWorld - (player.Center - new Vector2(4, 4)));

            if (player.channel && !released)
            {
                offset = 0 - swingRadians / 2;
                projectile.Center = player.MountedCenter + (direction.RotatedBy(offset) * range);
                if (swings < swingMax)
                {
                    charge+= chargeRate;
                    if (charge >= chargePerSwing)
                    {
                        charge -= chargePerSwing;
                        swings++;
                        Main.PlaySound(SoundID.MaxMana, (int)player.MountedCenter.X, (int)player.MountedCenter.Y, 1, 1.5f, -0.2f);
                        flickerTime = 0;
                    }
                }
            }
            else
            {
                Color color = new Color(130, 23, 252);
                if (swings > 0)
                    Lighting.AddLight(projectile.Center, color.R / 200f, color.G / 200f, color.B / 200f);
                if (!released)
                {
                    if (swings > 0)
                        Main.PlaySound(SoundID.Item, (int)player.MountedCenter.X, (int)player.MountedCenter.Y, 105, 0.5f, 0.5f);
                    released = true;
                    charge = chargePerSwing;
                }
                charge -= dechargeRate;
                float addToOffset = (dechargeRate / chargePerSwing) * swingRadians;
                if (timesSwung % 2 == 1)
                    addToOffset *= -1;
                if (addToOffset > 0)
                {
                    if (swingAcc < addToOffset)
                        swingAcc += 0.15f;
                    else
                        swingAcc = addToOffset;
                }
                if (addToOffset < 0)
                {
                    if (swingAcc > addToOffset)
                        swingAcc -= 0.15f;
                    else
                        swingAcc = addToOffset;
                } 
                offset += swingAcc;
                projectile.Center = player.MountedCenter + (direction.RotatedBy(offset) * (range + (extraRange * timesSwung)));
                if (!primsCreated && charge < chargePerSwing / 2)
                {
                    primsCreated = true;
                    CreatePrims(player.Center + (direction.RotatedBy((swingRadians + (timesSwung * 0.125f)) * player.direction / 3) * (range + (extraRange * timesSwung))), 
                    player.Center + (direction.RotatedBy((swingRadians + (timesSwung * 0.125f)) * player.direction / 6) *  1.5f * (range + (extraRange * (timesSwung)))),
                    player.Center + (direction.RotatedBy((swingRadians + (timesSwung * 0.125f)) * player.direction / -6) * 1.5f * (range + (extraRange * (timesSwung)))), 
                    player.Center + (direction.RotatedBy((swingRadians + (timesSwung * 0.125f)) * player.direction / -3) * (range + (extraRange * timesSwung))));
                }
                if (charge < 0)
                {
                    charge = chargePerSwing;
                    timesSwung++;
                    primsCreated = false;
                    if (timesSwung < swings)
                        Main.PlaySound(SoundID.Item, (int)player.MountedCenter.X, (int)player.MountedCenter.Y, 105, 0.5f, 0.5f);
                }
                if (timesSwung >= swings)
                    projectile.active = false;
            }
            Radians = direction.ToRotation() + (offset * player.direction);
            if (player.direction == -1)
                Radians += 3.14f;
        }
        
        public SpriteEffects Effects => ((Main.player[projectile.owner].direction * (int)Main.player[projectile.owner].gravDir) < 0) ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
        public float Truerotation => (Radians + 0.76f) - ((Effects == SpriteEffects.FlipHorizontally) ? MathHelper.PiOver2 : 0);
        public Vector2 Origin => (Effects == SpriteEffects.FlipHorizontally) ? new Vector2(Main.projectileTexture[projectile.type].Width, Main.projectileTexture[projectile.type].Height / 3) : new Vector2(0, Main.projectileTexture[projectile.type].Height / 3);

		public sealed override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            Texture2D tex = Main.projectileTexture[projectile.type];
			Color color = lightColor;
			Main.spriteBatch.Draw(tex, Main.player[projectile.owner].Center - Main.screenPosition, new Rectangle(0, 0, tex.Width, tex.Height / 3), color, Truerotation, Origin, projectile.scale, Effects, 0);
            Main.spriteBatch.Draw(tex, Main.player[projectile.owner].Center - Main.screenPosition, new Rectangle(0, 2 * (tex.Height / 3), tex.Width, tex.Height / 3), Color.White, Truerotation, Origin, projectile.scale, Effects, 0);
			if (!released && flickerTime < 16) {
				flickerTime++;
				color = Color.White;
				float flickerTime2 = (float)(flickerTime / 20f);
				float alpha = 1.5f - (((flickerTime2 * flickerTime2) / 2) + (2f * flickerTime2));
				if (alpha < 0) {
					alpha = 0;
				}
				Main.spriteBatch.Draw(tex, Main.player[projectile.owner].Center - Main.screenPosition, new Rectangle(0, tex.Height / 3, tex.Width, tex.Height / 3), color * alpha, Truerotation, Origin, projectile.scale, Effects, 1);
			}
            return false;
        }

        public override bool CanDamage() => released; //only damage enemies before it hits four enemies(piercing but without killing projectile), and after it starts being swung

        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            Player player = Main.player[projectile.owner];
            if (Collision.CheckAABBvLineCollision(targetHitbox.Center(), targetHitbox.Size(), player.Center, projectile.Center + (direction.RotatedBy(offset * player.direction) * 40)))
                return true;

            return projHitbox.Intersects(targetHitbox);
        }

        public override void ModifyDamageHitbox(ref Rectangle hitbox)
        {
            Player player = Main.player[projectile.owner];
            hitbox.X = (int)(player.MountedCenter.X + (direction.RotatedBy(offset * player.direction) * (range + (extraRange * timesSwung))).X) - projectile.height/2;
            hitbox.Y = (int)(player.MountedCenter.Y + (direction.RotatedBy(offset * player.direction) * (range + (extraRange * timesSwung))).Y) - projectile.width/2;
        }

        public void CreatePrims(Vector2 start, Vector2 mid, Vector2 mid2, Vector2 end)
        {
            bool reverse = true;
            if (timesSwung % 2 == 1)
                reverse = !reverse;
            if (Main.player[projectile.owner].direction == -1)
                reverse = !reverse;
            if (timesSwung % 2 == 0)
            {
				SpiritMod.primitives.CreateTrail(new GSaberPrimTrail(projectile, 
					start, 
					mid,
					mid2, 
					end,
					reverse));
            }
            else
            {
                SpiritMod.primitives.CreateTrail(new GSaberPrimTrail(projectile, 
					end, 
					mid2,
					mid, 
					start,
					reverse));
            }
        }
    }
}