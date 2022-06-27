using Microsoft.Xna.Framework;
using SpiritMod.Buffs;
using SpiritMod.Projectiles.Magic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Sets.MagicMisc.AstralClock
{
	public class StopWatch : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Astral Clock");
			Tooltip.SetDefault("Creates a clock around the player, stopping time. \nHas a 60 second cooldown");
			Item.staff[Item.type] = false;
		}

		public override void SetDefaults()
		{
			Item.mana = 100;
			Item.width = 40;
			Item.height = 40;
			Item.useTime = 30;
			Item.useAnimation = 30;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.noMelee = true; //so the item's animation doesn't do damage
			Item.knockBack = 5;
			Item.value = Item.sellPrice(0, 2, 0, 0);
			Item.rare = ItemRarityID.Pink;
			Item.UseSound = SoundID.Item20;
			Item.autoReuse = false;
			Item.shoot = ModContent.ProjectileType<Clock>();
			Item.shootSpeed = 0.3f;
		}

		public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
		{
			MyPlayer modPlayer = player.GetSpiritPlayer();
			modPlayer.shootDelay = 3600;
			modPlayer.clockX = (int)position.X;
			modPlayer.clockY = (int)position.Y;
			velocity = Vector2.Zero;
			player.AddBuff(ModContent.BuffType<ClockBuff>(), 200);
		}

		public override bool CanUseItem(Player player)
		{
			MyPlayer modPlayer = player.GetSpiritPlayer();
			if (modPlayer.shootDelay == 0)
				return true;
			return false;
		}
	}
}
