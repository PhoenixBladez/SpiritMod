using Terraria;
using System;
using Terraria.ID;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Terraria.ModLoader;

namespace SpiritMod.Items.Weapon.Gun
{
    public class TrueShadowShot : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Nightbane");
            Tooltip.SetDefault("Shoots out bouncing vile bullets\nRight-click to shoot out a tracker that sticks to enemies\nBullets will home in on tracked enemies\nThe tracker will regularly shoot out pulses of Cursed Flame that inflict 'Fel Brand'");

        }


        public override void SetDefaults()
        {
            item.damage = 44;  
            item.ranged = true;   
            item.width = 65;     
            item.height = 28;   
            item.useTime = 16;
            item.useAnimation = 16;
            item.useStyle = 5;
            item.noMelee = true;
            item.knockBack = 8;
            item.useTurn = false;
            item.value = Terraria.Item.sellPrice(0, 5, 0, 0);
            item.rare = 8;
            item.UseSound = SoundID.Item36;
            item.autoReuse = true;
            item.shoot = 10;
            item.shootSpeed = 11f;
            item.useAmmo = AmmoID.Bullet;
        }
        public override bool AltFunctionUse(Player player)
        {
            return true;
        }
        public override bool CanUseItem(Player player)
        {
            if (player.altFunctionUse == 2)
            {

                MyPlayer modPlayer = player.GetSpiritPlayer();
                if (modPlayer.shootDelay2 == 0)
                    return true;
                return false;
            }
            else
            {
                return true;
            }
        }
        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            Vector2 muzzleOffset = Vector2.Normalize(new Vector2(speedX, speedY)) * 45f;
            if (Collision.CanHit(position, 0, 0, position + muzzleOffset, 0, 0))
            {
                position += muzzleOffset;
            }
            if (player.altFunctionUse == 2)
            {
                Main.PlaySound(new Terraria.Audio.LegacySoundStyle(2, 94));
                MyPlayer modPlayer = player.GetSpiritPlayer();
                modPlayer.shootDelay2 = 300;
                Projectile.NewProjectile(position.X, position.Y, speedX, speedY, mod.ProjectileType("TrueShadowShotTracker"), item.damage / 3, knockBack, item.owner, 0, 0);
            }
            else
            {
                Main.PlaySound(new Terraria.Audio.LegacySoundStyle(2, 11));
                float spread = 30f * 0.0174f;//45 degrees converted to radians
                float baseSpeed = (float)Math.Sqrt(speedX * speedX + speedY * speedY);
                double baseAngle = Math.Atan2(speedX, speedY);
                {
                    double randomAngle = baseAngle + (Main.rand.NextFloat() - 0.5f) * spread;
                    speedX = baseSpeed * (float)Math.Sin(randomAngle);
                    speedY = baseSpeed * (float)Math.Cos(randomAngle);
                    Projectile.NewProjectile(position.X, position.Y, speedX, speedY, mod.ProjectileType("TrueVileBullet"), item.damage, knockBack, item.owner, 0, 0);
                }
            }
            return false;
        }
        public override void HoldItem(Player player)
        {
            MyPlayer modPlayer = player.GetSpiritPlayer();
            if (modPlayer.shootDelay2 == 1)
            {
                Main.PlaySound(25, -1, -1, 1, 1f, 0.0f);
                for (int index1 = 0; index1 < 5; ++index1)
                {
                    int index2 = Dust.NewDust(player.position, player.width, player.height, 75, 0.0f, 0.0f, (int)byte.MaxValue, new Color(), (float)Main.rand.Next(20, 26) * 0.1f);
                    Main.dust[index2].noLight = false;
                    Main.dust[index2].noGravity = true;
                    Main.dust[index2].velocity *= 0.5f;
                }
            }
        }
        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-10, 0);
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(null, "ShadowShot", 1);
            recipe.AddIngredient(null, "BrokenParts", 1);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.SetResult(this, 1);
            recipe.AddRecipe();
        }

    }
}
