
using SpiritMod.Items.Sets.FrigidSet;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Accessory.Leather
{
	[AutoloadEquip(EquipType.HandsOn)]
	public class FrigidGloves : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Frigid Wraps");
			Tooltip.SetDefault("Melee critical strikes may inflict Frostburn\n4% increased attack speed for every nearby enemy\nThis effect stacks up to six times");
		}
		public override void SetDefaults()
		{
			item.width = 26;
			item.height = 34;
			item.rare = ItemRarityID.Blue;
			item.value = 1200;
			item.accessory = true;
		}

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			player.GetSpiritPlayer().frigidGloves = true;
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ModContent.ItemType<LeatherGlove>(), 1);
			recipe.AddIngredient(ModContent.ItemType<FrigidFragment>(), 6);
			recipe.AddRecipeGroup("SpiritMod:PHMEvilMaterial", 4);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this, 1);
			recipe.AddRecipe();
		}
	}
}
