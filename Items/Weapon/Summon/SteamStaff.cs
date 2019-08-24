using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Weapon.Summon
{
    public class SteamStaff : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Starplate Staff");
			Tooltip.SetDefault("Summons a Starplate Spider to fight for you!\nThe Starplate Spider periodically rains down homing starry wisps");
		}


        public override void SetDefaults()
        {
            item.damage = 23;
            item.summon = true;
            item.mana = 11;
            item.width = 58;
            item.height = 58;
            item.useTime = 37;
            item.useAnimation = 37;
            item.useStyle = 1;
            item.noMelee = true;
            item.knockBack = 2;
            item.value = Item.buyPrice(0, 2, 0, 0);
            item.rare = 3;
            item.UseSound = SoundID.Item44;
            item.shoot = mod.ProjectileType("SteamMinion");
            item.shootSpeed = 10f;
            item.buffType = mod.BuffType("SteamMinionBuff");
            item.buffTime = 3600;

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