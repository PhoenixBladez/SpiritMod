using SpiritMod.Buffs;
using SpiritMod.Projectiles.Magic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Weapon.Swung
{
	public class EtherealSword : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Ethereal Sword");
			Tooltip.SetDefault("Inflicts Essence Trap");
		}


		public override void SetDefaults()
		{
			item.damage = 49;
			item.melee = true;
			item.width = 42;
			item.height = 42;
			item.useTime = 17;
			item.useAnimation = 17;
			item.useStyle = ItemUseStyleID.SwingThrow;
			item.knockBack = 5;
			item.value = 1000;
			item.rare = 5;
			item.UseSound = SoundID.Item1;
			item.autoReuse = true;
			item.value = Item.buyPrice(0, 4, 0, 0);
			item.value = Item.sellPrice(0, 1, 0, 0);
			item.useTurn = true;
			item.shoot = ModContent.ProjectileType<EtherealSwordProjectile>();
			item.shootSpeed = 6;
			item.crit = 6;
		}
		public override void OnHitNPC(Player player, NPC target, int damage, float knockback, bool crit)
		{
			target.AddBuff(ModContent.BuffType<EssenceTrap>(), 520);
		}
	}
}