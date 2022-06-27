using Microsoft.Xna.Framework;
using SpiritMod.Items.Sets.SeraphSet;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Sets.SeraphSet.SeraphArmor
{
	[AutoloadEquip(EquipType.Legs)]
	public class SeraphLegs : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Seraph's Greaves");
			Tooltip.SetDefault("Increases maximum mana by 50");
			SpiritGlowmask.AddGlowMask(Item.type, "SpiritMod/Items/Sets/SeraphSet/SeraphArmor/SeraphLegs_Glow");
		}
		public override void SetDefaults()
		{
			Item.width = 22;
			Item.height = 16;
			Item.value = Terraria.Item.sellPrice(0, 1, 0, 0);
			Item.rare = ItemRarityID.LightRed;
			Item.defense = 12;
		}
		public override void UpdateEquip(Player player)
		{
			player.statManaMax2 += 50;

		}
		public override void DrawArmorColor(Player drawPlayer, float shadow, ref Color color, ref int glowMask, ref Color glowMaskColor)
		{
			glowMaskColor = Color.White;
		}
		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe(1);
			recipe.AddIngredient(ModContent.ItemType<MoonStone>(), 10);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.Register();
		}
	}
}
