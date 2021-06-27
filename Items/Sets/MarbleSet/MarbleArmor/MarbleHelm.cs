using SpiritMod.Items.Sets.MarbleSet;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Sets.MarbleSet.MarbleArmor
{
	[AutoloadEquip(EquipType.Head)]
	public class MarbleHelm : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Gilded Laurel");
			Tooltip.SetDefault("");

		}
		public override void SetDefaults()
		{
			item.width = 28;
			item.height = 24;
			item.value = 1100;
			item.rare = ItemRarityID.Green;
			item.defense = 6;
		}
		public override void DrawHair(ref bool drawHair, ref bool drawAltHair)
		{
			drawHair = true;
		}

		public override bool IsArmorSet(Item head, Item body, Item legs)
		{
			return body.type == ModContent.ItemType<MarbleChest>() && legs.type == ModContent.ItemType<MarbleLegs>();
		}
		public override void UpdateArmorSet(Player player)
		{
			player.setBonus = "Press 'Up' to grant 'Divine Winds', allowing for limited flight\n5 second cooldown";
			player.GetSpiritPlayer().marbleSet = true;
		}
		public override void ArmorSetShadows(Player player)
		{
			player.armorEffectDrawShadow = true;
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ModContent.ItemType<MarbleChunk>(), 14);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}
