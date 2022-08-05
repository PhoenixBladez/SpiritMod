using SpiritMod.Projectiles.Held;
using SpiritMod.Projectiles.Magic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Sets.MagicMisc.ZephyrBreath
{
	public class BreathOfTheZephyr : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Breath of the Zephyr");
			Tooltip.SetDefault("Creates a mighty gust of wind to damage your foes and knock them back\nRight-click to thrust like a spear, and leech mana from struck foes");
		}


		public override void SetDefaults()
		{
			Item.damage = 19;
			Item.DamageType = DamageClass.Magic;
			Item.mana = 9;
			Item.width = 46;
			Item.height = 46;
			Item.useTime = 24;
			Item.useAnimation = 24;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.staff[Item.type] = true;
			Item.noMelee = true;
			Item.knockBack = 5;
			Item.value = Item.sellPrice(0, 0, 40, 0);
			Item.rare = ItemRarityID.Blue;
			Item.shoot = ModContent.ProjectileType<Zephyr>();
			Item.autoReuse = true;
		}

		public override bool AltFunctionUse(Player player)
		{
			return true;
		}
		public override bool CanUseItem(Player player)
		{
			if (player.altFunctionUse == 2) {
				Item.UseSound = SoundID.Item1;
				Item.shoot = ModContent.ProjectileType<ZephyrSpearProj>();
				Item.knockBack = 5;
				Item.shootSpeed = 6f;
				Item.noUseGraphic = true;
			}
			else {
				Item.UseSound = SoundID.Item34;
				Item.shoot = ModContent.ProjectileType<Zephyr>();
				Item.knockBack = 10;
				Item.noUseGraphic = false;
				Item.shootSpeed = 14f;
			}
			if (player.ownedProjectileCounts[ModContent.ProjectileType<ZephyrSpearProj>()] > 0)
				return false;

			return true;
		}

		public override void ModifyManaCost(Player player, ref float reduce, ref float mult) => mult = (player.altFunctionUse == 2) ? 0 : 1;
	}
}
