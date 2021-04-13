using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Sets.Vulture_Matriarch.Tome_of_the_Great_Scavenger
{
	public class Tome_of_the_Great_Scavenger : ModItem
	{
		public override void SetDefaults()
		{
			item.damage = 33;
			item.noMelee = true;
			item.noUseGraphic = false;
			item.magic = true;
			item.width = 36;
			item.height = 40;
			item.useTime = 30;
			item.useAnimation = 30;
			item.useStyle = 5;
			item.shoot = mod.ProjectileType("Tome_of_the_Great_Scavenger_Projectile");
			item.shootSpeed = 9f;
			item.knockBack = 4.4f;
			item.autoReuse = true;
			item.rare = 4;
			item.UseSound = SoundID.Item17;
			item.autoReuse = true;
			item.value = Item.sellPrice(gold: 2);
			item.useTurn = false;
			item.mana = 9;
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Tome of the Great Scavenger");
			Tooltip.SetDefault("Casts a torrent of sharp feathers\nHit enemies may drop more gold");
		}
		
		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			Vector2 muzzleOffset = Vector2.Normalize(new Vector2(speedX, speedY)) * 45f;
			if (Collision.CanHit(position, 0, 0, position + muzzleOffset, 0, 0))
			{
				position += muzzleOffset;
			}
			Vector2 perturbedSpeed = new Vector2(speedX, speedY).RotatedBy(MathHelper.ToRadians(-12));
			int p = Projectile.NewProjectile(position.X, position.Y, perturbedSpeed.X, perturbedSpeed.Y, type, damage, knockBack, player.whoAmI);
			Main.projectile[p].ai[0] = 2;
			Vector2 agaaa = new Vector2(speedX, speedY).RotatedBy(MathHelper.ToRadians(-6));
			int g = Projectile.NewProjectile(position.X, position.Y, agaaa.X, agaaa.Y, type, damage, knockBack, player.whoAmI);
			Main.projectile[p].ai[0] = 1;
			int b = Projectile.NewProjectile(position.X, position.Y, speedX, speedY, type, damage, knockBack, player.whoAmI);
			Main.projectile[p].ai[0] = 0;
			return false;
		}
	}
}
