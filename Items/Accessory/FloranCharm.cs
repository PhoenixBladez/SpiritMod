
using SpiritMod.Items.Material;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using SpiritMod.Tiles.Block;
using System.Collections.Generic;

namespace SpiritMod.Items.Accessory
{
	[AutoloadEquip(EquipType.Neck)]
	public class FloranCharm : SpiritAccessory
	{
		public override string SetDisplayName => "Floran Hunting Charm";
		public override string SetTooltip => "5% increased melee speed and slightly increases life regeneration while standing on grass";
		public override List<SpiritPlayerEffect> AccessoryEffects => new List<SpiritPlayerEffect>() {
			new FloranCharmEffect()
		};

		public override void SetDefaults()
		{
			item.width = 24;
			item.height = 24;
			item.value = Item.buyPrice(silver: 20);
			item.rare = ItemRarityID.Blue;
			item.defense = 2;
			item.accessory = true;
		}

		public override void AddRecipes()
		{
			ModRecipe modRecipe = new ModRecipe(mod);
			modRecipe.AddIngredient(ModContent.ItemType<FloranBar>(), 9);
			modRecipe.AddIngredient(ModContent.ItemType<EnchantedLeaf>(), 3);
			modRecipe.AddTile(TileID.Anvils);
			modRecipe.SetResult(this);
			modRecipe.AddRecipe();
		}
	}

	public class FloranCharmEffect : SpiritPlayerEffect
	{
		public bool standingOnGrass = false;
		public float meleeSpeed = 0.05f;
		public int lifeRegen = 3;
		private int grassTimer = 0;

		public override void ItemUpdateAccessory(Player player, bool hideVisual)
		{
			grassTimer--;
			if (grassTimer < 0) standingOnGrass = false;
			if (standingOnGrass) {
				player.meleeSpeed += meleeSpeed;
				player.lifeRegen += lifeRegen;
			}
		}

		public override void TileFloorVisuals(int type, Player player)
		{
			standingOnGrass = type == TileID.Grass
				|| type == TileID.JungleGrass
				|| type == TileID.MushroomGrass
				|| type == TileID.HallowedGrass
				|| type == TileID.FleshGrass
				|| type == TileID.CorruptGrass
				|| type == ModContent.TileType<HalloweenGrass>()
				|| type == ModContent.TileType<ReachGrassTile>()
				|| type == ModContent.TileType<SpiritGrass>();
			if (standingOnGrass) grassTimer = 2;
		}
	}
}
