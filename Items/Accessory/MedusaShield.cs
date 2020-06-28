
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Accessory
{
	[AutoloadEquip(EquipType.Shield)]
	public class MedusaShield : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Medusa Shield");
			Tooltip.SetDefault("Provides immunity to knockback and the stoned debuff\nAs your health goes down, your life regeneration increases");
		}
		public override void SetDefaults()
		{
			item.width = 28;
			item.height = 36;
			item.rare = ItemRarityID.Pink;
			item.value = 100000;
			item.accessory = true;
			item.defense = 6;
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ModContent.ItemType<GoldShield>(), 1);
			recipe.AddIngredient(ItemID.PocketMirror, 1);
			recipe.AddTile(TileID.TinkerersWorkbench);
			recipe.SetResult(this, 1);
			recipe.AddRecipe();
		}
		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			player.noKnockback = true;
			float defBoost = (float)(player.statLifeMax2 - player.statLife) / (float)player.statLifeMax2 * 20f;
			player.statDefense += (int)defBoost;
			player.buffImmune[BuffID.Stoned] = true;
		}
	}
}
