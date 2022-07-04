using SpiritMod.Items.Material;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Armor.LeatherArmor
{
	[AutoloadEquip(EquipType.Body)]
	public class LeatherPlate : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Marksman's Plate");

			ArmorIDs.Body.Sets.NeedsToDrawArm[Item.bodySlot] = true;
		}

		public override void SetDefaults()
		{
			Item.width = 30;
			Item.height = 18;
			Item.value = 4000;
			Item.rare = ItemRarityID.Blue;
			Item.defense = 2;
		}
		
		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ModContent.ItemType<OldLeather>(), 8);
			recipe.AddTile(TileID.WorkBenches);
			recipe.Register();
		}
	}
}
