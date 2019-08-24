using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;

namespace SpiritMod.Items.Accessory
{
	public class ShadowLens : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Animus Lens");
			Tooltip.SetDefault("Summons a spirit and shadow guardian to protect you\nThe spirit guardian shoots out homing bolts at foes\nThe shadow guardian periodically shoots out a pulse of shadows\nSpirit and shadow guardians deal more damage and fire more frequently when under half health\nIncreases critical strike chance by 7%");
		}


		public override void SetDefaults()
		{
			item.width = 32;
            item.height = 32;
			item.value = Item.buyPrice(0, 0, 11, 0);
            item.rare = 5;
            item.expert = true;
            item.defense = 1;
			item.accessory = true;
		}
        public override void UpdateAccessory(Player player, bool hideVisual)
		{
            player.meleeCrit += 7;
            player.thrownCrit += 7;
            player.magicCrit += 7;
            player.rangedCrit += 7;
            player.GetModPlayer<MyPlayer>(mod).animusLens = true;
        }

	}
}
