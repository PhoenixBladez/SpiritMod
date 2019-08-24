using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
namespace SpiritMod.Items.Weapon.Summon
{
    public class OrbiterStaff : ModItem
    {

        int charger;

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Orbiter Staff");
            Tooltip.SetDefault("Summons a mini meteor to charge at foes");
        }


        public override void SetDefaults()
        {
            item.width = 56;
            item.height = 62;
            item.value = Item.sellPrice(0, 2, 25, 0);
            item.rare = 3;
            item.mana = 14;
            item.damage = 16;
            item.knockBack = 3;
            item.useStyle = 1;
            item.useTime = 30;
            item.useAnimation = 30;
            item.summon = true;
            item.noMelee = true;
            item.shoot = mod.ProjectileType("Minior");
            item.buffType = mod.BuffType("MiniorBuff");
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