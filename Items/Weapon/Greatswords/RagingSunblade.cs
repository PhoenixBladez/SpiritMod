using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using SpiritMod.Prim;
using SpiritMod.Utilities;
using System.Collections.Generic;
using System.Linq;

namespace SpiritMod.Items.Weapon.Greatswords
{
    public class RagingSunblade : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Helios");
            SpiritGlowmask.AddGlowMask(item.type, Texture + "_glow");
        }

        public override void SetDefaults()
        {
            item.channel = true;
            item.damage = 180;
            item.width = 60;
            item.height = 60;
            item.useTime = 320;
            item.useAnimation = 320;
            item.crit = 4;
            item.useStyle = ItemUseStyleID.SwingThrow;
            item.melee = true;
            item.noMelee = true;
            item.knockBack = 8;
            item.useTurn = false;
            item.value = Item.sellPrice(0, 10, 0, 0);
            item.rare = ItemRarityID.Red;
            item.autoReuse = true;
            item.shoot = mod.ProjectileType("RagingSunbladeProj");
            item.shootSpeed = 6f;
            item.noUseGraphic = true;
        }
        public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI) => 
            GlowmaskUtils.DrawItemGlowMaskWorld(Main.spriteBatch, item, mod.GetTexture(Texture.Remove(0, "Starjinx/".Length) + "_glow"), rotation, scale);
        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-10, 0);
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.FragmentSolar, 18);
            recipe.AddTile(TileID.LunarCraftingStation);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }

    public class RagingSunbladeProj : ModProjectile
    {
        public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Helios");
        }

        public override void SetDefaults()
		{
			projectile.width = 45;
			projectile.height = 45;
			projectile.friendly = true;
			projectile.penetrate = -1;
			projectile.tileCollide = false;
			projectile.ignoreWater = true;
			projectile.aiStyle = -1;
		}
        float growCounter;
        double radians = 0;
        bool released = false;
        bool primsCreated = false;
        SolarSwordPrimTrail trail;
        public override void AI()
        {
            Player player = Main.player[projectile.owner];
            player.heldProj = projectile.whoAmI;
            if (!primsCreated)
            {
                trail = new SolarSwordPrimTrail(projectile);
                SpiritMod.primitives.CreateTrail(trail);
                primsCreated = true;
            }
            if (!released && player.channel)
            {
                player.GetModPlayer<MyPlayer>().usingHelios = true;
                player.maxFallSpeed *= 24f;
                player.jumpSpeedBoost *= 1.6f;
                player.accRunSpeed *= 1.6f;

                //CheckCollision(player);
            }
            else
            {
                if (!released)
                {
                    released = true;
                }
            }
            Vector2 direction = Main.MouseWorld - player.position;
            direction.Normalize();
            projectile.scale = MathHelper.Clamp(growCounter / 30f, 0, 1);

            SpinBlade(0.3, 0.1);

            player.itemAnimation -= 14;

            while (player.itemAnimation < 3)
            {
                Main.PlaySound(SoundID.Item, (int)player.MountedCenter.X, (int)player.MountedCenter.Y, 105, 0.5f, 0.5f);
                player.itemAnimation += 320;
            }
            player.itemTime = player.itemAnimation;

             if (growCounter <= 0)
            {
                projectile.active = false;
                player.itemTime = player.itemAnimation = 2;
            }

            trail._direction = player.direction;
        }

        private void CheckCollision(Player player) //I'm sorry forthis
        {
            int pX = (int)(player.Center.X / 16f);
            int pY = (int)(player.Center.Y / 16f);

            List<(float, float)> collectedAngles = new List<(float, float)>(); //angles hit at

            for (int i = pX - 10; i < pX + 10; ++i)
            {
                for (int j = pY - 10; j < pY + 10; ++j)
                {
                    float dist = Vector2.Distance((new Vector2(i, j) * 16) + new Vector2(8), player.Center);
                    if (dist < 150 * (growCounter / 60f) && Framing.GetTileSafely(i, j).active() && Main.tileSolid[Framing.GetTileSafely(i, j).type])
                        collectedAngles.Add((Vector2.Normalize(player.Center - (new Vector2(i, j) * 16)).ToRotation(), dist)); //collects angles
                }
            }

            collectedAngles.Sort(); //sort angles

            if (collectedAngles.Count > 0) //oh no
            {
                float angle = 0f;
                float adjAngle = 0;
                int index = collectedAngles.Count - 1;
                do
                {
                    if (index < 0)
                        return; //return if there's no valid angle
                    angle = collectedAngles[index].Item1;
                    adjAngle = angle += (float)(angle >= 0 ? Math.PI : -Math.PI);
                    if (adjAngle > Math.PI)
                        adjAngle = -(adjAngle - (float)Math.PI);
                    if (adjAngle < Math.PI)
                        adjAngle = -(adjAngle + (float)Math.PI);
                    index--; //adjusts angles; checks if there's an angle similar to it but opposite and denies the movement if there is
                } while (collectedAngles.Any(x => x.Item1 > adjAngle - MathHelper.PiOver2 && x.Item1 < adjAngle + MathHelper.PiOver2)); //god help me

                player.velocity = new Vector2(1, 0).RotatedBy(angle + Math.PI) * 20; //set the player's position
            }
        }

        private void SpinBlade(double d1, double adder)
        {
            Player player = Main.player[projectile.owner];
            for (double i = 0; i < d1; i+= adder)
            {
                if (player.direction == 1)
                    radians += adder;
                else
                    radians -= adder;

                if (radians > 6.28)
                    radians -= 6.28;
                if (radians < -6.28)
                    radians += 6.28;
                if (growCounter < 60 && !released)
                    growCounter+= (float)(adder / d1);
                if (released)
                    growCounter-= (float)(adder / d1);
                projectile.Center = player.Center + ((float)radians + 3.14f).ToRotationVector2() * 10 * growCounter / 6f;
                trail.Points.Add(projectile.Center - player.Center);
                if (Main.rand.Next(4) == 0)
                {
                    Dust dust = Dust.NewDustDirect(projectile.Center - new Vector2(projectile.width / 4, projectile.height / 4), projectile.width / 2, projectile.height / 2, 6, ((float)radians + 3.14f).ToRotationVector2().X * growCounter / 6f, ((float)radians + 3.14f).ToRotationVector2().Y * growCounter / 6f);
                    dust.scale = 1.5f;
                    dust.noGravity = true;
                    dust = Dust.NewDustDirect(player.Center - (((float)radians + 3.14f).ToRotationVector2() * 10 * growCounter / 6f) - new Vector2(projectile.width / 4, projectile.height / 4), projectile.width / 2, projectile.height / 2, 6, ((float)radians + 3.14f).ToRotationVector2().X * growCounter / -6f, ((float)radians + 3.14f).ToRotationVector2().Y * growCounter / -2f);
                    dust.scale = 1.5f;
                    dust.noGravity = true;
                }
            }
        }

        public override void ModifyHitNPC(NPC target, ref int damage, ref float knockBack, ref bool crit, ref int hitDirection) => target.AddBuff(189, 180);

        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            Player player = Main.player[projectile.owner];
            if (Collision.CheckAABBvLineCollision(targetHitbox.Center(), targetHitbox.Size(), player.Center, player.Center + ((float)radians + 3.14f).ToRotationVector2() * 17 * growCounter / 6f))
                return true;
            if (Collision.CheckAABBvLineCollision(targetHitbox.Center(), targetHitbox.Size(), player.Center, player.Center - ((float)radians + 3.14f).ToRotationVector2() * 17 * growCounter / 6f))
                return true;
            return projHitbox.Intersects(targetHitbox);
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			Color color = lightColor;
            Main.spriteBatch.Draw(Main.projectileTexture[projectile.type], Main.player[projectile.owner].Center - Main.screenPosition, null, Color.White, (float)radians + 3.9f, new Vector2(Main.projectileTexture[projectile.type].Width / 2, Main.projectileTexture[projectile.type].Height / 2), projectile.scale, SpriteEffects.None, 0);
			return false;
		}
    }
}