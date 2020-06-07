using System;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Consumable
{
    public class CluckItem : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Crimson Cluck");
			Tooltip.SetDefault("'Its plumage is as vibrant as the rising sun'");
		}


        public override void SetDefaults()
        {
            item.width = 30;
			item.height = 30;
            item.rare = 1;
			item.value = Item.sellPrice(0, 0, 5, 0);
            item.maxStack = 99;
            item.noUseGraphic = true;
            item.useStyle = ItemUseStyleID.SwingThrow;
            item.useTime = item.useAnimation = 20;

            item.noMelee = true;
            item.consumable = true;
            item.autoReuse = true;

        }
        public override bool UseItem(Player player)
        {
            NPC.NewNPC((int)player.Center.X, (int)player.Center.Y, ModContent.NPCType<CrimsonCluck>());
            return true;
        }

        }
}
