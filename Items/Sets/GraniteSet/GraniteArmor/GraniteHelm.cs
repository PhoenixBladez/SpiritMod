using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using SpiritMod.Items.Sets.GraniteSet;

namespace SpiritMod.Items.Sets.GraniteSet.GraniteArmor
{
	[AutoloadEquip(EquipType.Head)]
	public class GraniteHelm : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Granite Visor");
			SpiritGlowmask.AddGlowMask(item.type, "SpiritMod/Items/Sets/GraniteSet/GraniteArmor/GraniteHelm_Glow");
			Tooltip.SetDefault("Increases jump height slightly");

		}

		public override void SetDefaults()
		{
			item.width = 28;
			item.height = 24;
			item.value = 1100;
			item.rare = ItemRarityID.Green;
			item.defense = 6;
		}

		public override void DrawArmorColor(Player drawPlayer, float shadow, ref Color color, ref int glowMask, ref Color glowMaskColor) => glowMaskColor = Color.White;
		public override void UpdateEquip(Player player) => player.jumpSpeedBoost += 0.5f;
		public override bool IsArmorSet(Item head, Item body, Item legs) => body.type == ModContent.ItemType<GraniteChest>() && legs.type == ModContent.ItemType<GraniteLegs>();

		public override void UpdateArmorSet(Player player)
		{
			string tapDir = Language.GetTextValue(Main.ReversedUpDownArmorSetBonuses ? "Key.UP" : "Key.DOWN");
			player.setBonus = $"Double tap {tapDir} while falling to stomp downward\nHitting the ground releases a shockwave that scales with height\n4 second cooldown";
			player.GetSpiritPlayer().graniteSet = true;
		}

		public override void ArmorSetShadows(Player player) => player.armorEffectDrawShadow = true;

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ModContent.ItemType<GraniteChunk>(), 14);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}
