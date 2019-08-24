using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Graphics.Effects;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;
namespace SpiritMod.Items.Weapon.Swung
{
    public class QuicksilverSword2 : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Quicksilver Slicer");
            Tooltip.SetDefault("Shoots out multiple Quicksilver Waves from the Earth that split into multiple bouncing Quicksilver Bolts upon hitting enemies");
        }


        public override void SetDefaults()
        {
            item.damage = 76;
            item.useTime = 19;
            item.useAnimation = 19;
            item.melee = true;            
            item.width = 70;              
            item.height = 70;
            item.useStyle = 1;        
            item.knockBack = 5;
            item.value = Terraria.Item.sellPrice(0, 3, 0, 0);
            item.rare = 8;
            item.shootSpeed = 8;
            item.UseSound = SoundID.Item69;   
            item.autoReuse = true;
            item.useTurn = true;
            item.shoot = mod.ProjectileType("HarpyFeather");
        }
        public override void MeleeEffects(Player player, Rectangle hitbox)
        {
            {
                int dust = Dust.NewDust(new Vector2(hitbox.X, hitbox.Y), hitbox.Width, hitbox.Height, DustID.SilverCoin);
                Main.dust[dust].noGravity = true;
                Main.dust[dust].velocity *= 0f;

            }
        }
        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            for (int i = 0; i < 3; ++i)
            {
                if (Main.myPlayer == player.whoAmI)
                {
                    Vector2 mouse = Main.MouseWorld;
                    Projectile.NewProjectile(mouse.X + Main.rand.Next(-60, 60), player.Center.Y + 1000 + Main.rand.Next(-50, 50), 0, Main.rand.Next(-28, -18), mod.ProjectileType("QuicksilverWave"), damage, knockBack, player.whoAmI);
                }
            }
            return false;
        }
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(null, "Material", 16);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}