using Microsoft.Xna.Framework;
using SpiritMod.Projectiles;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.BossLoot.AvianDrops
{
	public class TalonPiercer : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Talon's Fury");
			Tooltip.SetDefault("Shoots feathers from off screen");
			Item.staff[Item.type] = true;
		}


		public override void SetDefaults()
		{
			Item.damage = 25;
			Item.DamageType = DamageClass.Magic;
			Item.mana = 10;
			Item.width = 46;
			Item.height = 46;
			Item.useTime = 16;
			Item.useAnimation = 16;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.noMelee = true;
			Item.knockBack = 3.5f;
			Item.useTurn = false;
			Item.value = Item.sellPrice(0, 1, 0, 0);
			Item.rare = ItemRarityID.Green;
			Item.UseSound = SoundID.Item20;
			Item.autoReuse = true;
			Item.shoot = ModContent.ProjectileType<BoneFeatherFriendly>();
			Item.shootSpeed = 17f;
		}

		public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
		{
			position -= velocity * 100;
			velocity.X += (Main.rand.Next(-3, 4) / 5f);
			velocity.Y += (Main.rand.Next(-3, 4) / 5f);
		}
	}
}
