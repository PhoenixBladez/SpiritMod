using SpiritMod.Items.Material;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Armor.MagalaArmor
{
	[AutoloadEquip(EquipType.Head)]
	public class MagalaHelm : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Gore Magala Veil");

		}


		int timer = 0;

		public override void SetDefaults()
		{
			item.width = 22;
			item.height = 20;
			item.value = 3000;
			item.rare = 4;
			item.vanity = true;
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ModContent.ItemType<MagalaScale>(), 5);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this, 1);
			recipe.AddRecipe();
		}
		public override void ArmorSetShadows(Player player)
		{
			player.armorEffectDrawShadow = true;
		}
	}
}
