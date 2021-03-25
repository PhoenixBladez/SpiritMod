
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Accessory
{
	[AutoloadEquip(EquipType.Neck)]
	public class ReachBrooch : SpiritAccessory
	{
		public override string SetDisplayName => "Forsworn Pendant";
		public override string SetTooltip => "Generates a subtle glow around the player\nAllows for increased night vision in the Briar";
		public override List<SpiritPlayerEffect> AccessoryEffects => new List<SpiritPlayerEffect>() {
			new ReachBroochEffect()
		};

		public override void SetDefaults()
		{
			item.width = 24;
			item.height = 24;
			item.value = Item.buyPrice(silver: 2);
			item.rare = ItemRarityID.Green;
			item.accessory = true;
		}
	}

	public class ReachBroochEffect : SpiritPlayerEffect
	{
		public override void ItemUpdateAccessory(Player player, bool hideVisual)
		{
			player.GetSpiritPlayer().reachBrooch = true;
			if (player.GetSpiritPlayer().ZoneReach && !Main.dayTime) {
				player.nightVision = true;
			}
			player.AddBuff(ModContent.BuffType<Buffs.GuidingLight>(), 120);
		}
	}
}
