using SpiritMod.Items.Material;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Sets.BloodcourtSet.BloodCourt
{
	[AutoloadEquip(EquipType.Body)]
	public class BloodCourtChestplate : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Bloodcourt's Vestments");
			Tooltip.SetDefault("6% increased damage\n12% increased melee speed\n20% chance to not consume ammo");

		}
		public override void SetDefaults()
		{
			Item.width = 30;
			Item.height = 20;
			Item.value = 6000;
			Item.rare = ItemRarityID.Green;
			Item.defense = 5;
		}
		public override void UpdateEquip(Player player)
		{
			player.allDamage += 0.06f;
			player.meleeSpeed += 0.12f;
			player.ammoCost80 = true;
		}
		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe(1);
			recipe.AddIngredient(ModContent.ItemType<DreamstrideEssence>(), 10);
			recipe.AddTile(TileID.Anvils);
			recipe.Register();
		}
	}
}
