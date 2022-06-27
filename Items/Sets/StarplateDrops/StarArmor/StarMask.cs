using Microsoft.Xna.Framework;
using SpiritMod.Items.Material;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace SpiritMod.Items.Sets.StarplateDrops.StarArmor
{
	[AutoloadEquip(EquipType.Head)]
	public class StarMask : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Astralite Visor");
			Tooltip.SetDefault("6% increased ranged critical strike chance");
			SpiritGlowmask.AddGlowMask(Item.type, "SpiritMod/Items/Sets/StarplateDrops/StarArmor/StarMask_Glow");
		}

		public override void DrawArmorColor(Player drawPlayer, float shadow, ref Color color, ref int glowMask, ref Color glowMaskColor)
			=> glowMaskColor = Color.White;

		public override void SetDefaults()
		{
			Item.width = 22;
			Item.height = 20;
			Item.value = Item.sellPrice(0, 0, 30, 0);
			Item.rare = ItemRarityID.Orange;
			Item.defense = 6;
		}

		public override void UpdateEquip(Player player)
		{
			player.GetCritChance(DamageClass.Ranged) +=(6);
		}

		public override void UpdateArmorSet(Player player)
		{
			string tapDir = Language.GetTextValue(Main.ReversedUpDownArmorSetBonuses ? "Key.UP" : "Key.DOWN");
			player.setBonus = $"Double tap {tapDir} to deploy an energy field at the cursor position\nThis field lasts for five seconds and supercharges all ranged projectiles that pass through it\n12 second cooldown";
			player.GetSpiritPlayer().starSet = true;
            player.endurance += 0.05f;
        }

		public override bool IsArmorSet(Item head, Item body, Item legs)
			=> body.type == ModContent.ItemType<StarPlate>() && legs.type == ModContent.ItemType<StarLegs>();

		public override void ArmorSetShadows(Player player)
			=> player.armorEffectDrawShadow = true;

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ModContent.ItemType<CosmiliteShard>(), 12);
			recipe.AddTile(TileID.Anvils);
			recipe.Register();
		}
	}
}
