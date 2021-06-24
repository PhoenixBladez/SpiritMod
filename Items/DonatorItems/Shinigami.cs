using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using SpiritMod.Utilities;
namespace SpiritMod.Items.DonatorItems
{
	class Shinigami : ModItem
	{

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Shinigami");
			Tooltip.SetDefault("Right click to dash through enemies ");
		}

		public override void SetDefaults()
		{
			item.width = 54;
			item.height = 66;
			item.useStyle = ItemUseStyleID.SwingThrow;
			item.UseSound = SoundID.Item1;

			item.value = Item.sellPrice(0, 7, 50, 0);
			item.rare = ItemRarityID.Purple;

			item.damage = 180;
			item.knockBack = 3f;
			item.melee = true;
			item.autoReuse = true;

			item.useTime = 18;
			item.useAnimation = 18;
		}

		public override bool AltFunctionUse(Player player)
		{
			return true;
		}
		public override void AddRecipes()
		{
			var recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.LunarBar, 12);
			recipe.AddTile(TileID.LunarCraftingStation);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
		public override bool CanUseItem(Player player)
		{
			if (player.altFunctionUse == 2) {
				if (player.dashDelay == 0) {
					item.useStyle = ItemUseStyleID.Stabbing;
					item.noMelee = true;
					player.GetModPlayer<MyPlayer>().PerformDash(
						DashType.Shinigami,
						(sbyte)player.direction);
				}
				else
					return false;
			}
			else {
				item.useStyle = ItemUseStyleID.SwingThrow;
				item.noMelee = false;
			}
			return true;
		}
	}
}
