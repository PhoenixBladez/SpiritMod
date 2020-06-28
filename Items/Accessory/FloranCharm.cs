
using SpiritMod.Items.Material;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Accessory
{
	[AutoloadEquip(EquipType.Neck)]
	public class FloranCharm : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Floran Hunting Charm");
			Tooltip.SetDefault("5% increased melee speed and slightly increases life regeneration while standing on grass");
		}


		public override void SetDefaults()
		{
			item.width = 24;
			item.height = 24;
			item.value = Item.buyPrice(0, 0, 20, 0);
			item.rare = ItemRarityID.Green;
			item.defense = 2;

			item.accessory = true;
		}

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			player.GetSpiritPlayer().floranCharm = true;
		}
		public override void AddRecipes()
		{
			ModRecipe modRecipe = new ModRecipe(mod);
			modRecipe.AddIngredient(ModContent.ItemType<FloranBar>(), 9);
			modRecipe.AddTile(TileID.Anvils);
			modRecipe.SetResult(this);
			modRecipe.AddRecipe();
		}
	}
}
