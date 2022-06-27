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
			Item.width = 20;
			Item.height = 18;
			Item.value = Item.buyPrice(gold: 4, silver: 60);
			Item.rare = ItemRarityID.LightRed;
			Item.vanity = true;
		}

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe(1);
			recipe.AddIngredient(ModContent.ItemType<Acid>(), 8);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.Register();
		}
	}
}