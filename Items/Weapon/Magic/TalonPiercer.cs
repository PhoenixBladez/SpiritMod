using Terraria;
using System;
using Microsoft.Xna.Framework;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Weapon.Magic
{
    public class TalonPiercer : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Talon's Fury");
			Tooltip.SetDefault("Shoots feathers from off screen");
		}


        int charger;
        private Vector2 newVect;
        public override void SetDefaults()
        {
            item.damage = 25;
            item.magic = true;
            item.mana = 10;
            item.width = 46;
            item.height = 46;
            item.useTime = 25;
            item.useAnimation = 25;
            item.useStyle = 5;
            Item.staff[item.type] = true;
            item.noMelee = true;
            item.knockBack = 3.5f;
            item.useTurn = false;
            item.value = Terraria.Item.sellPrice(0, 1, 0, 0);
            item.rare = 3;
            item.UseSound = SoundID.Item20;
            item.autoReuse = true;
            item.shoot = mod.ProjectileType("BoneFeatherFriendly");
            item.shootSpeed = 12f;
        }

        public override bool Shoot(Player player, ref Microsoft.Xna.Framework.Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
			position -= new Vector2(speedX, speedY) * 100;
			speedX += (Main.rand.Next(-3,4) / 5f);
			speedY += (Main.rand.Next(-3,4) / 5f);
				 for (int k = 0; k < 15; k++)
                {
					Vector2 mouse = new Vector2(Main.mouseX, Main.mouseY) + Main.screenPosition;
					Vector2 offset = mouse - player.position;
					offset.Normalize();
					offset*= 43f;
                    int dust = Dust.NewDust(player.Center + offset, 20, 20, 1);
					
                    Main.dust[dust].velocity *= -1f;
                    Main.dust[dust].noGravity = true;
            //        Main.dust[dust].scale *= 2f;
                    Vector2 vector2_1 = new Vector2((float)Main.rand.Next(-100, 101), (float)Main.rand.Next(-100, 101));
                    vector2_1.Normalize();
                    Vector2 vector2_2 = vector2_1 * ((float)Main.rand.Next(50, 100) * 0.04f);
                    Main.dust[dust].velocity = vector2_2;
                    vector2_2.Normalize();
                    Vector2 vector2_3 = vector2_2 * 34f;
                    Main.dust[dust].position = (player.Center + offset) - vector2_3;
                }
            return true;
        }
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(null, "Talon", 14);
            recipe.AddIngredient(null, "FossilFeather", 1);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
