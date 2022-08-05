using SpiritMod.Items.Material;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Sets.CryoliteSet.CryoliteArmor
{
	[AutoloadEquip(EquipType.Legs)]
	public class CryoliteLegs : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Cryolite Greaves");
			Tooltip.SetDefault("8% increased melee speed");
		}

		public override void SetDefaults()
		{
			Item.width = 38;
			Item.height = 26;
			Item.value = Item.sellPrice(0, 0, 80, 0);
			Item.rare = ItemRarityID.Orange;
			Item.defense = 10;
		}

		public override void UpdateEquip(Player player)
		{
            player.GetAttackSpeed(DamageClass.Melee) += .08f;
		}
		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ModContent.ItemType<CryoliteBar>(), 18);
			recipe.AddTile(TileID.Anvils);
			recipe.Register();
		}
	}
}
