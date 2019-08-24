using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System;
namespace SpiritMod.Items.Weapon.Gun
{
    public class BrainslugLauncher : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Brainslug Blaster");
			Tooltip.SetDefault("Shoots a Brain Slug that latches onto enemies \n Hold left click to keep the Brain Slug active");
		}


        public override void SetDefaults()
        {
            item.CloneDefaults(ItemID.PiranhaGun);
            item.damage = 45;
            item.ranged = true;
            item.width = 68;
            item.height = 24;
            item.useTime = 30;
            item.useAnimation = 30;
            item.useStyle = 5;
            item.noMelee = true;
            item.knockBack = 1;
            item.useTurn = false;
            item.value = Terraria.Item.sellPrice(0, 10, 0, 0);
            item.rare = 8;
            item.UseSound = SoundID.Item11;
            item.autoReuse = false;
            item.shoot = mod.ProjectileType("Brainslug");
            item.shootSpeed = 15f;
        }
        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-10, 0);
        }
        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            {
                base.item.shoot = base.mod.ProjectileType("Brainslug");
                return true;
            }
        }
    }
}