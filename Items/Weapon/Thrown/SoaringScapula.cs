using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
namespace SpiritMod.Items.Weapon.Thrown
{
    public class SoaringScapula : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Soaring Scapula");
			Tooltip.SetDefault("Pulls enemies towards the ground");
		}


        public override void SetDefaults()
        {
            item.width = 18;
            item.height = 20;
            item.value = Item.sellPrice(0, 0, 40, 0);
            item.rare = 6;
            item.maxStack = 1;
            item.crit = 4;
            item.damage = 25;
            item.knockBack = 2;
            item.useStyle = 1;
            item.useTime = item.useAnimation = 15;            
            item.ranged = true;
            item.noMelee = true;
            item.autoReuse = false;
            item.consumable = false;
            item.noUseGraphic = true;
            item.shoot = mod.ProjectileType("Scapula");
            item.shootSpeed = 8;
            item.UseSound = SoundID.Item1;
        }
    }
}