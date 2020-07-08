using Microsoft.Xna.Framework;
using SpiritMod.Items.Material;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace SpiritMod.Items.Armor.FieryArmor
{
	[AutoloadEquip(EquipType.Head)]
	public class ObsidiusHelm : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Slag Tyrant's Helm");
			Tooltip.SetDefault("5% increased minion damage\nIncreases your max number of sentries");
			SpiritGlowmask.AddGlowMask(item.type, "SpiritMod/Items/Armor/FieryArmor/ObsidiusHelm_Glow");
		}

		public override void SetDefaults()
		{
			item.width = 22;
			item.height = 20;
			item.value = Item.sellPrice(0, 0, 35, 0);
			item.rare = ItemRarityID.Orange;
			item.defense = 6;
		}

		public override void DrawArmorColor(Player drawPlayer, float shadow, ref Color color, ref int glowMask, ref Color glowMaskColor) 
			=> glowMaskColor = Color.White;

		public override void UpdateEquip(Player player)
		{
			player.maxTurrets += 1;
			player.minionDamage += .05f;
		}

		public override bool IsArmorSet(Item head, Item body, Item legs) 
			=> body.type == ModContent.ItemType<ObsidiusPlate>() && legs.type == ModContent.ItemType<ObsidiusGreaves>();

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ModContent.ItemType<CarvedRock>(), 14);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this, 1);
			recipe.AddRecipe();
		}

		public override void UpdateArmorSet(Player player)
		{
			string tapDir = Language.GetTextValue(Main.ReversedUpDownArmorSetBonuses ? "Key.UP" : "Key.DOWN");
			player.setBonus = $"Double tap {tapDir} to cause all sentries to release a burst of fireballs\n8 second cooldown";
			player.GetSpiritPlayer().fierySet = true;
		}
	}
}
