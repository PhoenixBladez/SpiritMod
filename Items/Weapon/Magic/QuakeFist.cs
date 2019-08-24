using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Weapon.Magic
{
	public class QuakeFist : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Quake Fist");
			Tooltip.SetDefault("Launches Prismatic fire \n Occasionally inflicts foes with 'Unstable Affliction'");
		}


        private Vector2 newVect;
        int charger;
        public override void SetDefaults()
		{
			item.damage = 67;
			item.magic = true;
			item.mana = 19;
			item.width = 30;
			item.height = 34;
			item.useTime = 24;
			item.useAnimation = 24;
			item.useStyle = 5;//this makes the useStyle animate as a staff instead of as a gun
			item.noMelee = true; //so the item's animation doesn't do damage
			item.knockBack = 6;
            item.useTurn = false;
            item.value = Terraria.Item.sellPrice(0, 7, 0, 0);
            item.rare = 9;
			item.UseSound = SoundID.Item8;
			item.autoReuse = true;
			item.shoot = mod.ProjectileType("PrismaticBolt");
			item.shootSpeed = 16f;
		}
        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
        
                int proj = Projectile.NewProjectile(position.X, position.Y, speedX, speedY, mod.ProjectileType("PrismBolt2"), damage, knockBack, player.whoAmI);
                Projectile newProj = Main.projectile[proj];
                newProj.friendly = true;
                newProj.hostile = false;
                Vector2 origVect = new Vector2(speedX, speedY);
                for (int X = 0; X <= 2; X++)
                {
                    if (Main.rand.Next(2) == 1)
                    {
                        newVect = origVect.RotatedBy(System.Math.PI / (Main.rand.Next(82, 1800) / 10));
                    }
                    else
                    {
                        newVect = origVect.RotatedBy(-System.Math.PI / (Main.rand.Next(82, 1800) / 10));
                    }
                    int proj2 = Projectile.NewProjectile(position.X, position.Y, newVect.X, newVect.Y, type, damage, knockBack, player.whoAmI);
                    Projectile newProj2 = Main.projectile[proj2];
                }
            for (int i = 0; i < 3; ++i)
            {
                if (Main.rand.Next(6) == 0)
                {
                    if (Main.myPlayer == player.whoAmI)
                    {
                        Vector2 mouse = Main.MouseWorld;
                        Projectile.NewProjectile(mouse.X + Main.rand.Next(-80, 80), player.Center.Y - 1000 + Main.rand.Next(-50, 50), 0, Main.rand.Next(18, 28), mod.ProjectileType("AtlasBolt"), 50, knockBack, player.whoAmI);
                    }
                }
            }
            return false;
        }
    }
}