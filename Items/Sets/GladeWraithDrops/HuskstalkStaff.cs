using SpiritMod.Items.Material;
using Terraria;
using Microsoft.Xna.Framework;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Sets.GladeWraithDrops
{
	public class HuskstalkStaff : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Huskstalk Staff");
			Tooltip.SetDefault("Shoots consecutive leaves");
		}


		public override void SetDefaults()
		{
			Item.damage = 11;
			Item.DamageType = DamageClass.Magic;
			Item.mana = 11;
			Item.width = 40;
			Item.height = 40;
			Item.useTime = 10;
			Item.useAnimation = 30;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.staff[Item.type] = true;
			Item.noMelee = true;
			Item.knockBack = 6;
			Item.useTurn = false;
			Item.value = Terraria.Item.sellPrice(0, 0, 15, 0);
			Item.rare = ItemRarityID.Blue;
			Item.UseSound = SoundID.Item20;
			Item.autoReuse = false;
			Item.shoot = ProjectileID.Leaf;
			Item.shootSpeed = 5.5f;
			Item.reuseDelay = 30;
		}

		public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
		{
			Vector2 mouse = Main.MouseWorld;
			Vector2 offset = mouse - player.Center;
			offset.Normalize();
			offset *= 25;
			position += offset;
		}
	}
}
