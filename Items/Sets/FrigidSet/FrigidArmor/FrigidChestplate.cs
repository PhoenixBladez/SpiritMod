using SpiritMod.Items.Material;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Sets.FrigidSet.FrigidArmor
{
	[AutoloadEquip(EquipType.Body)]
	public class FrigidChestplate : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Frigid Plate");
			Tooltip.SetDefault("3% increased critical strike chance");
		}


		public override void SetDefaults()
		{
			Item.width = 28;
			Item.height = 24;
			Item.value = Item.buyPrice(silver: 11);
			Item.rare = ItemRarityID.Blue;
			Item.defense = 4;
		}
		public override void UpdateEquip(Player player)
		{
			player.GetCritChance(DamageClass.Melee) += 3;
			player.GetCritChance(DamageClass.Magic) += 3;
			player.GetCritChance(DamageClass.Ranged) += 3;
		}
		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ModContent.ItemType<FrigidFragment>(), 9);
			recipe.AddTile(TileID.Anvils);
			recipe.Register();
		}
	}
}
