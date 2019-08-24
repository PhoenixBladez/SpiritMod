using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
namespace SpiritMod.Items.Weapon.Summon
{
	public class SkeletalonStaff : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Skeletalon Staff");
			Tooltip.SetDefault("Summons a fossilized bird to fight for you!");
		}


		public override void SetDefaults()
		{
            item.width = 26;
            item.height = 28;
            item.value = Item.sellPrice(0, 3, 25, 0);
            item.rare = 2;
            item.mana = 12;
            item.damage = 16;
            item.knockBack = 3;
            item.useStyle = 1;
            item.useTime = 30;
            item.useAnimation = 30;      
            item.summon = true;
            item.noMelee = true;
            item.shoot = mod.ProjectileType("SkeletalonMinion");
            item.buffType = mod.BuffType("SkeletalonMinionBuff");
            item.buffTime = 3600;
            item.UseSound = SoundID.Item44;
        }
					public override bool AltFunctionUse(Player player)
        {
            return true;
        }
        
        public override bool UseItem(Player player)
        {
            if(player.altFunctionUse == 2)
            {
                player.MinionNPCTargetAim();
            }
            return base.UseItem(player);
        }
        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
			            return player.altFunctionUse != 2;
            position = Main.MouseWorld;
            speedX = speedY = 0;
            return true;
        }
    }
}