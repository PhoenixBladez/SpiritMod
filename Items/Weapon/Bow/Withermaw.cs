using Terraria;
using System;
using Terraria.ID;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Terraria.ModLoader;
using SpiritMod.Projectiles;

namespace SpiritMod.Items.Weapon.Bow
{
    public class Withermaw : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Withermaw");
			Tooltip.SetDefault("Shoots out powerful Nightmare Shards along with arrows!");
		}



        public override void SetDefaults()
        {
            item.damage = 59;
            item.noMelee = true;
            item.ranged = true;
            item.width = 20;
            item.height = 38;
            item.useTime = 14;
            item.useAnimation = 14;
            item.useStyle = 5;
            item.shoot = 3;
            item.useAmmo = AmmoID.Arrow;
            item.knockBack = 5;
            item.value = Terraria.Item.sellPrice(0, 3, 0, 0);
            item.rare = 8;
            item.UseSound = SoundID.Item5;
            item.autoReuse = true;
            item.shootSpeed = 13f;
            item.crit = 7;

        }
        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
			if (Main.rand.Next(3) == 2)
			Projectile.NewProjectile(position.X, position.Y, speedX * 2f, speedY * 2f, mod.ProjectileType("BloodTear"), damage * 3 / 2, knockBack, player.whoAmI, 0f, 0f);
            return true; 
        }
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(null, "NightmareFuel", 10);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}