using Microsoft.Xna.Framework;
using SpiritMod.Projectiles.Summon.LaserGate;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;

namespace SpiritMod.Items.Equipment
{
	public class DynastyFan : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Dynasty Fan");
			Tooltip.SetDefault("Launch yourself in any direction with a gust of wind");
		}

		public override void SetDefaults()
		{
			Item.width = 44;
			Item.height = 48;
			Item.useTime = 100;
			Item.useAnimation = 100;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.staff[Item.type] = true;
			Item.noMelee = true;
			Item.value = 20000;
			Item.rare = ItemRarityID.Orange;
			Item.UseSound = SoundID.Item20;
			Item.autoReuse = false;
			Item.shoot = ProjectileID.WoodenArrowFriendly;
			Item.shootSpeed = 12f;
		}
		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) 
		{
			if (!player.HasBuff(BuffID.Featherfall)) {
                player.AddBuff(ModContent.BuffType<Buffs.DynastyFanBuff>(), 120);
				player.velocity = -velocity;
				int ing = Gore.NewGore(source, player.Center, player.velocity * 4, 825);
				Main.gore[ing].timeLeft = Main.rand.Next(30, 90);
				int ing1 = Gore.NewGore(source, player.Center, player.velocity * 4, 826);
				Main.gore[ing1].timeLeft = Main.rand.Next(30, 90);
				int ing2 = Gore.NewGore(source, player.Center, player.velocity * 4, 827);
				Main.gore[ing2].timeLeft = Main.rand.Next(30, 90);
			}

			return false;
		}

	}
}