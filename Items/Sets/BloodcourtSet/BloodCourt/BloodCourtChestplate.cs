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
			item.width = 30;
			item.height = 20;
			item.value = 6000;
			item.rare = ItemRarityID.Green;
			item.defense = 3;
		}
		public override void UpdateEquip(Player player)
		{
			player.allDamage += 0.06f;
			player.meleeSpeed += 0.12f;
			player.ammoCost80 = true;
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ModContent.ItemType<BloodFire>(), 10);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this, 1);
			recipe.AddRecipe();
		}
	}
}
