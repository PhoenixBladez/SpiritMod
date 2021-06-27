using SpiritMod.Items.Material;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace SpiritMod.Items.Sets.BloodcourtSet.BloodCourt
{
	[AutoloadEquip(EquipType.Head)]
	public class BloodCourtHead : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Bloodcourt's Visage");
			Tooltip.SetDefault("4% increased damage\nIncreases your max number of minions");

		}

		public override void SetDefaults()
		{
			item.width = 22;
			item.height = 20;
			item.value = 3000;
			item.rare = ItemRarityID.Green;
			item.defense = 1;
		}
        public override void UpdateEquip(Player player)
		{
			player.allDamage += 0.04f;
			player.maxMinions += 1;
		}

		public override void UpdateVanity(Player player, EquipType type) => player.GetSpiritPlayer().bloodCourtHead = true;

		public override void DrawHair(ref bool drawHair, ref bool drawAltHair)
			=> drawHair = true;

		public override void UpdateArmorSet(Player player)
		{
			string tapDir = Language.GetTextValue(Main.ReversedUpDownArmorSetBonuses ? "Key.UP" : "Key.DOWN");
			player.GetSpiritPlayer().bloodcourtSet = true;
			player.setBonus = $"Double tap {tapDir} to sacrifice 8% of your maximum health\n" +
							   "and launch a bolt of Dark Anima dealing high damage in a radius\n" +
							   "This bolt siphons 10 additional health over 5 seconds";
		}

		public override void ArmorSetShadows(Player player)
			=> player.armorEffectDrawShadow = true;

		public override bool IsArmorSet(Item head, Item body, Item legs)
			=> body.type == ModContent.ItemType<BloodCourtChestplate>() && legs.type == ModContent.ItemType<BloodCourtLeggings>();

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ModContent.ItemType<BloodFire>(), 6);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this, 1);
			recipe.AddRecipe();
		}
	}
}
