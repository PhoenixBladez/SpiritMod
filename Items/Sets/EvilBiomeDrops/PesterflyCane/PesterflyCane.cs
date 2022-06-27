using Microsoft.Xna.Framework;
using SpiritMod.Projectiles.Magic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Sets.EvilBiomeDrops.PesterflyCane
{
	public class PesterflyCane : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Pesterfly Cane");
			Tooltip.SetDefault("Summons pesterflies that swarm around hit enemies");
			Item.staff[Item.type] = true; //Set here since it's universal for this item
		}

		public override void SetDefaults()
		{
			Item.damage = 14;
			Item.DamageType = DamageClass.Magic;
			Item.mana = 8;
			Item.width = 32;
			Item.height = 32;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.noMelee = true;
			Item.knockBack = 5;
			Item.value = 20000;
			Item.rare = ItemRarityID.Green;
			Item.UseSound = SoundID.Item20;
			Item.autoReuse = true;
			Item.shoot = ModContent.ProjectileType<Pesterfly>();
			Item.shootSpeed = 8f;

			Item.useAnimation = 9;
			Item.useTime = 3;
			Item.reuseDelay = 16;
		}

		public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
		{
			Vector2 vel = velocity * Main.rand.NextFloat(0.95f, 1.05f);
			velocity = vel.RotatedByRandom(MathHelper.ToRadians(7.5f));
		}
	}
}