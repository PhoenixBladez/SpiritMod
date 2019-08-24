using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
namespace SpiritMod.Items.Weapon.Thrown
{
    public class ReachKnife : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Bramble Daggers");
			Tooltip.SetDefault("Hitting enemies and tiles may cause the dagger to split into multiple spore clouds");
		}


        public override void SetDefaults()
        {
            item.width = 20;
            item.height = 46;
            item.rare = 5;
            item.value = Terraria.Item.sellPrice(0, 3, 70, 0);
            item.damage = 17;
            item.knockBack = 2;
            item.useStyle = 1;
            item.useTime = item.useAnimation = 24;
            item.thrown = true;
            item.noMelee = true;
            item.autoReuse = true;
            item.noUseGraphic = true;
            item.shoot = mod.ProjectileType("ReachKnife");
            item.shootSpeed = 8;
            item.UseSound = SoundID.Item1;
        }
    }
}