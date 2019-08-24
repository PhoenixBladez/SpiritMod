using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Weapon.Thrown
{
    public class StarKunai : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Constellation Kunai");
            Tooltip.SetDefault("Shoots out four kunais in a spread");

        }



        public override void SetDefaults()
        {
            item.useStyle = 1;
            item.width = 9;
            item.height = 15;
            item.noUseGraphic = true;
            item.UseSound = SoundID.Item1;
            item.thrown = true;
            item.channel = true;
            item.noMelee = true;
            item.consumable = true;
            item.maxStack = 999;
            item.shoot = mod.ProjectileType("StarKunai");
            item.useAnimation = 20;
            item.useTime = 20;
            item.shootSpeed = 8.5f;
            item.damage = 19;
            item.knockBack = 1f;
            item.value = Terraria.Item.sellPrice(0, 0, 2, 0);
            item.rare = 3;
            item.autoReuse = true;
            item.maxStack = 999;
            item.consumable = true;
        }
        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            for (int i = 0; i < 4; i++)
            {
                float spread = 30f * 0.0174f;//45 degrees converted to radians
                float baseSpeed = (float)Math.Sqrt(speedX * speedX + speedY * speedY);
                double baseAngle = Math.Atan2(speedX, speedY);
                double randomAngle = baseAngle + (Main.rand.NextFloat() - 0.5f) * spread;
                speedX = baseSpeed * (float)Math.Sin(randomAngle);
                speedY = baseSpeed * (float)Math.Cos(randomAngle);
                Projectile.NewProjectile(position.X, position.Y, speedX, speedY, mod.ProjectileType("StarKunai"), item.damage, knockBack, item.owner, 0, 0);
            }
            return false;
        }
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(null, "SteamParts", 2);
            recipe.AddIngredient(null, "Kunai_Throwing", 50);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this, 50);
            recipe.AddRecipe();
        }
    }
}
