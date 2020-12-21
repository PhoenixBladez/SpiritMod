using Microsoft.Xna.Framework;
using SpiritMod.Buffs.Summon;
using SpiritMod.Projectiles.Summon.Snapspore;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
namespace SpiritMod.Items.Weapon.Summon
{
	public class SnapsporeStaff : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Snapspore Wand");
			Tooltip.SetDefault("Summons a Snapspore to fight for you\nSnapspores bounce toward enemies and emits poison clouds");
		}

		public override void SetDefaults()
		{
			item.width = 36;
			item.height = 38;
			item.value = Item.sellPrice(0, 0, 75, 0);
			item.rare = ItemRarityID.Green;
			item.mana = 10;
			item.damage = 17;
			item.knockBack = 1;
			item.useStyle = ItemUseStyleID.SwingThrow;
			item.useTime = 30;
			item.useAnimation = 30;
			item.summon = true;
			item.noMelee = true;
			item.shoot = ModContent.ProjectileType<SnapsporeMinion>();
			item.UseSound = new Terraria.Audio.LegacySoundStyle(6, 0);
		}
		public override bool AltFunctionUse(Player player)
		{
			return true;
		}

		public override bool UseItem(Player player)
		{
			if (player.altFunctionUse == 2) {
				player.MinionNPCTargetAim();
			}
			return base.UseItem(player);
		}
		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			player.AddBuff(ModContent.BuffType<SnapsporeBuff>(), 3600);
			return player.altFunctionUse != 2;
		}
	}
}