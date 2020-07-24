using Microsoft.Xna.Framework;
using SpiritMod.Projectiles.Summon.LaserGate;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

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
			item.width = 44;
			item.height = 48;
			item.useTime = 100;
			item.useAnimation = 100;
			item.useStyle = ItemUseStyleID.HoldingOut;
			Item.staff[item.type] = true;
			item.noMelee = true;
			item.value = 20000;
			item.rare = ItemRarityID.Orange;
			item.UseSound = SoundID.Item20;
			item.autoReuse = false;
			item.shoot = ModContent.ProjectileType<RightHopper>();
			item.shootSpeed = 12f;
		}
		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			if (!player.HasBuff(BuffID.Featherfall)) {
				player.velocity.X = 0 - speedX;
				player.velocity.Y = 0 - speedY;
				int ing = Gore.NewGore(player.Center, player.velocity * 4, 825);
				Main.gore[ing].timeLeft = Main.rand.Next(30, 90);
				int ing1 = Gore.NewGore(player.Center, player.velocity * 4, 826);
				Main.gore[ing1].timeLeft = Main.rand.Next(30, 90);
				int ing2 = Gore.NewGore(player.Center, player.velocity * 4, 827);
				Main.gore[ing2].timeLeft = Main.rand.Next(30, 90);
			}

			return false;
		}

	}
}