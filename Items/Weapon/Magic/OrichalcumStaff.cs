using Terraria;
using System;
using Microsoft.Xna.Framework;
using Terraria.ID;
using Terraria.ModLoader;
namespace SpiritMod.Items.Weapon.Magic
{
	public class OrichalcumStaff : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Orichalcum Staff");
			Tooltip.SetDefault("Summons homing orichalcum blooms at the cursor positon");
		}


		public override void SetDefaults()
		{
			item.damage = 40;
			item.magic = true;
			item.mana = 11;
			item.width = 40;
			item.height = 40;
			item.useTime = 34;
			item.useAnimation = 34;
			item.useStyle = ItemUseStyleID.HoldingOut;
			Item.staff[item.type] = true;
			item.noMelee = true; 
			item.knockBack = 1;
            item.useTurn = false;
            item.value = Terraria.Item.sellPrice(0, 3, 0, 0);
            item.rare = 5;
			item.UseSound = SoundID.Item20;
			item.autoReuse = true;
			item.shoot = mod.ProjectileType("OrichalcumStaffProj");
			item.shootSpeed = 1f;
		}
		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
                Vector2 mouse = new Vector2(Main.mouseX, Main.mouseY) + Main.screenPosition;
                Terraria.Projectile.NewProjectile(mouse.X, mouse.Y, 0f, 0f, mod.ProjectileType("OrichalcumStaffProj"), (int)(damage * 1), knockBack, player.whoAmI);
				for (int k = 0; k < 30; k++)
                {
					Vector2 offset = mouse - player.Center;
					offset.Normalize();
					if (speedX > 0)
					{
						offset = offset.RotatedBy(-0.1f);
					}
					else
					{
						offset = offset.RotatedBy(0.1f);
					}
					offset*= 58f;
                    int dust = Dust.NewDust(player.Center + offset, 1, 1, 242);
					Main.dust[dust].noGravity = true;
					Main.dust[dust].scale = 1.5f;
					float dustSpeed = Main.rand.Next(23) / 5;
					switch (Main.rand.Next(3))
					{
						case 0:
							Main.dust[dust].velocity = new Vector2(speedX * dustSpeed, speedY * dustSpeed).RotatedBy(1.57f);
							break;
						case 1:
							Main.dust[dust].velocity = new Vector2(speedX * dustSpeed, speedY * dustSpeed);
							break;
						case 2:
							Main.dust[dust].velocity = new Vector2(speedX * dustSpeed, speedY * dustSpeed).RotatedBy(-1.57f);
							break;
					}
                }
           
            return false;
        }
		public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.OrichalcumBar, 12);
             recipe.AddTile(TileID.MythrilAnvil);
            recipe.SetResult(this, 1);
            recipe.AddRecipe();
        }
	}
}