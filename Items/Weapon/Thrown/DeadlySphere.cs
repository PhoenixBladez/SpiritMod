using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Weapon.Thrown
{
	public class DeadlySphere : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Spike Sphere");
		}


        public override void SetDefaults()
        {
            item.useStyle = 1;
            item.width = 30;
            item.height = 50;
            item.noUseGraphic = true;
            item.UseSound = SoundID.Item1;
            item.thrown = true;
            item.channel = true;
            item.noMelee = true;
            item.shoot = mod.ProjectileType("DeadlySphere");
            item.useAnimation = 19;
            item.consumable = true;
            item.maxStack = 999;
            item.useTime = 19;
            item.shootSpeed = 15.5f;
            item.damage = 47;
            item.knockBack = 1f;
			item.value = Terraria.Item.sellPrice(0, 0, 3, 0);
            item.rare = 8;
            item.autoReuse = true;
            item.maxStack = 999;
            item.consumable = true;
        }
    }
}
