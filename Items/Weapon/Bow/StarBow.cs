using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
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
			DisplayName.SetDefault("Seraph's Storm");
            Tooltip.SetDefault("Launches moon fire from the Heavens");
            SpiritGlowmask.AddGlowMask(item.type, "SpiritMod/Items/Weapon/Bow/StarBow_Glow");

        }


		//private Vector2 newVect;
        public override void SetDefaults()
        {
            item.width = 22;
			item.damage = 46;
			
            item.height = 40;
            item.value = Terraria.Item.sellPrice(0, 2, 50, 0);
            item.rare = 4;

            item.crit = 4;
            item.knockBack = 4;

            item.useStyle = 5;
            item.useTime = 21;
            item.useAnimation = 21;

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
				Projectile.NewProjectile(mouse.X + Main.rand.Next(-80, 80), player.Center.Y - 550 + Main.rand.Next(-50, 50), 0, Main.rand.Next(14,18), mod.ProjectileType("StarBolt"), damage, knockBack, player.whoAmI);
			}
			}
			return false;
        }
        public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
        {
            Texture2D texture;
            texture = Main.itemTexture[item.type];
            spriteBatch.Draw
            (
                ModContent.GetTexture("SpiritMod/Items/Weapon/Bow/StarBow_Glow"),
                new Vector2
                (
                    item.position.X - Main.screenPosition.X + item.width * 0.5f,
                    item.position.Y - Main.screenPosition.Y + item.height - texture.Height * 0.5f + 2f
                ),
                new Rectangle(0, 0, texture.Width, texture.Height),
                Color.White,
                rotation,
                texture.Size() * 0.5f,
                scale,
                SpriteEffects.None,
                0f
            );
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