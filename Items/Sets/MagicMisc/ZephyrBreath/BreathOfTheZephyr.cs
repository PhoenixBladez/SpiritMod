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
			item.damage = 19;
			item.magic = true;
			item.mana = 9;
			item.width = 46;
			item.height = 46;
			item.useTime = 24;
			item.useAnimation = 24;
			item.useStyle = ItemUseStyleID.HoldingOut;
			Item.staff[item.type] = true;
			item.noMelee = true;
			item.knockBack = 5;
			item.value = Terraria.Item.sellPrice(0, 0, 40, 0);
			item.rare = ItemRarityID.Blue;
			item.shoot = ModContent.ProjectileType<Zephyr>();
			item.autoReuse = true;
		}

		public override bool AltFunctionUse(Player player)
		{
			return true;
		}
		public override bool CanUseItem(Player player)
		{
			if (player.altFunctionUse == 2) {
				item.UseSound = SoundID.Item1;
				item.shoot = ModContent.ProjectileType<ZephyrSpearProj>();
				item.knockBack = 5;
				item.shootSpeed = 6f;
				item.noUseGraphic = true;
			}
			else {
				item.UseSound = SoundID.Item34;
				item.shoot = ModContent.ProjectileType<Zephyr>();
				item.knockBack = 10;
				item.noUseGraphic = false;
				item.shootSpeed = 14f;
			}
			if (player.ownedProjectileCounts[ModContent.ProjectileType<ZephyrSpearProj>()] > 0)
				return false;

			return true;
		}

		public override void ModifyManaCost(Player player, ref float reduce, ref float mult) => mult = (player.altFunctionUse == 2) ? 0 : 1;
	}
}
