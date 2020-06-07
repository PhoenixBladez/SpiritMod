using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Weapon.Thrown
{
	public class TargetBottle : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Target Bottle");
			Tooltip.SetDefault("Hit it with a bullet in the air to make extremely high damage shards\n'Take a crack at this bottle!'");
		}
        public override void SetDefaults()
        {
            item.useStyle = ItemUseStyleID.SwingThrow;
            item.width = 9;
            item.height = 15;
            item.noUseGraphic = true;
            item.UseSound = SoundID.Item1;
            item.ranged = true;
            item.noMelee = true;
            item.consumable = true;
            item.maxStack = 999;
            item.shoot = ModContent.ProjectileType<TargetBottle>();
            item.useAnimation = 25;
            item.useTime = 25;
            item.shootSpeed = 8.5f;
            item.damage = 0;
            item.knockBack = 1.5f;
			item.value = Terraria.Item.sellPrice(0, 0, 0, 20);
            item.crit = 8;
            item.rare = 1;
            item.autoReuse = true;
            item.maxStack = 999;
            item.consumable = true;
        }
    }
}
