using Terraria;
using System;
using Terraria.ID;
using System.Diagnostics;
using SpiritMod.Projectiles;
using Microsoft.Xna.Framework;
using Terraria.ModLoader;

namespace SpiritMod.Items.Weapon.Bow
{
    public class MarbleBow : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Gilded Bow");
			Tooltip.SetDefault("Right-click to shoot slower, more powerful arrows");
		}



        public override void SetDefaults()
        {
            item.damage = 21;
            item.noMelee = true;
            item.ranged = true;
            item.width = 22;
            item.height = 46;
            item.useTime = 34;
            item.useAnimation = 34;
            item.useStyle = 5;
            item.shoot = 1;
            item.useAmmo = AmmoID.Arrow;
            item.knockBack = 1;
            item.value = Terraria.Item.sellPrice(0, 2, 0, 0);
            item.rare = 3;
            item.UseSound = SoundID.Item5;
            item.autoReuse = false;
            item.useTurn = false;
            item.shootSpeed = 6.2f;
        }
        public override bool AltFunctionUse(Player player)
        {
            return true;
        }
        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            if (player.altFunctionUse == 2)
            {
                int p = Projectile.NewProjectile(position.X, position.Y, speedX, speedY, type, damage, knockBack, player.whoAmI);
                Main.projectile[p].GetGlobalProjectile<SpiritGlobalProjectile>(mod).shotFromMarbleBow = true;
                return false;
            }
            else
            {

                int p = Projectile.NewProjectile(position.X, position.Y, speedX, speedY, type, damage, knockBack, player.whoAmI);
            }
            return false;
        }
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(null, "MarbleChunk", 16);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
        public override bool CanUseItem(Player player)
        {
            if (player.altFunctionUse == 2)
            {
                item.useTime = 60;
                item.useAnimation = 60;
                item.damage = 30;
                item.shootSpeed = 20;
                item.knockBack = 5;
                item.autoReuse = false;
            }
            else
            {
                item.useTime = 28;
                item.useAnimation = 28;
                item.shootSpeed = 6.2f;
                item.damage = 21;
                item.knockBack = 1;
                item.autoReuse = true;
            }
            return base.CanUseItem(player);
        }
    }
}