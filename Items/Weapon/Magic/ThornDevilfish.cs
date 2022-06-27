
using Microsoft.Xna.Framework;

using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Weapon.Magic
{
	public class ThornDevilfish : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Thorn Devilfish");
			Tooltip.SetDefault("Shoots out poisonous bubbles");
		}



		public override void SetDefaults()
		{
			Item.damage = 11;
			Item.staff[Item.type] = true;
			Item.noMelee = true;
			Item.DamageType = DamageClass.Magic;
			Item.width = 32;
			Item.height = 32;
			Item.useTime = 27;
			Item.mana = 10;
			Item.useAnimation = 27;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.knockBack = 3;
			Item.value = Terraria.Item.sellPrice(0, 0, 15, 0);
			Item.rare = ItemRarityID.Green;
			Item.autoReuse = false;
			Item.shootSpeed = 7;
			Item.UseSound = SoundID.Item111;
			Item.shoot = ProjectileID.ToxicBubble;
		}

		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) 
        {
            for (int I = 0; I < 3; I++)
            {
                int p = Projectile.NewProjectile(position.X, position.Y, velocity.X + ((float)Main.rand.Next(-180, 180) / 100), velocity.Y + ((float)Main.rand.Next(-180, 180) / 100), ProjectileID.ToxicBubble, damage, knockback, player.whoAmI, 0f, 0f);
                Main.projectile[p].timeLeft = 60;
                Main.projectile[p].scale *= .6f;
				Main.projectile[p].DamageType = DamageClass.Magic;
            }
			return false;
		}
	}
}
