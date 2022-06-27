using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using SpiritMod.Tiles.Block;
using System.Collections.Generic;

namespace SpiritMod.Items.Sets.FloranSet
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
			Item.width = 24;
			Item.height = 24;
			Item.value = Item.buyPrice(silver: 20);
			Item.rare = ItemRarityID.Blue;
			Item.defense = 2;
			Item.accessory = true;
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
				player.GetAttackSpeed(DamageClass.Melee) += meleeSpeed;
				player.lifeRegen += lifeRegen;
			}
		}

		public override void TileFloorVisuals(int type, Player player)
		{
			standingOnGrass = type == TileID.Grass
				|| type == TileID.JungleGrass
				|| type == TileID.MushroomGrass
				|| type == TileID.HallowedGrass
				|| type == TileID.CrimsonGrass
				|| type == TileID.CorruptGrass
				|| type == ModContent.TileType<HalloweenGrass>()
				|| type == ModContent.TileType<ReachGrassTile>()
				|| type == ModContent.TileType<SpiritGrass>();
			if (standingOnGrass) grassTimer = 2;
		}
	}
}
