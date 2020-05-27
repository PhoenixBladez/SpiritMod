using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Graphics.Effects;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Weapon.Swung
{
    public class Headsplitter : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Headsplitter");
			Tooltip.SetDefault("Right click to release an explosion of vengeance \nUsing it too frequently will reduce its damage");
		}


        public override void SetDefaults()
        {
            item.damage = 21;            
            item.melee = true;            
            item.width = 34;              
            item.height = 40;             
            item.useTime = 25;           
            item.useAnimation = 25;     
            item.useStyle = 1;        
            item.knockBack = 6;
            item.value = Terraria.Item.sellPrice(0, 0, 20, 0);
            item.rare = 2;
            item.UseSound = SoundID.Item1;        
            item.shoot = mod.ProjectileType("PestilentSwordProjectile");
            item.shootSpeed = 12f;
			item.autoReuse = true;			
        }
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(null, "BloodFire", 12);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this, 1);
            recipe.AddRecipe();
        }
		public override bool AltFunctionUse(Player player)
        {
            return true;
        }
		public override void OnHitNPC(Player player, NPC target, int damage, float knockback, bool crit)
        {
            if (Main.rand.Next(4) == 0)
            {
                target.AddBuff(mod.BuffType("SurgingAnguish"), 180);
            }
        }
		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            MyPlayer modPlayer = player.GetSpiritPlayer();
			if (modPlayer.shootDelay < 150 && player.altFunctionUse == 2)
			{
				damage = 1 + (int)((damage * 1.5f) / (MathHelper.Clamp((float)Math.Sqrt(modPlayer.shootDelay), 1, 180)));
				Projectile.NewProjectile(position.X, position.Y, 0, 0, mod.ProjectileType("FlayedExplosion"), damage, knockBack, Main.myPlayer);
				modPlayer.shootDelay = 180;
			}
            return false;
        }
    }
}