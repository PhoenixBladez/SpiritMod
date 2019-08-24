using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
namespace SpiritMod.Items.Weapon.Thrown
{
    public class InfernalJavelin : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Infernal Javelin");
			Tooltip.SetDefault("'A spear forged with fire' \n Combusts hit foes, with additional hits causing the flame to intensify.");
		}


        public override void SetDefaults()
        {
            item.width = item.height = 46;
            item.rare = 5;
            item.value = Terraria.Item.sellPrice(0, 3, 70, 0);
            item.damage = 42;
            item.knockBack = 6;
            item.useStyle = 1;
            item.useTime = item.useAnimation = 25;
            item.thrown = true;
            item.noMelee = true;
            item.autoReuse = true;
            item.noUseGraphic = true;
            item.shoot = mod.ProjectileType("InfernalJavelin");
            item.shootSpeed = 14;
            item.UseSound = SoundID.Item1;
        }
    }
}