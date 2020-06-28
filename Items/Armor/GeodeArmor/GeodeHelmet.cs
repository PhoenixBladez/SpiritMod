
using SpiritMod.Items.Material;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Armor.GeodeArmor
{
	[AutoloadEquip(EquipType.Head)]
	public class GeodeHelmet : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Geode Helmet");
		}
		public override void SetDefaults()
		{
			item.width = 40;
			item.height = 30;
			item.value = Terraria.Item.sellPrice(0, 0, 75, 0);
			item.rare = ItemRarityID.LightRed;

			item.vanity = true;
		}

		public override void UpdateEquip(Player player)
		{
			player.moveSpeed += 0.15f;
			player.maxRunSpeed += 1;

		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ModContent.ItemType<Geode>(), 6);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}
