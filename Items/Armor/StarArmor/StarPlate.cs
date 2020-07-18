using Microsoft.Xna.Framework;
using SpiritMod.Items.Material;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Armor.StarArmor
{
	[AutoloadEquip(EquipType.Body)]
	public class StarPlate : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Astralite Chestguard");
			Tooltip.SetDefault("Increases ranged damage by 4%");
			SpiritGlowmask.AddGlowMask(item.type, "SpiritMod/Items/Armor/StarArmor/StarPlate_Glow");
		}
		public override void DrawArmorColor(Player drawPlayer, float shadow, ref Color color, ref int glowMask, ref Color glowMaskColor)
		{
			glowMaskColor = Color.White;
		}
		int timer = 0;

		public override void SetDefaults()
		{
			item.width = 22;
			item.height = 20;
			item.value = Terraria.Item.sellPrice(0, 0, 38, 0);
			item.rare = ItemRarityID.Orange;
			item.defense = 9;
		}
		public override void UpdateEquip(Player player)
        {
            player.rangedDamage += .04f;
		}
		public override void ArmorSetShadows(Player player)
		{
			player.armorEffectDrawShadow = true;
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ModContent.ItemType<CosmiliteShard>(), 18);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}
