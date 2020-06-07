using System;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Consumable
{
    public class ChaosPearl : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Windshear Pearl");
			Tooltip.SetDefault("Teleports you to the projectile's landing position\nYour body cannot sustain multiple uses");
		}


        public override void SetDefaults()
        {
            item.width = 16;
            item.height = 16;
            item.value = Item.sellPrice(0, 0, 3, 0);
            item.rare = 3;
            item.maxStack = 999;
            //item.crit = 4;
            item.damage = 0;
           // item.knockBack = 3;
            item.useStyle = ItemUseStyleID.SwingThrow;
            item.useTime = item.useAnimation = 18;
          //  item.thrown = true;
            item.noMelee = true;
            item.autoReuse = false;
            item.consumable = true;
            item.noUseGraphic = true;
            item.shoot = ModContent.ProjectileType<ChaosPearl>();
            item.shootSpeed = 12;
            item.UseSound = SoundID.Item1;
        }
    }
}