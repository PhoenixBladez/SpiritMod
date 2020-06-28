using SpiritMod.Buffs;
using SpiritMod.Projectiles.Magic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Weapon.Swung
{
	public class VeinDrainer : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Vein Drainer");
			Tooltip.SetDefault("Steals life on hit");
		}


		public override void SetDefaults()
		{
			item.damage = 50;
			item.melee = true;
			item.width = 60;
			item.height = 72;
			item.useTime = 28;
			item.useAnimation = 28;
			item.useStyle = ItemUseStyleID.SwingThrow;
			item.knockBack = 4;
			item.shoot = ModContent.ProjectileType<Moondrainer>();
			item.rare = 5;
			item.UseSound = SoundID.Item1;
			item.autoReuse = true;
			item.value = Item.buyPrice(0, 12, 0, 0);
			item.value = Item.sellPrice(0, 0, 90, 0);
			item.crit = 0;
			item.shootSpeed = 8f;
		}

		public override void OnHitNPC(Player player, NPC target, int damage, float knockBack, bool crit)
		{
			if(Main.rand.Next(3) == 1) {
				player.HealEffect(4);
				player.statLife += 4;
			}
			{
				target.AddBuff(ModContent.BuffType<BCorrupt>(), 180);
			}
		}
	}
}