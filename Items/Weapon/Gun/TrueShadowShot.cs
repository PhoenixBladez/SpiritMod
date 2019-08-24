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
            DisplayName.SetDefault("True Shadow Shot");
            Tooltip.SetDefault("Shoots out multiple Shadow Bolts that home in on enemies\nAlso shoots out a Cursed Flame that inflicts 'Fel Brand,' dealing immense DoT");

        }


        public override void SetDefaults()
        {
            item.damage = 59;  
            item.ranged = true;   
            item.width = 65;     
            item.height = 28;   
            item.useTime = 32;
            item.useAnimation = 32;
            item.useStyle = 5;
            item.noMelee = true;
            item.knockBack = 8;
            item.useTurn = false;
            item.value = Terraria.Item.sellPrice(0, 3, 0, 0);
            item.rare = 8;
            item.UseSound = SoundID.Item36;
            item.autoReuse = true;
            item.shoot = 10;
            item.shootSpeed = 11f;
            item.useAmmo = AmmoID.Bullet;
        }

        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            int proj = Projectile.NewProjectile(position.X, position.Y, speedX, speedY, mod.ProjectileType("CursedBone"), damage, knockBack, player.whoAmI);
            Projectile newProj = Main.projectile[proj];
            newProj.friendly = true;
            newProj.hostile = false;
            for (int X = 0; X <= 4; X++)
            {
                Vector2 vel = new Vector2(speedX, speedY);
                if (Main.rand.Next(2) == 1)
                {
                    vel = vel.RotatedBy(System.Math.PI / (Main.rand.Next(82, 1800) / 10));
                }
                else
                {
                    vel = vel.RotatedBy(-System.Math.PI / (Main.rand.Next(82, 1800) / 10));
                }
                vel *= .7f + Main.rand.NextFloat(.6f);
                int proj2 = Projectile.NewProjectile(position.X, position.Y, vel.X, vel.Y, mod.ProjectileType("ShadowBolt"), damage, knockBack, player.whoAmI);
                Projectile newProj2 = Main.projectile[proj2];
            }
            return false;
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
