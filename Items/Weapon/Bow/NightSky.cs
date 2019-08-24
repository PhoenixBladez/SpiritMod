using Terraria;
using System;
using Terraria.ID;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Terraria.ModLoader;

namespace SpiritMod.Items.Weapon.Bow
{
    public class NightSky : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Night Sky");
			Tooltip.SetDefault("Occasionally shoots out powerful stars!");
		}


        int charger;
        public override void SetDefaults()
        {
            item.damage = 39;
            item.noMelee = true;
            item.ranged = true;
            item.width = 24;
            item.height = 46;
            item.useTime = 29;
            item.useAnimation = 29;
            item.useStyle = 5;
            item.shoot = 3;
            item.useAmmo = AmmoID.Arrow;
            item.knockBack = 4;
            item.value = Item.buyPrice(0, 4, 0, 0);
            item.rare = 4;
            item.UseSound = SoundID.Item5;
            item.autoReuse = true;
            item.shootSpeed = 12.8f;
            item.crit = 6;
        }
        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            charger++;
            if (charger >= 4)
            {
                for (int I = 0; I < 4; I++)
                {
                    Projectile.NewProjectile(position.X - 8, position.Y + 8, speedX + ((float)Main.rand.Next(-230, 230) / 100), speedY + ((float)Main.rand.Next(-230, 230) / 100), ProjectileID.FallingStar, damage, knockBack, player.whoAmI, 0f, 0f);
                }
                charger = 0;
            }
            return true;
        }
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(null, "StarlightBow", 1);
            recipe.AddIngredient(null, "BloomBuster", 1);
            recipe.AddIngredient(null, "Longbowne", 1);
            recipe.AddIngredient(ItemID.HellwingBow, 1);
            recipe.AddTile(TileID.DemonAltar);
            recipe.SetResult(this, 1);
            recipe.AddRecipe();
        }
    }
}