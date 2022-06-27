using Microsoft.Xna.Framework;
using SpiritMod.Items.Sets.SlagSet;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace SpiritMod.Items.Sets.SlagSet.FieryArmor
{
	[AutoloadEquip(EquipType.Head)]
	public class ObsidiusHelm : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Slag Tyrant's Helm");
			Tooltip.SetDefault("5% increased minion damage\nIncreases your max number of sentries");
			SpiritGlowmask.AddGlowMask(Item.type, "SpiritMod/Items/Sets/SlagSet/FieryArmor/ObsidiusHelm_Glow");
		}

		public override void SetDefaults()
		{
			Item.width = 22;
			Item.height = 20;
			Item.value = Item.sellPrice(0, 0, 35, 0);
			Item.rare = ItemRarityID.Orange;
			Item.defense = 6;
		}

		public override void DrawArmorColor(Player drawPlayer, float shadow, ref Color color, ref int glowMask, ref Color glowMaskColor)
			=> glowMaskColor = new Color(100, 100, 100, 100);

		public override void UpdateEquip(Player player)
		{
			player.maxTurrets += 1;
			player.minionDamage += .05f;
		}

		public override bool IsArmorSet(Item head, Item body, Item legs)
			=> body.type == ModContent.ItemType<ObsidiusPlate>() && legs.type == ModContent.ItemType<ObsidiusGreaves>();

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe(1);
			recipe.AddIngredient(ModContent.ItemType<CarvedRock>(), 14);
			recipe.AddTile(TileID.Anvils);
			recipe.Register();
		}

		public override void UpdateArmorSet(Player player)
		{
			string tapDir = Language.GetTextValue(Main.ReversedUpDownArmorSetBonuses ? "Key.UP" : "Key.DOWN");
			player.setBonus = $"Double tap {tapDir} to cause all sentries to release a burst of fireballs\n8 second cooldown";
			player.GetSpiritPlayer().fierySet = true;
		}
	}
}
