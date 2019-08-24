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
			DisplayName.SetDefault("Talon Piercer");
			Tooltip.SetDefault("Shoots a barrage of different feathers");
		}


        int charger;
        private Vector2 newVect;
        public override void SetDefaults()
        {
            item.damage = 22;
            item.magic = true;
            item.mana = 15;
            item.width = 46;
            item.height = 46;
            item.useTime = 27;
            item.useAnimation = 27;
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
            item.shootSpeed = 8f;
        }

        public override bool Shoot(Player player, ref Microsoft.Xna.Framework.Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            Vector2 origVect = new Vector2(speedX, speedY);
            for (int X = 0; X < 2; X++)
            {
                if (Main.rand.Next(2) == 1)
                {
                    newVect = origVect.RotatedBy(System.Math.PI / (Main.rand.Next(92, 2000) / 10));
                }
                else
                {
                    newVect = origVect.RotatedBy(-System.Math.PI / (Main.rand.Next(92, 2000) / 10));
                }
                Projectile proj = Main.projectile[Projectile.NewProjectile(position.X, position.Y, newVect.X, newVect.Y, type, damage, knockBack, player.whoAmI)];
                proj.friendly = true;
                proj.hostile = false;
                proj.netUpdate = true;
                {
                    {
                        charger++;
                        if (charger >= 6)
                        {
                            for (int I = 0; I < 1; I++)
                            {
                                Projectile.NewProjectile(position.X - 8, position.Y + 8, speedX + ((float)Main.rand.Next(-230, 230) / 100), speedY + ((float)Main.rand.Next(-230, 230) / 100), mod.ProjectileType("GiantFeather"), damage, knockBack, player.whoAmI, 0f, 0f);
                            }
                            charger = 0;
                        }
                    }
                }
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
