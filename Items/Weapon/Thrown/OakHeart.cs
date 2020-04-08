using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Weapon.Thrown
{
	public class OakHeart : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Oak Heart");
			Tooltip.SetDefault("Hitting foes may cause poisonous spores to rain down\nPoisons hit foes");
		}


        public override void SetDefaults()
        {
            item.useStyle = 1;
            item.width = 9;
            item.height = 15;
            item.noUseGraphic = true;
            item.UseSound = SoundID.Item1;
            item.thrown = true;
            item.channel = true;
			item.melee = true;
            item.noMelee = true;
            item.maxStack = 1;
            item.shoot = mod.ProjectileType("OakHeart");
            item.useAnimation = 25;
            item.useTime = 25;
            item.shootSpeed = 9f;
            item.damage = 13;
            item.knockBack = 1.5f; ;
			item.value = Item.sellPrice(0, 1, 0, 0);
            item.rare = 2;
            item.autoReuse = true;
            item.consumable = false;
        }
    }
}