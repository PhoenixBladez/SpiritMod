using System;

using Microsoft.Xna.Framework;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;

namespace SpiritMod.Items.Weapon.Magic
{
    public class SepulchreStaff : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Staff of the Dark Magus");
            Tooltip.SetDefault("Shoots out a ball of green flames that jumps from enemy to enemy");
        }



        public override void SetDefaults()
        {
            item.damage = 11;
            Item.staff[item.type] = true;
            item.noMelee = true;
            item.magic = true;
            item.width = 34;
            item.height = 34;
            item.useTime = 26;
            item.mana = 8;
            item.useAnimation = 26;
            item.useStyle = 5;
            item.knockBack = 4f ;
            item.value = Terraria.Item.sellPrice(0, 3, 0, 0);
            item.rare = 1;
            item.UseSound = SoundID.Item8;
            item.autoReuse = false;
            item.shootSpeed = 14;
            item.shoot = mod.ProjectileType("CursedBallJump");
        }
        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            Vector2 muzzleOffset = Vector2.Normalize(new Vector2(speedX, speedY - 3)) * 45f;
            if (Collision.CanHit(position, 0, 0, position + muzzleOffset, 0, 0))
            {
                position += muzzleOffset;
            }
            return true;
        }
    }
}
