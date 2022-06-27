using SpiritMod.Items.Sets.MarbleSet;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Sets.MarbleSet.MarbleArmor
{
	[AutoloadEquip(EquipType.Body)]
	public class MarbleChest : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Gilded Robe");
			Tooltip.SetDefault("3% increased movement speed");

			ArmorIDs.Body.Sets.HidesHands[Item.bodySlot] = false;
			ArmorIDs.Body.Sets.NeedsToDrawArm[Item.bodySlot] = true;
		}

		public override void SetDefaults()
		{
			Item.width = 28;
			Item.height = 24;
			Item.value = 12100;
			Item.rare = ItemRarityID.Green;
			Item.defense = 5;
		}

		public override void UpdateEquip(Player player) => player.maxRunSpeed += 0.03f;

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ModContent.ItemType<MarbleChunk>(), 12);
			recipe.AddTile(TileID.Anvils);
			recipe.Register();
		}
	}
}
