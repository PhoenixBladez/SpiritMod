using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpiritMod.Mechanics.Fathomless_Chest.Items.Water_Bottle
{
    public class Water_Bottle : ModItem
    {
        public override void SetDefaults()
        {
            item.width = 26;
            item.height = 36;
            item.useTurn = true;
            item.maxStack = 30;
			item.rare = -11;
            item.useAnimation = 15;
            item.useTime = 15;
            item.useStyle = 2;
            item.UseSound = SoundID.Item3;
            item.consumable = false;
            item.value = Item.sellPrice(0, 2, 0, 0);
        }

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Fathomless Water Bottle");
            Tooltip.SetDefault("Restores 60 life\nCan over-heal\nOver-healing fades away overtime\nGrants 10 seconds of lava immunity\n60 second cooldown");
        }
		public override bool UseItem(Player player) 
		{
			player.statLife += 60;
			player.statLifeMax2 += 60;
			player.statLifeMax += 60;
			player.HealEffect(60, true);
			player.AddBuff(21,60*60);
			player.AddBuff(1,60*10);
			return true;
		}
		
		public override bool CanUseItem(Player player) 
		{
			/*if (player.HasBuff(21))
				return false;*/
			return true;
		}
    }
}
