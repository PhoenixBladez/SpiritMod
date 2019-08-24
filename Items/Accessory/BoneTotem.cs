using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Accessory
{
    public class BoneTotem : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Undead Totem");
			Tooltip.SetDefault("Increases melee damage by 7% and melee speed by 5% when under half health");
		}

        public override void SetDefaults()
		{
			item.width = 26;
			item.height = 40;
            item.value = Item.buyPrice(0, 2, 0, 0);
			item.rare = 3;

			item.accessory = true;
			item.defense = 1;
		}

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			if (player.statLife <= player.statLifeMax2 / 2)
			{
                player.meleeDamage += 0.07f;
                player.meleeSpeed += 0.05f;
            }
		}
	}
}
