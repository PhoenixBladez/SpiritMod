using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Armor.SilkArmor
{
	[AutoloadEquip(EquipType.Body)]
	public class SilkRobe : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Manasilk Robe");
			Tooltip.SetDefault("Increases minion damage by 1");

			ArmorIDs.Body.Sets.NeedsToDrawArm[Item.bodySlot] = true;
		}

		public override void SetDefaults()
		{
			Item.width = 32;
			Item.height = 30;
			Item.value = 12500;
			Item.rare = ItemRarityID.Blue;
			Item.defense = 2;
		}

		public override void UpdateEquip(Player player)
		{
            player.GetSpiritPlayer().silkenRobe = true;
		}
		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe(1);
			recipe.AddIngredient(ItemID.Silk, 5);
			recipe.AddRecipeGroup("SpiritMod:GoldBars", 2);
			recipe.AddIngredient(ItemID.FallenStar, 1);
			recipe.AddTile(TileID.Anvils);
			recipe.Register();
		}
	}
}