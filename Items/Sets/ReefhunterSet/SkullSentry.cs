using Microsoft.Xna.Framework;
using SpiritMod.Items.Sets.ReefhunterSet.Projectiles;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Sets.ReefhunterSet
{
	public class SkullSentry : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Maneater Skull");
			Tooltip.SetDefault("Summons a skull infested by maneater worms that fire red mucus at nearby enemies");
		}

		public override void SetDefaults()
		{
			item.CloneDefaults(ItemID.StaffoftheFrostHydra);
			item.damage = 24;
			item.width = 28;
			item.height = 14;
			item.useTime = item.useAnimation = 30;
			item.knockBack = 2f;
			item.shootSpeed = 0f;
			item.noMelee = true;
			item.autoReuse = true;
			item.sentry = true;
			item.rare = ItemRarityID.Blue;
			item.value = Item.sellPrice(gold: 2);
			item.useStyle = ItemUseStyleID.HoldingUp;
			item.UseSound = SoundID.Item77;
			item.shoot = ModContent.ProjectileType<SkullSentrySentry>();
		}

		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			Vector2 mouse = Main.MouseWorld;
			float maxDist = 600f;
			if (player.Distance(mouse) >= 600f)
				mouse = player.DirectionTo(mouse) * maxDist;

			Projectile.NewProjectile(mouse.X, mouse.Y, speedX, speedY, type, damage, knockBack, player.whoAmI);
			player.UpdateMaxTurrets();
			return false;
		}
	}
}