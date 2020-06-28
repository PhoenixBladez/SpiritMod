using SpiritMod.Buffs.Summon;
using SpiritMod.Projectiles.Summon;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
namespace SpiritMod.Items.Weapon.Summon
{
	public class CthulhuStaff1 : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("R'lyehian Scepter");
			Tooltip.SetDefault("Summons a mini R'lyehian to fight for you!");
		}


		public override void SetDefaults()
		{
			item.width = 44;
			item.height = 44;
			item.value = Item.sellPrice(0, 7, 0, 0);
			item.rare = ItemRarityID.Pink;
			item.mana = 12;
			item.damage = 32;
			item.knockBack = 3;
			item.useStyle = ItemUseStyleID.SwingThrow;
			item.useTime = 30;
			item.useAnimation = 30;
			item.summon = true;
			item.noMelee = true;
			item.shoot = ModContent.ProjectileType<Cthulhu>();
			item.buffType = ModContent.BuffType<CthulhuBuff>();
			item.buffTime = 3600;
			item.UseSound = SoundID.Item44;
		}
	}
}