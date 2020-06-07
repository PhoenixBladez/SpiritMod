using Terraria;
using System;
using Microsoft.Xna.Framework;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Weapon.Magic
{
    public class BreathOfTheZephyr : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Breath of the Zephyr");
			Tooltip.SetDefault("Creates a mighty gust of wind to damage your foes and knock them back\nRight-click to thrust like a spear, at the cost of mana");
		}


        public override void SetDefaults()
        {
            item.damage = 19;
            item.magic = true;
            item.mana = 9;
            item.width = 46;
            item.height = 46;
            item.useTime = 18;
            item.useAnimation = 18;
            item.useStyle = ItemUseStyleID.HoldingOut;
            Item.staff[item.type] = true;
            item.noMelee = true;
            item.knockBack = 5;
            item.value = Terraria.Item.sellPrice(0, 0, 40, 0);
            item.rare = 1;
            item.shoot = mod.ProjectileType("Zephyr");
            item.autoReuse = false;
        }

        public override bool AltFunctionUse(Player player)
        {
            return true;
        }
        public override bool CanUseItem(Player player)
        {
            if (player.altFunctionUse == 2)
            {
                item.UseSound = SoundID.Item1;
                item.shoot = mod.ProjectileType("ZephyrSpearProj");
				item.knockBack = 5;
                item.noMelee = false;
                item.shootSpeed = 6f;
                item.noUseGraphic = true;
            }
            else
            {
                item.UseSound = SoundID.Item34;
                item.shoot = mod.ProjectileType("Zephyr");
				item.knockBack = 10;
                item.noMelee = true;
                item.noUseGraphic = false;
                item.shootSpeed = 14f;
            }
            return true;
        }
    }
}
