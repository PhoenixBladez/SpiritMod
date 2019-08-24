using Terraria;
using System;
using Terraria.ID;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Terraria.ModLoader;

namespace SpiritMod.Items.Weapon.Bow
{
	public class Revenant : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Revenant");
			Tooltip.SetDefault("Converts arrows into Soul Burning Revenant Arrows!");
		}


		public override void SetDefaults()
		{
			item.width = 12;
			item.height = 28;
			item.value = Item.sellPrice(0, 1, 0, 0);
			item.rare = 5;
			item.damage = 44;
			item.knockBack = 1f;
			item.useStyle = 5;
			item.useTime = 22;
			item.useAnimation = 22;
			item.useAmmo = AmmoID.Arrow;
			item.ranged = true;
			item.noMelee = true;
			item.autoReuse = true;
			item.shoot = mod.ProjectileType("SpiritArrow");
			item.shootSpeed = 10f;
			item.UseSound = SoundID.Item5;
		}
        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            Terraria.Projectile.NewProjectile(position.X, position.Y, speedX, speedY, mod.ProjectileType("SpiritArrow"), damage, knockBack, player.whoAmI, 0f, 0f);
            return false;
        }
        public override void AddRecipes()
		{
			ModRecipe modRecipe = new ModRecipe(mod);
			modRecipe.AddIngredient(null, "SpiritBar", 14);
			modRecipe.AddTile(134);
			modRecipe.SetResult(this, 1);
			modRecipe.AddRecipe();
		}
	}
}
