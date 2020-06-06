using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Weapon.Thrown
{
    public class CultDagger : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Sacrificial Dagger");
            Tooltip.SetDefault("Steals a small amount of mana upon hitting enemies");

        }


        public override void SetDefaults()
        {
            item.useStyle = ItemUseStyleID.SwingThrow;
            item.width = 30;
            item.height = 50;
            item.noUseGraphic = true;
            item.UseSound = SoundID.Item1;
            item.melee = true;
            item.channel = true;
            item.noMelee = true;
            item.shoot = mod.ProjectileType("CultDagger");
            item.useAnimation = 14;
            item.maxStack = 999;
            item.useTime = 14;
            item.shootSpeed = 11.5f;
            item.damage = 97;
            item.knockBack = 1.5f;
			item.value = Item.buyPrice(0, 45, 0, 0);
            item.rare = 10;
            item.autoReuse = true;
        }
    }
}
