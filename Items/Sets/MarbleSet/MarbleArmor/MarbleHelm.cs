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
			Tooltip.SetDefault("3% increased movement speed");

		}
		public override void SetDefaults()
		{
			Item.width = 28;
			Item.height = 24;
			Item.value = 1100;
			Item.rare = ItemRarityID.Green;
			Item.defense = 6;
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
		public override void UpdateEquip(Player player)
		{
			player.maxRunSpeed += 0.03f;
		}
		public override void ArmorSetShadows(Player player)
		{
			player.armorEffectDrawShadow = true;
		}
		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ModContent.ItemType<MarbleChunk>(), 14);
			recipe.AddTile(TileID.Anvils);
			recipe.Register();
		}
	}
}
