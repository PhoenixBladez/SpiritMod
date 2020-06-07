using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Graphics.Effects;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;
namespace SpiritMod.Items.Weapon.Swung
{
    public class SkyStrike : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Sky Strike");
			Tooltip.SetDefault("Shoots down a powerful star at the cursor position");
		}


        public override void SetDefaults()
        {
            item.damage = 43;
            item.useTime = 21;
            item.useAnimation = 21;
            item.melee = true;            
            item.width = 42;              
            item.height = 42;     
            item.rare = 4;
            item.useStyle = ItemUseStyleID.SwingThrow;        
            item.knockBack = 5;
            item.value = Terraria.Item.sellPrice(0, 2, 0, 0);
            item.UseSound = SoundID.Item1;   
            item.autoReuse = true;
            item.useTurn = true;
            item.shoot = ModContent.ProjectileType<SkyStar>();
        }
        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            Main.PlaySound(2, (int)player.position.X, (int)player.position.Y, 9);
            Vector2 mouse = new Vector2(Main.mouseX, Main.mouseY) + Main.screenPosition;
            int amount = 1;
            for (int i = 0; i < amount; ++i)
            {
                Vector2 pos = new Vector2(mouse.X + player.width * 0.5f + Main.rand.Next(-35, 36), mouse.Y - 600f);
                pos.X = (pos.X * 10f + mouse.X) / 11f + (float)Main.rand.Next(-60, 61);
                pos.Y -= 150;
                float spX = mouse.X + player.width * 0.5f + Main.rand.Next(-100, 101) - mouse.X;
                float spY = mouse.Y - pos.Y;
                if (spY < 0f)
                    spY *= -1f;
                if (spY < 20f)
                    spY = 20f;

                float length = (float)Math.Sqrt((double)(spX * spX + spY * spY));
                length = 12 / length;
                spX *= length;
                spY *= length;
                spX = spX + (float)Main.rand.Next(-40, 41) * 0.02f;
                spY = spY + (float)Main.rand.Next(-40, 41) * 0.06f;
                spX *= (float)Main.rand.Next(75, 150) * 0.006f;
                pos.X += (float)Main.rand.Next(-20, 21);
                Projectile.NewProjectile(pos.X, pos.Y, spX, spY, type, damage, 2, player.whoAmI);
            }
            return false;
        }
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.Starfury, 1);
            recipe.AddIngredient(ItemID.FallenStar, 2);
            recipe.AddIngredient(null, "StellarBar", 4);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}