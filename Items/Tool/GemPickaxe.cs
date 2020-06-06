using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Graphics.Effects;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Tool
{
    public class GemPickaxe : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Brilliant Harvester");
            Tooltip.SetDefault("Mining stone may also yield gems and ores\nCan mine Demonite and Crimtane");
		}


        public override void SetDefaults()
        {
            item.width = 32;
            item.height = 32;
            item.value = 12000;
            item.rare = 2;

            item.pick = 55;

            item.damage = 8;
            item.knockBack = 3;

            item.useStyle = ItemUseStyleID.SwingThrow;
            item.useTime = 16;
            item.useAnimation = 20; 

            item.melee = true;
            item.useTurn = true;
            item.autoReuse = true;

            item.UseSound = SoundID.Item1;
        }
 		public override void HoldItem(Player player)
		{
			MyPlayer modPlayer = player.GetSpiritPlayer();
			modPlayer.gemPickaxe = true;
        }       
    }
}
