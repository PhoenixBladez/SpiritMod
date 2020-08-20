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

		}

		public override void SetDefaults()
		{
			item.width = 32;
			item.height = 30;
			item.value = 12500;
			item.rare = ItemRarityID.Blue;
			item.defense = 2;
		}
		public override void DrawHands(ref bool drawHands, ref bool drawArms)
		{
			drawHands = true;
		}
		public override void UpdateEquip(Player player)
		{
            player.GetSpiritPlayer().silkenRobe = true;
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.Silk, 5);
			recipe.AddRecipeGroup("SpiritMod:GoldBars", 2);
			recipe.AddIngredient(ItemID.FallenStar, 1);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this, 1);
			recipe.AddRecipe();
		}
	}
}