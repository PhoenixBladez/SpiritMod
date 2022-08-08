using SpiritMod.Items.Material;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace SpiritMod.Items.Sets.FrigidSet.FrigidArmor
{
	[AutoloadEquip(EquipType.Head)]
	public class FrigidHelm : ModItem
	{
		public override void SetStaticDefaults() => DisplayName.SetDefault("Frigid Faceplate");

		public override void SetDefaults()
		{
			Item.width = 28;
			Item.height = 24;
			Item.value = 1100;
			Item.rare = ItemRarityID.Blue;
			Item.defense = 3;
		}

		public override bool IsArmorSet(Item head, Item body, Item legs) => body.type == ModContent.ItemType<FrigidChestplate>() && legs.type == ModContent.ItemType<FrigidLegs>();

		public override void UpdateArmorSet(Player player)
		{
			string tapDir = Language.GetTextValue(Main.ReversedUpDownArmorSetBonuses ? "Key.UP" : "Key.DOWN");
			player.setBonus = $"Double tap {tapDir} to create an icy wall at the cursor position\n8 second cooldown";
			player.GetSpiritPlayer().frigidSet = true;

			if (Main.rand.NextBool(6))
			{
				int dust = Dust.NewDust(player.position, player.width, player.height, DustID.Flare_Blue);
				Main.dust[dust].noGravity = true;
			}
		}

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ModContent.ItemType<FrigidFragment>(), 8);
			recipe.AddTile(TileID.Anvils);
			recipe.Register();
		}
	}
}
