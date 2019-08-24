using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
namespace SpiritMod.Items.Weapon.Summon
{
	public class ReachStaffChest : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Thornwild Staff");
            Tooltip.SetDefault("Summons a Briar Elemental to fight for you");

        }


		public override void SetDefaults()
		{
            item.width = 26;
            item.height = 28;
            item.value = Item.sellPrice(0, 0, 3, 0);
            item.rare = 2;
            item.mana = 7;
            item.damage = 7;
            item.knockBack = 2;
            item.useStyle = 1;
            item.useTime = 30;
            item.useAnimation = 30;    
            item.summon = true;
            item.noMelee = true;
            item.shoot = mod.ProjectileType("ReachSummon");
            item.buffType = mod.BuffType("ReachSummonBuff");
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