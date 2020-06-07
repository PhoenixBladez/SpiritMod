using Terraria;
using System;
using Terraria.ID;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Terraria.ModLoader;

namespace SpiritMod.Items.Weapon.Gun
{
    public class KnocbackGun : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Freeman");
			Tooltip.SetDefault("Shoots a rocket that can be controlled by the cursor\n'The right man in the wrong place can make all the difference in the world'");
		}

		
        public override void SetDefaults()
        {
            item.damage = 25;
            item.ranged = true;
            item.width = 65;
            item.height = 21;
            item.useTime = 40;
            item.useAnimation = 40;
			item.useTurn = false;
            item.useStyle = ItemUseStyleID.HoldingOut;
            item.noMelee = true;
            item.knockBack = 7;
            item.channel = true;
            item.value = Item.buyPrice(0, 11, 0, 0);
            item.rare = 3;
            item.UseSound = SoundID.Item36;
            item.autoReuse = true;
            item.shoot = ModContent.ProjectileType<FreemanRocket>();
            item.shootSpeed = 6f;
            item.useAmmo = AmmoID.Bullet;
        }

        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-10, 0);
        }

        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            Main.PlaySound(SoundLoader.customSoundType, player.position, mod.GetSoundSlot(SoundType.Custom, "Sounds/CoilRocket"));
            type = ModContent.ProjectileType<FreemanRocket>();
            return true;
        }
    }
}