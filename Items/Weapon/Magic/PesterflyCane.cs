using Microsoft.Xna.Framework;
using SpiritMod.Projectiles.Magic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Weapon.Magic
{
	public class PesterflyCane : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Pesterfly Cane");
			Tooltip.SetDefault("Summons pesterflies that swarm around hit enemies");
			Item.staff[item.type] = true; //Set here since it's universal for this item
		}

		public override void SetDefaults()
		{
			item.damage = 14;
			item.magic = true;
			item.mana = 8;
			item.width = 32;
			item.height = 32;
			item.useStyle = ItemUseStyleID.HoldingOut;
			item.noMelee = true;
			item.knockBack = 5;
			item.value = 20000;
			item.rare = ItemRarityID.Green;
			item.UseSound = SoundID.Item20;
			item.autoReuse = true;
			item.shoot = ModContent.ProjectileType<Pesterfly>();
			item.shootSpeed = 8f;

			item.useAnimation = 9;
			item.useTime = 3;
			item.reuseDelay = 16;
		}

		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			Vector2 vel = new Vector2(speedX, speedY) * Main.rand.NextFloat(0.95f, 1.05f);
			vel = vel.RotatedByRandom(MathHelper.ToRadians(7.5f));
			speedX = vel.X;
			speedY = vel.Y;
			return true;
		}
	}
}