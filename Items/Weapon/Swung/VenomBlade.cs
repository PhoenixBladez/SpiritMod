using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Graphics.Effects;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;
namespace SpiritMod.Items.Weapon.Swung
{
    public class VenomBlade : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Venom Blade");
			Tooltip.SetDefault("Occasionally shoots out a bolt of powerful venom");
		}


        public override void SetDefaults()
        {
            item.damage = 52;
            item.useTime = 26;
            item.useAnimation = 26;
            item.melee = true;            
            item.width = 60;              
            item.height = 64;             
            item.useStyle = 1;        
            item.knockBack = 5;
            item.value = Terraria.Item.sellPrice(0, 2, 0, 0);
            item.rare = 5;
            item.shootSpeed = 10;
            item.UseSound = SoundID.Item1;   
            item.autoReuse = true;
            item.useTurn = true;
            item.crit = 12;
            item.shoot = mod.ProjectileType("GeodeStaveProjectile");
    }

        public override bool OnlyShootOnSwing => true;

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.SpiderFang, 12);
            recipe.AddIngredient(ItemID.SoulofNight, 4);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}