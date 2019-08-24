using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Graphics.Effects;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Weapon.Swung
{
    public class DarkfireKatana : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Darkfire Katana");
			Tooltip.SetDefault("Right Click to make the Sword and it's user immensely powerful for 30 seconds. \n The weapon of an Epic Ninja");
		}


        public override void SetDefaults()
        {

            item.damage = 105;
            item.useTime = 12;
            item.useAnimation = 12;
            item.melee = true;            
            item.width = 60;              
            item.useStyle = 1;        
            item.knockBack = 5;      
            item.value = 19000;        
            item.rare = 11;
            item.UseSound = SoundID.Item1;          
            item.autoReuse = true;
            item.useTurn = true;
            item.crit = 6;
        }

        public override void UpdateInventory(Player player)
        {
            if (player.FindBuffIndex (mod.BuffType("PowerUnleash")) >= 0)
            {
                item.damage = 145;
                item.useTime = 8;
                item.useAnimation = 8;
            }
            else
            {
                item.damage = 110;
                item.useTime = 12;
                item.useAnimation = 12;
            }
        }

        public override bool UseItem(Player player)
        {

            if (player.altFunctionUse == 2)
            {
                if (player.FindBuffIndex (mod.BuffType("UnPowered")) >= 0)
                {

                }
                else
                {
                    player.AddBuff(mod.BuffType("PowerUnleash"), 1800);
                    player.AddBuff(mod.BuffType("UnPowered"), 7200);
                }
            }
            

            return true;
        }
        public override bool AltFunctionUse(Player player)
        {
            return true;
        }
    }
}
