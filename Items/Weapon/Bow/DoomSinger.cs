using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Weapon.Bow
{
    public class DoomSinger : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Doom Singer");
			Tooltip.SetDefault("No mortal should wield this.");
		}


        public override void SetDefaults()
        {
            item.damage = 70;
            item.ranged = true;
            item.width = 16;
            item.height = 27;
            item.useTime = 16;
            item.useAnimation = 16;
            item.useStyle = 5;
            item.noMelee = true;
            item.knockBack = 4;
            item.useTurn = false;
            item.value = Terraria.Item.sellPrice(0, 10, 0, 0);
            item.rare = 7;
            item.UseSound = SoundID.Item11;
            item.autoReuse = true;
            item.shoot = 1;
            item.shootSpeed = 16f;
            item.useAmmo = AmmoID.Arrow;
        }



        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            if (type == ProjectileID.WoodenArrowFriendly)
            {
                type = mod.ProjectileType("DoomSingerArrow");
                return true;
            }
            {
                int numberProjectiles = 5;
                for (int i = 0; i < numberProjectiles; i++)
                {
                    Vector2 perturbedSpeed = new Vector2(speedX, speedY).RotatedByRandom(MathHelper.ToRadians(30));
                    Projectile.NewProjectile(position.X, position.Y, perturbedSpeed.X, perturbedSpeed.Y, mod.ProjectileType("DoomSingerArrow"), damage, knockBack, player.whoAmI);
                }
                return false;
            }
            }

            /*public override Vector2? HoldoutOffset()
            {
                return new Vector2(10, 0);
            }*/
        }
    }