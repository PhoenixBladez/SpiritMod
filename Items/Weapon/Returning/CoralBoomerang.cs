using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Weapon.Returning
{
	public class CoralBoomerang : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Hightide Boomerang");
			Tooltip.SetDefault("Throwing speed and damage increase while the player is underwater\nHitting enemies causes the boomerang to fragment and deal extra damage");
		}


		public override void SetDefaults()
		{
			item.damage = 10;
			item.melee = true;
			item.width = 30;
			item.height = 28;
			item.useTime = 28;
			item.useAnimation = 25;
			item.noUseGraphic = true;
			item.useStyle = ItemUseStyleID.SwingThrow;
			item.knockBack = 3;
			item.value = Terraria.Item.sellPrice(0, 0, 15, 0);
			item.rare = ItemRarityID.Blue;
			item.shootSpeed = 11f;
			item.shoot = mod.ProjectileType("CoralBoomerang");
			item.UseSound = SoundID.Item1;
			item.autoReuse = false;
		}

		public override bool CanUseItem(Player player)
		{
			if(player.wet) {
				item.damage = 13;
				item.shootSpeed = 16f;
			} else {
				item.damage = 10;
				item.shootSpeed = 11f;
			}
			for(int i = 0; i < 1000; ++i) {
				if(Main.projectile[i].active && Main.projectile[i].owner == Main.myPlayer && Main.projectile[i].type == item.shoot) {
					return false;
				}
			}
			return true;
		}
		public override void AddRecipes()
		{
			ModRecipe modRecipe = new ModRecipe(base.mod);
			modRecipe.AddIngredient(ItemID.Coral, 12);
			modRecipe.AddIngredient(ItemID.Starfish, 1);
			modRecipe.AddIngredient(ItemID.BottledWater, 1);
			modRecipe.AddTile(TileID.Anvils);
			modRecipe.SetResult(this, 1);
			modRecipe.AddRecipe();
		}
	}
}