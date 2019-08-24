using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Weapon.Summon
{
	public class FlayerStaff : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Miniflayer Staff");
			Tooltip.SetDefault("Summons a mind-flaying jellyfish to fight for you!");
		}


		public override void SetDefaults()
		{
			item.width = 22;
			item.height = 42;
			item.value = Item.buyPrice(0, 3, 0, 0);
			item.rare = 3;
			item.damage = 22;
			item.useStyle = 1;
			item.useTime = 36;
			item.useAnimation = 36;
			item.mana = 14;
			item.summon = true;
			item.noMelee = true;
			item.shoot = mod.ProjectileType("Flayer");
			item.buffType = mod.BuffType("Flayerbuff");
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
