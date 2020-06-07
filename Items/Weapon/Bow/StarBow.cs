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
            Tooltip.SetDefault("Launches bolts of sporadic lunar energy");
            SpiritGlowmask.AddGlowMask(item.type, "SpiritMod/Items/Weapon/Bow/StarBow_Glow");

        }


		//private Vector2 newVect;
        public override void SetDefaults()
        {
            item.width = 22;
			item.damage = 40;
			
            item.height = 40;
            item.value = Terraria.Item.sellPrice(0, 2, 50, 0);
            item.rare = 4;
            item.knockBack = 4;

            item.useStyle = ItemUseStyleID.HoldingOut;
            item.useTime = 17;
            item.useAnimation = 17;

            item.useAmmo = AmmoID.Arrow;

            item.ranged = true;
            item.noMelee = true;
            item.autoReuse = true;

            item.shoot = ModContent.ProjectileType<SleepingStar>();
            item.shootSpeed = 9;

            item.UseSound = SoundID.Item5;
        }

   /*    public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
			if(Main.myPlayer == player.whoAmI) {
				Vector2 mouse = Main.MouseWorld;
				 for (int i = 0; i < 3; ++i)
            {
				Projectile.NewProjectile(mouse.X + Main.rand.Next(-80, 80), player.Center.Y - 550 + Main.rand.Next(-50, 50), 0, Main.rand.Next(14,18), ModContent.ProjectileType<StarBolt>(), damage, knockBack, player.whoAmI);
			}
			}
			return false;
        }*/
		 public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            int projType;
            projType = Main.rand.Next(new int[] { mod.ProjectileType("SleepingStar1"), ModContent.ProjectileType<SleepingStar>() });
            Projectile.NewProjectile(position.X, position.Y, speedX, speedY, mod.ProjectileType("SleepingStar1"), damage, knockBack, player.whoAmI, 1);
			Projectile.NewProjectile(position.X, position.Y, speedX, speedY, ModContent.ProjectileType<SleepingStar>(), damage, knockBack, player.whoAmI, 2);
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