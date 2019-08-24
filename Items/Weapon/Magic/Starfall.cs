using Terraria;
using System;
using Microsoft.Xna.Framework;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Weapon.Magic
{
	public class Starfall : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Starfall");
			Tooltip.SetDefault("Shoots multiple starry bolts from the sky");
		}


		public override void SetDefaults()
		{
			item.damage = 42;
			item.magic = true;
			item.mana = 9;
			item.width = 40;
			item.height = 40;
			item.useTime = 21;
			item.useAnimation = 21;
			item.useStyle = 5;
			Item.staff[item.type] = true; //this makes the useStyle animate as a staff instead of as a gun
			item.noMelee = true; //so the item's animation doesn't do damage
			item.knockBack = 5;
            item.useTurn = false;
            item.value = Terraria.Item.sellPrice(0, 5, 0, 0);
            item.rare = 5;
			item.UseSound = SoundID.Item20;
			item.autoReuse = true;
			item.shoot = mod.ProjectileType("StarfallProjectile");
			item.shootSpeed = 14f;
		}
		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
			if(Main.myPlayer == player.whoAmI) {
				Vector2 mouse = Main.MouseWorld;
				 for (int i = 0; i < 3; ++i)
            {
				Projectile.NewProjectile(mouse.X + Main.rand.Next(-80, 80), player.Center.Y - 550 + Main.rand.Next(-50, 50), 0, Main.rand.Next(13,15), type, damage, knockBack, player.whoAmI);
			}
			}
			return false;
        }
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(null, "StellarBar", 10);
            recipe.AddIngredient(531, 1);
            recipe.AddTile(TileID.Bookcases);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}