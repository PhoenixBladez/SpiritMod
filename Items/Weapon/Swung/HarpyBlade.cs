using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Graphics.Effects;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;
namespace SpiritMod.Items.Weapon.Swung
{
    public class HarpyBlade : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Harpy Blade");
			Tooltip.SetDefault("Rains a harpy feather from the sky at the cursor position");
		}


        public override void SetDefaults()
        {
            item.damage = 18;
            item.useTime = 35;
            item.useAnimation = 35;
            item.melee = true;            
            item.width = 46;              
            item.height = 46;             
            item.useStyle = 1;        
            item.knockBack = 3;
            item.value = Terraria.Item.sellPrice(0, 0, 10, 0);
            item.rare = 2;
            item.shootSpeed = 8;
            item.UseSound = SoundID.Item1;   
            item.autoReuse = true;
            item.useTurn = true;
            item.shoot = mod.ProjectileType("HarpyFeather");
        }
        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            Vector2 mouse = Main.MouseWorld;
            Projectile.NewProjectile(mouse.X + Main.rand.Next(-50, 50), player.Center.Y - 1000 + Main.rand.Next(-50, 50), 0, Main.rand.Next(18, 28), type, damage, knockBack, player.whoAmI);
            return false;
        }
    }
}