using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Weapon.Summon
{
    public class ProbeStaff : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Probe Staff");
			Tooltip.SetDefault("Summons a Probe to fight for you!");
		}


        public override void SetDefaults()
        {
            item.damage = 36;
            item.summon = true;
            item.mana = 17;
            item.width = 48;
            item.height = 48;
            item.useTime = 37;
            item.useAnimation = 26;
            item.useStyle = 1;
            item.noMelee = true;
            item.knockBack = 1;
            item.value = Item.buyPrice(0, 2, 0, 0);
            item.rare = 5;
            item.UseSound = SoundID.Item44;
            item.shoot = mod.ProjectileType("ProbeMinion");
            item.shootSpeed = 10f;
            item.buffType = mod.BuffType("ProbeBuff");
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
		public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.HallowedBar, 11);
			recipe.AddIngredient(ItemID.SoulofMight, 13);
			recipe.AddIngredient(null,"PrintProbe",1);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.SetResult(this, 1);
            recipe.AddRecipe();
        }
        
    }
}