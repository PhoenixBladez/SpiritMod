using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System;
namespace SpiritMod.Items.Weapon.Gun
{
    public class Terravolver : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Terravolver");
            Tooltip.SetDefault("'Nature goes out with a bang'\n33% chance not to consume ammo\nRapidly shoots out Elemental Bullets that inflict a multitude of debuffs on hit foes\nRarely shoots out an explosive Terra Bomb that hits multiple foes\nRight click to shoot out multiple homing bolts of energy");

        }


        private Vector2 newVect;
        int charger;
        public override void SetDefaults()
        {
            item.damage = 43;
            item.ranged = true;
            item.width = 58;
            item.height = 32;
            item.useTime = 8;
            item.useAnimation = 8;
            item.useStyle = 5;
            item.noMelee = true;
            item.knockBack = 0.3f;
            item.useTurn = false;
            item.value = Terraria.Item.sellPrice(0, 3, 0, 0);
            item.rare = 8;
            item.UseSound = SoundID.Item92;
            item.autoReuse = true;
            item.shoot = mod.ProjectileType("TerraBullet");
            item.shootSpeed = 14f;
            item.useAmmo = AmmoID.Bullet;
        }
		 public override bool ConsumeAmmo(Player player)
        {
            if (Main.rand.Next(3) == 0)
            {
                return false;
            }
            return true;
        }
        public override bool AltFunctionUse(Player player)
        {
            return true;
        }
        public override bool Shoot(Player player, ref Microsoft.Xna.Framework.Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            if (player.altFunctionUse == 2)
            {
				item.damage = 64;
                item.useTime = 29;
                item.useAnimation = 29;
				item.knockBack = 8;

                Vector2 origVect = new Vector2(speedX, speedY);
                for (int X = 0; X <= 3; X++)
                {
                    if (Main.rand.Next(2) == 1)
                    {
                        newVect = origVect.RotatedBy(System.Math.PI / (Main.rand.Next(82, 1800) / 10));
                    }
                    else
                    {
                        newVect = origVect.RotatedBy(-System.Math.PI / (Main.rand.Next(82, 1800) / 10));
                    }
                    int proj2 = Projectile.NewProjectile(position.X, position.Y, newVect.X, newVect.Y, mod.ProjectileType("TerraBullet1"), 45, knockBack, player.whoAmI);
                    Projectile newProj2 = Main.projectile[proj2];
                }
                return false;
            }
            else
		    item.damage = 43;
            item.useTime = 8;
            item.useAnimation = 8;
			item.knockBack = 0.3f;
            {
                charger++;
                if (charger >= 7)
                {
                    for (int I = 0; I < 1; I++)
                    {
                        Projectile.NewProjectile(position.X - 8, position.Y + 8, speedX + ((float)Main.rand.Next(-230, 230) / 100), speedY + ((float)Main.rand.Next(-230, 230) / 100), mod.ProjectileType("TerraBomb"), 60, knockBack, player.whoAmI, 0f, 0f);
                    }
                    charger = 0;
                }

                type = mod.ProjectileType("TerraBullet");
                float spread = 10 * 0.00774f;//45 degrees converted to radians
                float baseSpeed = (float)Math.Sqrt(speedX * speedX + speedY * speedY);
                double baseAngle = Math.Atan2(speedX, speedY);
                double randomAngle = baseAngle + (Main.rand.NextFloat() - 0.5f) * spread;
                speedX = baseSpeed * (float)Math.Sin(randomAngle);
                speedY = baseSpeed * (float)Math.Cos(randomAngle);
                return true;
            }
        }
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(null, "TrueShadowShot", 1);
            recipe.AddIngredient(null, "TrueHolyBurst", 1);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.SetResult(this, 1);
            recipe.AddRecipe();

            ModRecipe recipe1 = new ModRecipe(mod);
            recipe1.AddIngredient(null, "TrueCrimbine", 1);
            recipe1.AddIngredient(null, "TrueHolyBurst", 1);
            recipe1.AddTile(TileID.MythrilAnvil);
            recipe1.SetResult(this, 1);
            recipe1.AddRecipe();
        }
        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-10, 0);
        }
    }
}