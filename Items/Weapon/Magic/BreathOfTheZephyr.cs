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
			Tooltip.SetDefault("Creats a mighty gust of wind to damage your foes");
		}


        public override void SetDefaults()
        {
            item.damage = 19;
            item.magic = true;
            item.mana = 13;
            item.width = 46;
            item.height = 46;
            item.useTime = 18;
            item.useAnimation = 18;
            item.useStyle = 5;
            Item.staff[item.type] = true;
            item.noMelee = true;
            item.knockBack = 0;
            item.value = Terraria.Item.sellPrice(0, 1, 0, 0);
            item.rare = 1;
            item.UseSound = SoundID.Item34;
            item.autoReuse = false;
            item.shoot = mod.ProjectileType("Zephyr");
            item.shootSpeed = 26f;
            item.autoReuse = false;
        }
    }
}
