using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Weapon.Thrown
{
	public class SpiritBall : ModItem
    {
		public override void SetStaticDefaults()
		{
            DisplayName.SetDefault("Soul Shatter");
            Tooltip.SetDefault("Shoots out an orb that leaves behind lingering clouds of souls");
        }


        public override void SetDefaults()
        {
            item.useStyle = 1;
            item.width = 24;
            item.height = 24;
            item.noUseGraphic = true;
            item.UseSound = SoundID.Item106;
            item.thrown = true;
            item.channel = true;
            item.noMelee = true;
            item.shoot = mod.ProjectileType("SpiritBall");
            item.useAnimation = 18;
            item.consumable = false;
            item.maxStack = 1;
            item.useTime = 18;
            item.shootSpeed = 15.5f;
            item.damage = 39;
            item.knockBack = 2f;
			item.value = Terraria.Item.sellPrice(0, 0, 3, 0);
            item.rare = 5;
            item.autoReuse = true;
        }
    }
}
