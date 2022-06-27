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
			Item.width = 54;
			Item.height = 66;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.UseSound = SoundID.Item1;

			Item.value = Item.sellPrice(0, 7, 50, 0);
			Item.rare = ItemRarityID.Purple;

			Item.damage = 180;
			Item.knockBack = 3f;
			Item.DamageType = DamageClass.Melee;
			Item.autoReuse = true;

			Item.useTime = 18;
			Item.useAnimation = 18;
		}

		public override bool AltFunctionUse(Player player)
		{
			return true;
		}
		public override void AddRecipes()
		{
			var recipe = CreateRecipe();
			recipe.AddIngredient(ItemID.LunarBar, 12);
			recipe.AddTile(TileID.LunarCraftingStation);
			recipe.Register();
		}
		public override bool CanUseItem(Player player)
		{
			if (player.altFunctionUse == 2) {
				if (player.dashDelay == 0) {
					Item.useStyle = ItemUseStyleID.Thrust;
					Item.noMelee = true;
					player.GetModPlayer<MyPlayer>().PerformDash(
						DashType.Shinigami,
						(sbyte)player.direction);
				}
				else
					return false;
			}
			else {
				Item.useStyle = ItemUseStyleID.Swing;
				Item.noMelee = false;
			}
			return true;
		}
	}
}
