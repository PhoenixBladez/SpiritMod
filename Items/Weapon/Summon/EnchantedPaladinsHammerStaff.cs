using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Weapon.Summon
{
    public class EnchantedPaladinsHammerStaff : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Enchanted Paladin's Hammer Staff");
			Tooltip.SetDefault("Summons an Enchanted Paladin's Hammer to fight for you!");
		}


        public override void SetDefaults()
        {
            item.damage = 55;
            item.summon = true;
            item.mana = 15;
            item.width = 36;
            item.height = 36;
            item.useTime = 40;
            item.useAnimation = 26;
            item.useStyle = 1;
            item.noMelee = true;
            item.knockBack = 1;
            item.value = Item.buyPrice(0, 8, 90, 0);
            item.rare = 8;
            item.UseSound = SoundID.Item44;
            item.shoot = mod.ProjectileType("EnchantedPaladinsHammerMinion");
            item.shootSpeed = 10f;
            item.buffType = mod.BuffType("EnchantedPaladinsHammerBuff");
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