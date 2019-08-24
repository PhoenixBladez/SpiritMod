using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Weapon.Spear
{
    public class EtherealSpear : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Ethereal Spear");
			Tooltip.SetDefault("Inflicts Essence Trap");
		}


        public override void SetDefaults()
        {
            item.CloneDefaults(ItemID.BoneJavelin);
            item.useStyle = 1;
            item.width = 38;
            item.height = 38;
            item.noUseGraphic = true;
            item.UseSound = SoundID.Item1;
            item.thrown = true;
            item.channel = true;
            item.noMelee = true;
            item.consumable = true;
            item.maxStack = 999;
            item.shoot = mod.ProjectileType("EtherealSpearProjectile");
            item.useAnimation = 22;
            item.useTime = 22;
            item.shootSpeed = 11f;
            item.damage = 42;
            item.knockBack = 5f;
			item.value = Item.buyPrice(0, 0, 1, 0);
			item.value = Item.sellPrice(0, 0, 0, 40);
            item.crit = 6;
            item.rare = 5;
            item.autoReuse = true;
        }
    }
}