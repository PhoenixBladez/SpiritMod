using Microsoft.Xna.Framework;
using SpiritMod.Items.Material;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Armor.GraniteArmor
{
	[AutoloadEquip(EquipType.Body)]
	public class GraniteChest : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Granite Breastplate");
			SpiritGlowmask.AddGlowMask(item.type, "SpiritMod/Items/Armor/GraniteArmor/GraniteBody_Glow");

		}

		public override void SetDefaults()
		{
			item.width = 28;
			item.height = 24;
			item.value = 1100;
			item.rare = ItemRarityID.Green;
			item.defense = 9;
		}
		public override void DrawArmorColor(Player drawPlayer, float shadow, ref Color color, ref int glowMask, ref Color glowMaskColor)
		{
			glowMaskColor = Color.White;
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ModContent.ItemType<GraniteChunk>(), 12);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}
