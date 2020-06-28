using Microsoft.Xna.Framework;
using SpiritMod.Items.Material;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Armor.SeraphArmor
{
	[AutoloadEquip(EquipType.Body)]
	public class SeraphArmor : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Seraph's Breastplate");
			Tooltip.SetDefault("10% increased magic damage");
			SpiritGlowmask.AddGlowMask(item.type, "SpiritMod/Items/Armor/SeraphArmor/SeraphBody_Glow");
		}

		public override void SetDefaults()
		{
			item.width = 34;
			item.height = 24;
			item.value = Item.buyPrice(gold: 6);
			item.rare = 4;
			item.defense = 18;
		}

		public override void UpdateEquip(Player player)
		{
			player.magicDamage += .10f;
		}
		public override void DrawArmorColor(Player drawPlayer, float shadow, ref Color color, ref int glowMask, ref Color glowMaskColor)
		{
			glowMaskColor = Color.White;
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ModContent.ItemType<MoonStone>(), 13);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.SetResult(this, 1);
			recipe.AddRecipe();
		}
	}
}
