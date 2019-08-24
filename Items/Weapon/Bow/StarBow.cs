using System;

using Microsoft.Xna.Framework;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;

namespace SpiritMod.Items.Weapon.Bow
{
    public class StarBow : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Star Bow");
            Tooltip.SetDefault("Launches moon fire from the heavens");


        }


		//private Vector2 newVect;
        public override void SetDefaults()
        {
            item.width = 22;
			item.damage = 46;
			
            item.height = 40;
            item.value = Terraria.Item.sellPrice(0, 2, 50, 0);
            item.rare = 5;

            item.crit = 6;
            item.knockBack = 4;

            item.useStyle = 5;
            item.useTime = 24;
            item.useAnimation = 24;

            item.useAmmo = AmmoID.Arrow;

            item.ranged = true;
            item.noMelee = true;
            item.autoReuse = true;

            item.shoot = mod.ProjectileType("StarBolt");
            item.shootSpeed = 9;

            item.UseSound = SoundID.Item5;
        }

       public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
			if(Main.myPlayer == player.whoAmI) {
				Vector2 mouse = Main.MouseWorld;
				 for (int i = 0; i < 3; ++i)
            {
				Projectile.NewProjectile(mouse.X + Main.rand.Next(-80, 80), player.Center.Y - 550 + Main.rand.Next(-50, 50), 0, Main.rand.Next(13,15), mod.ProjectileType("StarBolt"), damage, knockBack, player.whoAmI);
			}
			}
			return false;
        }
		 public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(null, "MoonStone", 10);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }

    }
}