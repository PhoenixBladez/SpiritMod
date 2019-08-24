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
			Tooltip.SetDefault("Deals both melee and throwing damage\nPoisons hit foes");
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
            item.useAnimation = 21;
            item.useTime = 21;
            item.shootSpeed = 12f;
            item.damage = 15;
            item.knockBack = 3.5f;
			item.value = Item.sellPrice(0, 1, 0, 0);
            item.rare = 2;
            item.autoReuse = true;
            item.consumable = false;
        }
    }
}