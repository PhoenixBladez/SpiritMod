using Microsoft.Xna.Framework;
using SpiritMod.Items.Material;
using SpiritMod.Projectiles;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Armor.AcidArmor
{
	[AutoloadEquip(EquipType.Head)]
	public class AcidMask : SpiritItem
	{
		public override string SetDisplayName => "Acid Mask";
		
		public override void SetDefaults()
		{
			item.width = 20;
			item.height = 18;
			item.value = Item.buyPrice(gold: 4, silver: 60);
			item.rare = ItemRarityID.LightRed;
			item.vanity = true;
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ModContent.ItemType<Acid>(), 8);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.SetResult(this, 1);
			recipe.AddRecipe();
		}
	}
}