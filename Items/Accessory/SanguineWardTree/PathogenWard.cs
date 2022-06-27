
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Accessory.SanguineWardTree
{
	public class PathogenWard : ModItem
	{
		public override bool IsLoadingEnabled(Mod mod) => false;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Pathogen Ward");
			Tooltip.SetDefault("A bloody ward surrounds you, inflicting Blood Corruption to nearby enemies\nKilling enemies within the aura restores some life\nHearts are more likely to drop from enemies\nProvides immunity to the 'Poisoned' buff");

		}


		public override void SetDefaults()
		{
			Item.width = 18;
			Item.height = 18;
			Item.value = Item.buyPrice(0, 1, 50, 0);
			Item.rare = ItemRarityID.LightRed;
			Item.accessory = true;
		}
		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			player.GetSpiritPlayer().vitaStone = true;
			player.buffImmune[BuffID.Poisoned] = true;
		}
		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ModContent.ItemType<Bloodstone>(), 1);
			recipe.AddIngredient(ItemID.Bezoar, 1);
			recipe.AddTile(TileID.TinkerersWorkbench);
			recipe.Register();
		}
	}
}
