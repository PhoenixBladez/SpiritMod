using Terraria;
using System;
using Terraria.ID;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Terraria.ModLoader;
using SpiritMod.Projectiles;

namespace SpiritMod.Items.Weapon.Bow
{
    public class Talonshot : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Talonshot");
			Tooltip.SetDefault("Occasionally shoots out an arcane feather.");
		}


        int charger;
        public override void SetDefaults()
        {
            item.damage = 25;
            item.noMelee = true;
            item.ranged = true;
            item.width = 20;
            item.height = 38;
            item.useTime = 34;
            item.useAnimation = 34;
            item.useStyle = 5;
            item.shoot = 3;
            item.useAmmo = AmmoID.Arrow;
            item.knockBack = 3;
            item.value = 1000;
            item.rare = 3;
            item.UseSound = SoundID.Item5;
			item.value = Item.buyPrice(0, 5, 0, 0);
			item.value = Item.sellPrice(0, 1, 0, 0);
            item.autoReuse = true;
            item.shootSpeed = 14f;
            item.crit = 8;

        }
        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            {
                {
                    charger++;
                    if (charger >= 5)
                    {
                        for (int I = 0; I < 1; I++)
                        {
                            Projectile.NewProjectile(position.X - 8, position.Y + 8, speedX + ((float)Main.rand.Next(-230, 230) / 100), speedY + ((float)Main.rand.Next(-230, 230) / 100), mod.ProjectileType("GiantFeather"), damage, knockBack, player.whoAmI, 0f, 0f);
                        }
                        charger = 0;
                    }
                    return true;
                }
            }
            {

                Vector2 origVect = new Vector2(speedX, speedY);
                Vector2 newVect1 = origVect.RotatedBy(System.Math.PI / 17);
                Vector2 newVect2 = origVect.RotatedBy(-System.Math.PI / 17);
                int projShot0 = Terraria.Projectile.NewProjectile(position.X, position.Y, newVect2.X, newVect2.Y, type, damage, knockBack, player.whoAmI, 0f, 0f);
                Projectile.NewProjectile(position.X, position.Y, newVect1.X, newVect1.Y, type, damage, knockBack, player.whoAmI, 0f, 0f);
                return true;
            }
        }
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(null, "Talon", 14);
            recipe.AddIngredient(null, "FossilFeather", 1);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}