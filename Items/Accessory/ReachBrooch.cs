
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.ModLoader;

namespace SpiritMod.Items.Accessory
{
	[AutoloadEquip(EquipType.Neck)]
	public class ReachBrooch : SpiritAccessory
	{
		public override List<SpiritPlayerEffect> AccessoryEffects => new List<SpiritPlayerEffect>() {
			new ReachBroochEffect()

		};
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Forsworn Pendant");	
			Tooltip.SetDefaults("Generates a subtle glow around the player\nAllows for increased night vision in the Briar");	
			SpiritGlowmask.AddGlowMask(item.type, "SpiritMod/Items/Accessory/ReachBrooch");
		}
		public override void DrawArmorColor(Player drawPlayer, float shadow, ref Color color, ref int glowMask, ref Color glowMaskColor)
		{
			glowMaskColor = Color.White;
		}
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
