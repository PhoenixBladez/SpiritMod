using SpiritMod.Items.Material;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Sets.CascadeSet.Armor
{
	[AutoloadEquip(EquipType.Legs)]
	public class CascadeLeggings : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Cascade Greaves");
			Tooltip.SetDefault("8% increased melee speed");
		}

		public override void SetDefaults()
		{
			item.width = 38;
			item.height = 26;
			item.value = Terraria.Item.sellPrice(0, 0, 80, 0);
			item.rare = ItemRarityID.Orange;
			item.defense = 3;
		}

		public override void UpdateEquip(Player player)
		{
            player.meleeSpeed += .08f;
		}
		public override void AddRecipes()
		{
			var recipe = new ModRecipe(mod);
			recipe.AddIngredient(ModContent.ItemType<DeepCascadeShard>(), 18);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}
