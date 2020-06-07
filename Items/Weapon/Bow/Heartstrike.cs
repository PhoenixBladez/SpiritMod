using Terraria;
using System;
using Terraria.ID;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Terraria.ModLoader;
using SpiritMod.Projectiles;

namespace SpiritMod.Items.Weapon.Bow
{
    public class Heartstrike : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Heartstrike");
			Tooltip.SetDefault("Right click after 5 shots to launched a flayed arrow\nEnemies hit by flayed arrows will explode upon death");
		}


        int counter = 0;
        public override void SetDefaults()
        {
            item.damage = 18;
            item.noMelee = true;
            item.ranged = true;
            item.width = 24;
            item.height = 46;
            item.useTime = 25;
            item.useAnimation = 25;
            item.useStyle = ItemUseStyleID.HoldingOut;
            item.shoot = 3;
            item.useAmmo = AmmoID.Arrow;
            item.knockBack = 4;
            item.value = Item.buyPrice(0, 1, 0, 0);
            item.rare = 2;
            item.UseSound = SoundID.Item5;
            item.autoReuse = true;
            item.shootSpeed = 8f;
        }
        public override bool AltFunctionUse(Player player)
        {
            return true;
        }
        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
			if (player.altFunctionUse == 2)
			{
				type = mod.ProjectileType("FlayedShot");
				if (counter > 0)
				{
					return false;
				}
				else
				{
					counter = 5;
				}
                
			}
			else
			{
				counter--;
			}
            if (counter == 0)
            {
                Main.PlaySound(new Terraria.Audio.LegacySoundStyle(2, 20));
                {
                    for (int i = 0; i < 7; i++)
                    {
                        int num = Dust.NewDust(player.position, player.width, player.height, 5, 0f, -2f, 0, default(Color), 2f);
                        Main.dust[num].noGravity = true;
                        Main.dust[num].position.X += Main.rand.Next(-50, 51) * .05f - 1.5f;
                        Main.dust[num].position.Y += Main.rand.Next(-50, 51) * .05f - 1.5f;
                        Main.dust[num].scale *= .25f;
                        if (Main.dust[num].position != player.Center)
                            Main.dust[num].velocity = player.DirectionTo(Main.dust[num].position) * 6f;
                    }
                }
            }
            return true;
        }
        public override bool CanUseItem(Player player)
        {
            if (player.altFunctionUse == 2)
            {
                if (counter > 0)
                {
                    return false;
                }  
                else
                {
                    return true;
                }
   
            }
            return true;
        }
        
        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-6, 0);
        }
       public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(null, "BloodFire", 12);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this, 1);
            recipe.AddRecipe();
        }
    }
}