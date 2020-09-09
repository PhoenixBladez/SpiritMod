
using SpiritMod.Items.Material;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Accessory.Leather
{
	[AutoloadEquip(EquipType.Shield)]
	public class Strikeshield : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Strikeshield");
			Tooltip.SetDefault("Being struck by enemies may damage them slightly\nBeing struck by enemies also causes minions to target them\n3 summon tag damage to enemies that hurt the player\n4 second duration");
		}

		public override void SetDefaults()
		{
			item.width = 24;
			item.height = 28;
			item.rare = ItemRarityID.Green;
			item.defense = 1;
			item.accessory = true;
		}

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			player.GetSpiritPlayer().strikeshield = true;
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ModContent.ItemType<LeatherShield>(), 1);
            recipe.AddIngredient(ItemID.ManaCrystal, 1);
            recipe.AddIngredient(ModContent.ItemType<Items.Placeable.Tiles.BlastStoneItem>(), 10);
            recipe.AddRecipeGroup("SpiritMod:EvilMaterial1", 3);
            recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this, 1);
			recipe.AddRecipe();
		}
	}
}
